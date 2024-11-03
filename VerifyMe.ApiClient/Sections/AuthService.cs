using RestSharp;
using VerifyMe.ApiClient.Common;
using VerifyMe.ApiClient.Models.Post.Auth;
using VerifyMe.ApiClient.Models.Resuls.Auth;

namespace VerifyMe.ApiClient.Sections;

public class AuthService(string url, string applicationToken) : VerifyBase(url, applicationToken)
{
    private readonly string route = "/api/challenge";
    
    public async Task<ChallengeAuthResult?> Auth(DtoPhoneAuth dto)
    {
        var options = new RestRequest($"{route}/auth", Method.Post);
        options.AddOrUpdateHeader("accessToken", applicationToken);
        options.AddBody(dto);
        return await Client.PostAsync<ChallengeAuthResult>(options);
    }
    
}