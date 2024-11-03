using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class AppRepository(VerifyContext ef)
{
    public async Task<IList<App>> GetApps()
    {
        return await ef.Apps.AsNoTracking().ToListAsync();
    }

    public async Task<App?> GetAppById(long id)
    {
        return await ef.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateApp(App newApp)
    {
        await ef.Apps.AddAsync(newApp);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateApp(App app)
    {
        ef.Apps.Update(app);
        await ef.SaveChangesAsync();
    }

    public async Task RemoveApp(App app)
    {
        ef.Remove(app);
        await ef.SaveChangesAsync();
    }

    public async Task<App?> GetAppByAccessToken(string accessToken)
    {
        return await ef.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.AccessToken == accessToken);
    }
}