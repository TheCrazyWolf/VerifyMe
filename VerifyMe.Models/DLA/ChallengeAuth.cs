using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VerifyMe.Models.Enums;

namespace VerifyMe.Models.DLA;

public class ChallengeAuth 
{
    [Key] public Guid Id { get; set; }
    /// <summary>
    /// ID Пользователя
    /// </summary>
    public long UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }
    /// <summary>
    /// Приложение/Сервис
    /// </summary>
    public long? ApplicationId { get; set; }
    [ForeignKey("ApplicationId")] public App? Application { get; set; }
    /// <summary>
    /// Дата и время начала авторизации
    /// </summary>
    public DateTime Created { get; set; }
    /// <summary>
    /// Статус заявки на вход
    /// </summary>
    public ChallengeStatus Status { get; set; }
}