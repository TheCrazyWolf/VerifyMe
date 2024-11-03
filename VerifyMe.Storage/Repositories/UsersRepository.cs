using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class UsersRepository(VerifyContext ef)
{
    public async Task<User?> GetUserByPhoneAsync(string phone)
    {
        return await ef.Users.AsNoTracking().FirstOrDefaultAsync(x=> x.PhoneNumber == phone);
    }

    public async Task AddUserAsync(User user)
    {
        await ef.Users.AddAsync(user);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        ef.Update(user);
        await ef.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        ef.Remove(user);
        await ef.SaveChangesAsync();
    }
}