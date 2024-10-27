using VerifyMe.Models.DLA;
using VerifyMe.Storage;

namespace VerifyMe.Services.AppsServices;

public class AppsServices(VerifyStorage storage)
{
    public async Task<App?> GetAppAsync(long appId)
    {
        return await storage.Apps.GetAppById(appId);
    }
    
    public async Task CreateApp(App app)
    {
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
}