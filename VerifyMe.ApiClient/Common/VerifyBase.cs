using RestSharp;

namespace VerifyMe.ApiClient.Common;

public class VerifyBase(string url, string applicationToken)
{
    protected RestClient Client = new RestClient(url);
}