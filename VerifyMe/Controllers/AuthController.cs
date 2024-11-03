using Microsoft.AspNetCore.Mvc;
using VerifyMe.Models.DTO.ChallengeAuth;
using VerifyMe.Services.AppsServices;
using VerifyMe.Services.AuthServices;
using VerifyMe.Services.SmsServices;

namespace VerifyMe.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(SmsService smsService, AppsServices appsServices, AuthService authService) : ControllerBase
{
    [HttpPost("Auth")]
    public async Task<ChallengeAuthResult> Auth([FromHeader] string accessToken, [FromBody] DtoPhoneAuth dto)
    {
        var application = await appsServices.GetAppByAccessToken(accessToken);
        if (application == null) return new ChallengeAuthResult(isSuccess: false, systemMessage: "Доступ запрещен. Проверьте передачу токена в заголовке AccessToken");
        var user = await authService.GetUserByPhoneNumber(dto.Phone);
        if (user == null) return new ChallengeAuthResult(isSuccess: false, systemMessage: "Пользователь не зарегистрирован в телеграм-боте");        
        await authService.RejectInActiveChallenges();
        var challengeAuth = await authService.CreateChallengeAuth(application, user);
        var smsResult = await smsService.SendSmsRequestAuth(application, challengeAuth, user);
        if (!smsResult.IsSuccess) return new ChallengeAuthResult(isSuccess: smsResult.IsSuccess, systemMessage: smsResult.SystemMessage); 
        return await authService.WaitResultOfChallenge(challengeAuth, 60);
    }

    public class DtoPhoneAuth
    {
        public string Phone { get; set; } = string.Empty;
    }
}