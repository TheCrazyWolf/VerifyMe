using System.ComponentModel.DataAnnotations;

namespace VerifyMe.Models.Common;

public class Entity
{
    [Key] public long Id { get; set; }
}