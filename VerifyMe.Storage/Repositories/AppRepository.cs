using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class AppRepository(VerifyContext ef)
{
    public async Task<IList<App>> GetAppsAsync()
    {
        return await ef.Apps.AsNoTracking().ToListAsync();
    }

    public async Task<App?> GetAppByIdAsync(long id)
    {
        return await ef.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAppAsync(App newApp)
    {
        await ef.Apps.AddAsync(newApp);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateAppAsync(App app)
    {
        ef.Apps.Update(app);
        await ef.SaveChangesAsync();
    }

    public async Task RemoveAppAsync(App app)
    {
        ef.Remove(app);
        await ef.SaveChangesAsync();
    }

    public async Task<App?> GetAppByAccessTokenAsync(string accessToken)
    {
        return await ef.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.AccessToken == accessToken);
    }
}