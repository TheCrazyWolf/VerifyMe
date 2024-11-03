# VerifyMe - отправка верификационных кодов по номеру телефона через Telegram-бота
Пример получения кодов            |  Пример подтверждения номера в телеграмме
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Bot/exampleGetCode.png?raw=true)  |  ![](/GitDemoScreenshots/Bot/exampleStart.png?raw=true)

### Предварительная настройка
1. В файле appsettings.json вставьте токена от телеграм-бота полученного через BotFather в строчку TelegramBotAccessToken
2. В этом же файле настройте пароль администратора для доступа к веб-панели в строке AdminPasswordSecurity
```
{
  "TelegramBotAccessToken": "34523459411:AAH_fpodYu45swnOIME54Q9jN7234Ssj1g",
  "AdminPasswordSecurity" : "e332a76c29654f?cb7f!6e6b31ced090c7befaa450f3d9fd$6be54b6d5ff6eafa91"
}

```

### Предварительная настройка в веб панели
1. Выполните вход
2. Создайте новое приложение и сохраните ключ доступа

Пример веб-панели            |  Возможность просмотра, что отправлено
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/WebAdmin/exampleWeb1.png?raw=true)  |  ![](/GitDemoScreenshots/WebAdmin/exampleWeb2.png?raw=true)

### Отправка Sms кода со своего сервиса
1. Сформируйте Post-запрос на example.com/api/sms/send со следующими данными:
заголовок accessToken = ключ доступа к приложению, корторый выдан через веб-панель
В теле запроса::
```
{
    "Phone": "8(923)-XXX-XX-XX",
    "Message": "Ваш код <b>103-301</b> для подтверждении оплаты внутри-игровой валюты. Не передавайте этот код посторонним."
}
```
3. Получите в ответ SmsResult, который содержит информацию об успешности запроса и системым сообщением

Пример успешной отправки            |  Пример возращаемых данных в случае неудачи
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Requests/exampleRequest1.png?raw=true)  |  ![](/GitDemoScreenshots/Requests/exampleRequest2.png?raw=true)

### Авторизация используя телеграм и номер телефона
1. Сформируйте Post-запрос на example.com/api/auth/auth со следующими данными:
   заголовок accessToken = ключ доступа к приложению, корторый выдан через веб-панель
   В теле запроса::
```
{
    "Phone": "8(923)-XXX-XX-XX",
}
```
3. Получите в ответ ChallengeAuthResult, который содержит информацию об успешности запроса и системым сообщением и данных пользователя

Пример сообщения при попытки авторизоваться            |  Пример возращаемых данных в случае авторизации
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Auth/AuthExample1.png?raw=true)  |  ![](/GitDemoScreenshots/Auth/AuthExample2.png?raw=true)

### Пользователям, получающие смс-коды
1. Необходимо пользователя перенаправить в телеграм-бота для подтверждения номера телефона в нем
2. После чего, предложите на своей платформе ввести пользователю номер телефона
3. Выполните запрос и получите ответ


### Клиент в проекте
Пример создания экземпляра клиента в проекте
```csharp
var host = "http://127.0.0.1:5002";
var accessTokenApp = "4162f1e9899c47229125305a59304535";
var phoneForExample = "79991231212";

VerifyApi verifyApi = new VerifyApi(host, accessTokenApp);
```

Пример отправки смс кодов:
```csharp
var code = new VerificationCodeGenerator().GenerateCode(8);
var sms = new SendSms(phoneForExample, $"Код для проверки <b>{code}</b>");
var smsResult = await verifyApi.Sms.SendSmsAsync(sms);

if (smsResult is not null && smsResult.IsSuccess)
{
    Console.WriteLine($"{smsResult.SystemMessage}. Введите проверочный код:");
    int maxCount = 2;
    for (int i = 0; i <= maxCount; i++)
    {
        if (Console.ReadLine() == code) return;
        Console.WriteLine($"Код неверный. У вас осталось: {maxCount-i} попыток");
    }
}
```

Пример авторизации
```csharp
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
```
