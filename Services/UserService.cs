using Carsure.Data;
using Carsure.Models;

namespace Carsure.Services;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Register(string name, string email, string password)
    {
        if (_dbContext.Users.Any(u => u.Email == email))
            return false;

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return true;
    }

    public User? Login(string email, string password)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);

        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}
