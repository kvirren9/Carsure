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

    public void Save(User user)
    {
        var existing = _dbContext.Users.FirstOrDefault(u => u.Id == user.Id);
        if (existing is null)
        {
            _dbContext.Users.Add(user);
        }
        else
        {
            existing.Name = user.Name;
            existing.Email = user.Email;
            existing.PasswordHash = user.PasswordHash;
            existing.Role = user.Role;
        }
        _dbContext.SaveChanges();
    }

    public User? FindByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public User? FindById(int id)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Id == id);
    }

    public IReadOnlyList<User> GetAllUsers()
    {
        return _dbContext.Users.OrderBy(u => u.Name).ToList();
    }

    public bool Register(string name, string email, string password)
    {
        if (FindByEmail(email) is not null)
            return false;

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = UserRole.User
        };

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return true;
    }

    public User? Login(string email, string password)
    {
        var user = FindByEmail(email);

        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }

    public bool SetRole(int userId, UserRole role)
    {
        var user = FindById(userId);
        if (user is null)
            return false;

        user.Role = role;
        _dbContext.SaveChanges();
        return true;
    }


    public bool UpdateProfile(int userId, string name, string email, string? newPassword)
    {
        var user = FindById(userId);
        if (user is null)
            return false;

        var existing = FindByEmail(email);
        if (existing is not null && existing.Id != userId)
            return false;

        user.Name = name;
        user.Email = email;

        if (!string.IsNullOrWhiteSpace(newPassword))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

        _dbContext.SaveChanges();
        return true;
    }
}
