using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Models.Enums;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class ChallengesAuthsRepository(VerifyContext ef)
{
    public static readonly int DefaultLifeChallengeInSeconds = 60;
    
    public async Task<ChallengeAuth?> GetChallengeById(string challengeId)
    {
        return await ef.ChallengeAuths
            .AsNoTracking()
            .Include(x=> x.User)
            .Include(x=> x.Application)
            .FirstOrDefaultAsync(x => x.Id == challengeId);
    }

    public async Task<IList<ChallengeAuth>> GetChallengesWithUnknownStatus()
    {
        return await ef.ChallengeAuths
            .AsNoTracking()
            .Where(x => x.Status == ChallengeStatus.Unknown)
            .Where(x => DateTime.Now >= x.Created.AddSeconds(DefaultLifeChallengeInSeconds))
            .ToListAsync();
    }

    public async Task CreateChallenge(ChallengeAuth challenge)
    {
        await ef.AddAsync(challenge);
        await ef.SaveChangesAsync();
    }
    
    public async Task UpdateChallenge(ChallengeAuth challenge)
    {
        ef.Update(challenge);
        await ef.SaveChangesAsync();
    }

    public async Task RemoveChallenge(ChallengeAuth challenge)
    {
        ef.Remove(challenge);
        await ef.SaveChangesAsync();
    }
}