using VerifyMe.Models.Common;

namespace VerifyMe.Models.DLA;

public class User : Entity
{
    public string PhoneNumber { get; set; } = string.Empty;
}