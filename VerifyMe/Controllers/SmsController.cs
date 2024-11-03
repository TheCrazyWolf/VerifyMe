using Microsoft.AspNetCore.Mvc;
using VerifyMe.Models.DTO;
using VerifyMe.Models.DTO.Sms;
using VerifyMe.Services.AppsServices;
using VerifyMe.Services.SmsServices;

namespace VerifyMe.Controllers;

[Route("api/sms")]
[ApiController]
public class SmsController(SmsService smsService, AppsServices appsServices) : ControllerBase
{
    [HttpGet("send")]
    public async Task<SmsResult> Send(string accessToken, string phone, string message)
    {
        return await Send(accessToken: accessToken, new PostSendSms(phone: phone, message: message));
    }
    
    [HttpPost("send")]
    public async Task<SmsResult> Send([FromHeader] string accessToken, [FromBody] PostSendSms dto)
    {
        var application = await appsServices.GetAppByAccessTokenAsync(accessToken: accessToken);
        if(application == null) return new SmsResult(isSuccess: false, systemMessage: "Доступ запрещен. Проверьте передачу токена в заголовке AccessToken");
        return await smsService.SendSmsAsync(app: application, phoneNumber: dto.Phone, message: dto.Message);
    }
    
    [HttpPost("sendMany")]
    public async Task<IList<SmsResultExtended>> SendMany([FromHeader] string accessToken, [FromBody] IList<PostSendSms> dto)
    {
        var application = await appsServices.GetAppByAccessTokenAsync(accessToken: accessToken);
        if (application == null) return new List<SmsResultExtended> { new ( isSuccess: false, phone: string.Empty, systemMessage: "Доступ запрещен. Проверьте передачу токена в заголовке AccessToken")};
        return await smsService.SendSmsAsync(app: application, dtos: dto);
    }
}