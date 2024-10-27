using VerifyMe.Models.Common;

namespace VerifyMe.Models.DLA;

public class App : Entity
{
    /// <summary>
    /// Ключ доступа к приложению/Сервиса
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>
    /// Названия приложению/Сервиса
    /// </summary>
    public string Name { get; set; } = string.Empty;
}