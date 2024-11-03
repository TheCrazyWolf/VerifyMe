using System.Text;

namespace VerifyMe.ApiClient.Extensions;

public class VerificationCodeGenerator
{
    private readonly Random _random = new Random();

    // Параметры для настройки генерации
    public bool UseDigits { get; set; } = true;
    public bool UseSpecialCharacters { get; set; } = false;
    public bool UseLetters { get; set; } = false;
    public bool UseUppercaseLetters { get; set; } = false;
    public bool UseLowercaseLetters { get; set; } = false;

    private const string Digits = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?";
    private const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

    public string GenerateCode(int length)
    {
        // Построение пула символов на основе параметров
        StringBuilder characterPool = new StringBuilder();

        if (UseDigits)
            characterPool.Append(Digits);

        if (UseSpecialCharacters)
            characterPool.Append(SpecialCharacters);

        if (UseLetters)
        {
            if (UseUppercaseLetters) characterPool.Append(UppercaseLetters);

            if (UseLowercaseLetters) characterPool.Append(LowercaseLetters);
        }

        if (length == 0)
            throw new InvalidOperationException("Необходимо указать хотя бы один тип символов для генерации кода.");

        // Генерация кода нужной длины
        StringBuilder code = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            int index = _random.Next(characterPool.Length);
            code.Append(characterPool[index]);
        }

        return code.ToString();
    }
}