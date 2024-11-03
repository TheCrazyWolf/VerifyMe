using VerifyMe.ApiClient.Sections;

namespace VerifyMe.ApiClient;

public class VerifyApi(string url, string applicationToken)
{
    public AuthService Auth { get; private set; } = new AuthService(url, applicationToken);
    public SmsService Sms { get; private set; } = new SmsService(url, applicationToken);
}