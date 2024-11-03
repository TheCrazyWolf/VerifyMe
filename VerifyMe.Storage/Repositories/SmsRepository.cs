using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class SmsRepository(VerifyContext ef)
{
    public async Task<IList<Sms>> GetSmsByAppIdAsync(long appId)
    {
        return await ef.Sms
            .AsNoTracking()
            .Include(x=> x.User)
            .Where(x => x.AppId == appId)
            .OrderByDescending(x=> x.DateTimeSend)
            .ToListAsync();
    }

    public async Task CreateSmsAsync(Sms sms)
    {
        await ef.Sms.AddAsync(sms);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateSmsAsync(Sms sms)
    {
        ef.Sms.Update(sms);
        await ef.SaveChangesAsync();
    }

    public async Task RemoveSmsAsync(Sms sms)
    {
        ef.Remove(sms);
        await ef.SaveChangesAsync();
    }
}