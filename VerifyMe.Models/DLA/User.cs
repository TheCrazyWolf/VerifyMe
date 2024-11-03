using VerifyMe.Models.Common;

namespace VerifyMe.Models.DLA;

public class User : Entity
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата и время подключения к сервису
    /// </summary>
    public DateTime DateTimeRegister { get; set; }  
    
    /// <summary>
    /// Фамиля
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Имя
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Никнейм
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}