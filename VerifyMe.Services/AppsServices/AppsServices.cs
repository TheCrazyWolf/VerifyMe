using VerifyMe.Models.DLA;
using VerifyMe.Storage;

namespace VerifyMe.Services.AppsServices;

public class AppsServices(VerifyStorage storage)
{
    public async Task<App?> GetAppAsync(long appId)
    {
        return await storage.Apps.GetAppById(appId);
    }
    
    public async Task<App?> GetAppByAccessToken(string accessToken)
    {
        return await storage.Apps.GetAppByAccessToken(accessToken);
    }
    
    public async Task CreateApp(App app)
    {
        app.DateTimeCreated = DateTime.Now;
        await storage.Apps.CreateApp(app);
    }

    public async Task UpdateApp(App app)
    {
        await storage.Apps.UpdateApp(app);
    }

    public async Task RemoveApp(App app)
    {
        await storage.Apps.RemoveApp(app);
    }

    public async Task<IList<App>> GetApps()
    {
        return await storage.Apps.GetApps();
    }
}