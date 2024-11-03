using RestSharp;
using VerifyMe.ApiClient.Common;
using VerifyMe.ApiClient.Models.Post.Sms;
using VerifyMe.ApiClient.Models.Resuls.Sms;

namespace VerifyMe.ApiClient.Sections;

public class SmsService(string url, string applicationToken) : VerifyBase(url, applicationToken)
{
    private readonly string route = "/api/sms";
    
    public async Task<SmsResult?> SendSmsAsync(SendSms dto)
    {
        var options = new RestRequest($"{route}/sms", Method.Post);
        options.AddOrUpdateHeader("accessToken", applicationToken);
        options.AddBody(dto);
        return await Client.PostAsync<SmsResult>(options);
    }
    
    public async Task<SmsResult?> SendSmsManyAsync(IList<SendSms> dto)
    {
        var options = new RestRequest($"{route}/sendMany", Method.Post);
        options.AddOrUpdateHeader("accessToken", applicationToken);
        options.AddBody(dto);
        return await Client.PostAsync<SmsResult>(options);
    }
}