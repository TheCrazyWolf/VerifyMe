using RestSharp;

namespace VerifyMe.ApiClient.Common;

public class VerifyBase(string url)
{
    protected RestClient Client = new RestClient(url);
}