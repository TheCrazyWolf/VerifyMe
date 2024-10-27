using Microsoft.EntityFrameworkCore;
using VerifyMe.Models.DLA;
using VerifyMe.Storage.Context;

namespace VerifyMe.Storage.Repositories;

public class UsersRepository(VerifyContext ef)
{
    public async Task<User?> GetUserByPhone(string phone)
    {
        return await ef.Users.FirstOrDefaultAsync(x=> x.PhoneNumber == phone);
    }

    public async Task AddUser(User user)
    {
        await ef.Users.AddAsync(user);
        await ef.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        ef.Update(user);
        await ef.SaveChangesAsync();
    }

    public async Task DeleteUser(User user)
    {
        ef.Remove(user);
        await ef.SaveChangesAsync();
    }
}