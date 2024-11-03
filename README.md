# VerifyMe - отправка верификационных кодов по номеру телефона, авторизация через Telegram-бота

Этот сервер позволит Вам позволит решить следующие задачи:
1. Реализовать как дополнительный способ авторизации путем использования телеграмма
2. Подтверждение личности и принадлежности номеру конкретному человека
3. Все выше перечисленно бесплатно, при наличии хоста, который способен запускать NET приложения 

Вы можете модифицировать сервер по своему усмотрению, так же интегрировать в своей проект

Существующие решения платны и имеют недостатки по типу "вместо себя отправить контакт любого" и это все прожуётся.

Пример получения кодов            |  Пример подтверждения номера в телеграмме
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Bot/exampleGetCode.png?raw=true)  |  ![](/GitDemoScreenshots/Bot/exampleStart.png?raw=true)

### Предварительная настройка в конфиг файлах
1. Ну, во первых перейдите в BotFather создайте своего ущербного бота и получить токен
2. В файле appsettings.json вставьте токена от телеграм-бота полученного через BotFather в строчку TelegramBotAccessToken
3. ВАЖНО! В этом же файле настройте пароль администратора для доступа к веб-панели в строке AdminPasswordSecurity.
Отнеситесь к этому серьезно, сделайте пароль длинным, обгатите его спец. символами и т.д.
```
{
  "TelegramBotAccessToken": "34523459411:AAH_fpodYu45swnOIME54Q9jN7234Ssj1g",
  "AdminPasswordSecurity" : "e332a76c29654f?cb7f!6e6b31ced090c7befaa450f3d9fd$6be54b6d5ff6eafa91"
}

```

### Предварительная настройка в админ-панели
1. Настройте systemctl (типа служба на линуксе или че там еще)
2. Выполните вход в админ-панель используя пароль из конфига
3. Создайте новое приложение и сохраните ключ доступа к нему

Если у Вас несколько проектов и вы хотите разграничить смс-уведомления, чтобы пользователи понимали
откуда идет СМС или запрос об авторизации

Пример веб-панели            |  Возможность просмотра, что отправлено
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/WebAdmin/exampleWeb1.png?raw=true)  |  ![](/GitDemoScreenshots/WebAdmin/exampleWeb2.png?raw=true)

### Отправка SMS-кода со своего сервиса, используя HTTP-запрос
1. Сформируйте Post-запрос на https://example.com:5002/api/sms/send со следующими данными:
заголовок accessToken = ключ доступа к приложению, который был выдан через веб-панель
В теле запроса:
```json
{
    "Phone": "8(923)-XXX-XX-XX",
    "Message": "Ваш код <b>103-301</b> для подтверждении оплаты внутри-игровой валюты. Не передавайте этот код посторонним."
}
```
3. Получите в ответ SmsResult, который содержит информацию об успешности запроса и системным сообщением

Пример успешной отправки            |  Пример возращаемых данных в случае неудачи
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Requests/exampleRequest1.png?raw=true)  |  ![](/GitDemoScreenshots/Requests/exampleRequest2.png?raw=true)

### Авторизация через Телеграм по номеру телефона со своего сервиса, используя HTTP-запрос
1. Сформируйте Post-запрос на https://example.com:5002/api/challenge/auth со следующими данными:
   заголовок accessToken = ключ доступа к приложению, который был выдан через веб-панель
В теле запроса:
```json
{
    "Phone": "8(923)-XXX-XX-XX"
}
```
3. Получите в ответ ChallengeAuthResult, который содержит информацию об успешности запроса и системным  сообщением и данных пользователя

Внимание! Тайм-аут 1 минута, если пользователь не нажал авторизоваться, будет возращен ChallengeAuthResult c информацией об неудачи


Пример сообщения при попытки авторизоваться            |  Пример возращаемых данных в случае авторизации
:-------------------------:|:-------------------------:
![](/GitDemoScreenshots/Auth/AuthExample1.png?raw=true)  |  ![](/GitDemoScreenshots/Auth/AuthExample2.png?raw=true)

### Другие вариации запросов
Ну, если Вы совсем прям из жопы не смогли сформировать запрос, можно воспользоваться GET-запросом

Для отправки СМС-сообщения
```http request
https://example.com:5002/api/sms/send?accessToken=token&phone=79991231212&message=Ваш СМС код 123-123
```
Для авторизации
```http request
https://example.com:5002/api/challenge/auth?accessToken=token&phone=79991231212
```

### API-клиент для запросов
Если Вы ведет разработку на .NET, вы можете использовать клиент в комплекте. Для Всех остальных случаев, Вы можете
использовать свои HTTP клиенты для выполнения запросов
1. Положите nuget пакет в своей репозиторий
2. Установите

#### Клиент в проекте
Пример создания экземпляра клиента в проекте
```csharp
var host = "http://127.0.0.1:5002";
var accessTokenApp = "4162f1e9899c47229125305a59304535";
var phoneForExample = "79991231212";

VerifyApi verifyApi = new VerifyApi(host, accessTokenApp);
```
или
#### Добавление сервиса в DI
```csharp
builder.Services.AddScoped<VerifyApi>(opt =>
{
    var config = opt.GetRequiredService<IConfiguration>();
    var url = config.GetValue<string>("VerifyMe:ApiUrl");
    var key = config.GetValue<string>("VerifyMe:ApiKey");
    ArgumentNullException.ThrowIfNull(url, "VerifyMe:ApiUrl");
    ArgumentNullException.ThrowIfNull(key, "VerifyMe:ApiKey");
    return new VerifyApi(url: url, applicationToken: key);
});
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

### Пользователям, получающие смс-коды
1. Прежде чем сделать запрос. Предупредите пользователя о, том что необходимо перейти в телеграм-бота для подтверждения номера телефона.
2. После чего, предложите на своей платформе ввести пользователю номер телефона
3. Выполните запрос и получите ответ
