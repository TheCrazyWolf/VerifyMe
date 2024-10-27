using MudBlazor.Services;
using Telegram.Bot;
using VerifyMe.Components;
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