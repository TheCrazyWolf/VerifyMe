using System.Text.RegularExpressions;

namespace VerifyMe.Services.Extensions;

public static class PhoneNormalizer
{
    public static string GetNormalizedPhoneNumber(this string phoneNumber)
    {
        phoneNumber = Regex.Replace(phoneNumber, @"[^\d]", "");
        
        // Заменяем первую цифру 8 на +7
        if (phoneNumber.StartsWith("8"))
            phoneNumber = "+7" + phoneNumber.Substring(1);
        else if (phoneNumber.StartsWith("7"))
            phoneNumber = "+" + phoneNumber;

        return phoneNumber;
    }
}