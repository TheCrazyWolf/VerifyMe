using VerifyMe.Models.DLA;
using VerifyMe.Storage;

namespace VerifyMe.Services.AppsServices;

public class AppsServices(VerifyStorage storage)
{
    public async Task<App?> GetAppAsync(long appId)
    {
        return await storage.Apps.GetAppByIdAsync(appId);
    }
    
    public async Task<App?> GetAppByAccessTokenAsync(string accessToken)
    {
        return await storage.Apps.GetAppByAccessTokenAsync(accessToken);
    }
    
    public async Task CreateAppAsync(App app)
    {
        app.DateTimeCreated = DateTime.Now;
        await storage.Apps.CreateAppAsync(app);
    }

    public async Task UpdateAppAsync(App app)
    {
        await storage.Apps.UpdateAppAsync(app);
    }

    public async Task RemoveAppAsync(App app)
    {
        await storage.Apps.RemoveAppAsync(app);
    }

    public async Task<IList<App>> GetApps()
    {
        return await storage.Apps.GetAppsAsync();
    }
}