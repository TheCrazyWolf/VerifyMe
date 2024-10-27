using MudBlazor.Services;
using Telegram.Bot;
using VerifyMe.Components;
using VerifyMe.Services.AppsServices;
using VerifyMe.Services.SmsServices;
using VerifyMe.Services.UsersServices;
using VerifyMe.Storage;
using VerifyMe.Storage.Context;
using VerifyMe.Telegram;
using VerifyMe.Telegram.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        var accessTokenTelegram = sp.GetRequiredService<IConfiguration>().GetValue<string>("TelegramBotAccessToken");
        ArgumentNullException.ThrowIfNull(accessTokenTelegram, nameof(accessTokenTelegram));
        TelegramBotClientOptions options = new(accessTokenTelegram);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
builder.Services.AddHostedService<PollingService>();
builder.Services.AddScoped<VerifyContext>();
builder.Services.AddScoped<VerifyStorage>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<SmsService>();
builder.Services.AddScoped<AppsServices>();

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Маршруты для Web API
app.MapControllers();

app.Run();