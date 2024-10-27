using VerifyMe.Models.DLA;
using VerifyMe.Storage;

namespace VerifyMe.Services.UsersServices;

public class UsersService(VerifyStorage storage)
{
    public async Task AddOrUpdate(User user)
    {
        var userFromStorage = await storage.Users.GetUserByPhone(user.PhoneNumber);

        if (userFromStorage is null)
        {
            await storage.Users.AddUser(user);
            return;
        }
        userFromStorage.Id = user.Id;
        userFromStorage.PhoneNumber = user.PhoneNumber;
        await storage.Users.UpdateUser(userFromStorage);
    }
}