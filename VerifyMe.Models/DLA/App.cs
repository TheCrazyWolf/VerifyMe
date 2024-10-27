using VerifyMe.Models.Common;

namespace VerifyMe.Models.DLA;

public class App : Entity
{
    /// <summary>
    /// Ключ доступа к приложения/Сервиса
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    /// <summary>
    /// Названия приложения/Сервиса
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Описание приложения/Сервиса
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Дата и время создания приложения/Сервиса
    /// </summary>
    public DateTime DateTimeCreated { get; set; }
}