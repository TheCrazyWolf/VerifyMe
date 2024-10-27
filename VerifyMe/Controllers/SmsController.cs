using Microsoft.AspNetCore.Mvc;
using VerifyMe.Models.DTO;
using VerifyMe.Services.AppsServices;
using VerifyMe.Services.SmsServices;

namespace VerifyMe.Controllers;

[Route("api/sms")]
[ApiController]
public class SmsController(SmsService smsService, AppsServices appsServices) : ControllerBase
{
    [HttpPost("Send")]
    public async Task<IActionResult> Send([FromHeader] string accessToken, [FromBody] PostSendSms dto)
    {
        var application = await appsServices.GetAppByAccessToken(accessToken);
        if(application == null) return Unauthorized("Доступ запрещен. Проверьте передачу токена в заголовке AccessToken");

        var resultInCode = await smsService.SendSmsAsync(application, dto.Phone, dto.Message);
        
        if(resultInCode is 200) return Ok("Сообщение отправлено");
        if(resultInCode is 404) return NotFound("Пользователь не зарегистрирован. Перейдите в телеграм бот.");

        return Ok();
    }
}