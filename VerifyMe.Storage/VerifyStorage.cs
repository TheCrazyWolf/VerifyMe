using VerifyMe.Storage.Context;
using VerifyMe.Storage.Repositories;

namespace VerifyMe.Storage;

public class VerifyStorage(VerifyContext ef)
{
    public AppRepository Apps { get; set; } = new AppRepository(ef);
    public SmsRepository Sms { get; set; } = new SmsRepository(ef);
    public UsersRepository Users { get; set; } = new UsersRepository(ef);
    public ChallengesAuthsRepository ChallengesAuths { get; set; } = new ChallengesAuthsRepository(ef);
}