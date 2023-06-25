using Mkt.Core.Entities;

namespace Mkt.Core.Repositories;
public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task SaveChangesAsync();
    Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
}
