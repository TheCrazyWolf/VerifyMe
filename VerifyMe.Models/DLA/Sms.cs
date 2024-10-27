using System.ComponentModel.DataAnnotations.Schema;
using VerifyMe.Models.Common;

namespace VerifyMe.Models.DLA;

public class Sms : Entity
{
    /// <summary>
    /// Кому отправлено
    /// </summary>
    public long? UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }
    
    /// <summary>
    /// Через какое приложение/сервис
    /// </summary>
    public long? AppId { get; set; }
    [ForeignKey("AppId")] public App? App { get; set; }
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Доставлен ?
    /// </summary>
    public bool IsDelivered { get; set; }
}