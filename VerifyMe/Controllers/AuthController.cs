using Microsoft.AspNetCore.Mvc;
using VerifyMe.Models.DTO.ChallengeAuth;
using VerifyMe.Services.AppsServices;
using VerifyMe.Services.AuthServices;
using VerifyMe.Services.SmsServices;

namespace VerifyMe.Controllers;

[Route("api/challenge")]
[ApiController]
public class AuthController(SmsService smsService, AppsServices appsServices, AuthService authService) : ControllerBase
{
    [HttpPost("auth")]
    public async Task<ChallengeAuthResult> Auth([FromHeader] string accessToken, [FromBody] DtoPhoneAuth dto)
    {
        var application = await appsServices.GetAppByAccessTokenAsync(accessToken: accessToken);
        if (application == null) return new ChallengeAuthResult(isSuccess: false, systemMessage: "Доступ запрещен. Проверьте передачу токена в заголовке AccessToken");
        var user = await authService.GetUserByPhoneNumberAsync(dto.Phone);
        if (user == null) return new ChallengeAuthResult(isSuccess: false, systemMessage: "Пользователь не зарегистрирован в телеграм-боте");        
        var challengeAuth = await authService.CreateChallengeAuth(application: application, user: user);
        var smsResult = await smsService.SendSmsRequestAuthAsync(application, challengeAuth, user);
        if (!smsResult.IsSuccess) return new ChallengeAuthResult(isSuccess: smsResult.IsSuccess, systemMessage: smsResult.SystemMessage); 
        return await authService.WaitResultOfChallengeAsync(challengeAuth, attempts: 60);
    }
    
    [HttpGet("auth")]
    public async Task<ChallengeAuthResult> Auth(string accessToken, string phone)
    {
        return await Auth(accessToken: accessToken, new DtoPhoneAuth(phone: phone));
    }
}