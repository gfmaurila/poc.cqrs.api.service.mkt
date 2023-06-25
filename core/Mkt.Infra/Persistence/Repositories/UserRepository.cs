using Microsoft.EntityFrameworkCore;
using Mkt.Core.Entities;
using Mkt.Core.Repositories;

namespace Mkt.Infra.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MktDbContext _db;
    public UserRepository(MktDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var find = await _db.Users.AsNoTracking().Where(f => f.Id == id).ToListAsync();
        return find.FirstOrDefault();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var find = await _db.Users.AsNoTracking().Where(f => f.Email == email).ToListAsync();
        return find.FirstOrDefault();
    }

    public async Task AddUserAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
    {
        return await _db.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
    }
}
