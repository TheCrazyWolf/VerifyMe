using VerifyMe.Models.DLA;
using VerifyMe.Storage;

namespace VerifyMe.Services.UsersServices;

public class UsersService(VerifyStorage storage)
{
    public async Task AddOrUpdateAsync(User user)
    {
        var userFromStorage = await storage.Users.GetUserByPhoneAsync(user.PhoneNumber);

        if (userFromStorage is null)
        {
            user.DateTimeRegister = DateTime.Now;
            user.FirstName = user.FirstName;
            user.LastName = user.LastName;
            user.UserName = user.UserName;
            await storage.Users.AddUserAsync(user);
            return;
        }
        userFromStorage.Id = user.Id;
        userFromStorage.FirstName = user.FirstName;
        userFromStorage.LastName = user.LastName;
        userFromStorage.UserName = user.UserName;
        userFromStorage.PhoneNumber = user.PhoneNumber;
        await storage.Users.UpdateUserAsync(userFromStorage);
    }
}