using VerifyMe.ApiClient;
using VerifyMe.ApiClient.Extensions;
using VerifyMe.ApiClient.Models.Post.Auth;
using VerifyMe.ApiClient.Models.Post.Sms;

var host = "http://127.0.0.1:5002";
var accessTokenApp = "4162f1e9899c47229125305a59304535";
var phoneForExample = "79991231212";

VerifyApi verifyApi = new VerifyApi(host, accessTokenApp);

/* пример отправки верификационных кодов */
var code = new VerificationCodeGenerator().GenerateCode(8);
var sms = new SendSms(phoneForExample, $"Код для проверки <b>{code}</b>");
var smsResult = await verifyApi.Sms.SendSmsAsync(sms);

if (smsResult is not null)
{
    Console.WriteLine($"{smsResult.SystemMessage}. Введите проверочный код:");
    int maxCount = 2;
    for (int i = 0; i <= maxCount; i++)
    {
        if (Console.ReadLine() == code) return;
        Console.WriteLine($"Код неверный. У вас осталось: {maxCount-i} попыток");
    }
}

/* пример авторизации через бота */
var dtoPhone = new DtoPhoneAuth(phoneForExample);
Console.WriteLine($"Отправили запрос на авторизаци. Перейдите в бота и нажмите нужное действие");
var challengeAuthResult = await verifyApi.Auth.Auth(dtoPhone);

if (challengeAuthResult is not null)
{
    Console.WriteLine($"{challengeAuthResult.SystemMessage}");
    if (challengeAuthResult.IsSuccess &&  challengeAuthResult.User is not null)
    {
        Console.WriteLine($"Вы авторизировались как: {challengeAuthResult.User.Username}");
    }
}
