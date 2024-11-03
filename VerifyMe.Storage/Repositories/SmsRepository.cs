using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class SmsRepository(VerifyContext ef)
{
    public async Task<IList<Sms>> GetSmsByAppId(long appId)
    {
        return await ef.Sms
            .AsNoTracking()
            .Include(x=> x.User)
            .Where(x => x.AppId == appId)
            .OrderByDescending(x=> x.DateTimeSend)
            .ToListAsync();
    }

    public async Task CreateSms(Sms sms)
    {
        await ef.Sms.AddAsync(sms);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateSms(Sms sms)
    {
        ef.Sms.Update(sms);
        await ef.SaveChangesAsync();
    }

    public async Task RemoveSms(Sms sms)
    {
        ef.Remove(sms);
        await ef.SaveChangesAsync();
    }
}