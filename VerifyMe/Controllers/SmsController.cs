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
        var application = await appsServices.GetAppByAccessToken(accessToken);
        if(application == null) return new SmsResult(false,"Доступ запрещен. Проверьте передачу токена в заголовке AccessToken");
        return await smsService.SendSmsAsync(application, dto.Phone, dto.Message);
    }
    
    [HttpPost("sendMany")]
    public async Task<IActionResult> SendMany([FromHeader] string accessToken, [FromBody] IList<PostSendSms> dto, 
        CancellationToken cancellationToken)
    {
        var application = await appsServices.GetAppByAccessToken(accessToken);
        if(application == null) return Ok(new SmsResult(false,"Доступ запрещен. Проверьте передачу токена в заголовке AccessToken"));
        cancellationToken.ThrowIfCancellationRequested();
        return Ok(await smsService.SendSmsAsync(application, dto));
    }
}