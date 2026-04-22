using Carsure.Data;
using Carsure.Models;

namespace Carsure.Services;

public class UserService
{
    private readonly InMemoryUserRepository _userRepository;

    public UserService(InMemoryUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public bool Register(string name, string email, string password)
    {
        if (_userRepository.EmailExists(email))
            return false;

        var user = new User
        {
            Name = name,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _userRepository.Add(user);
        return true;
    }

    public User? Login(string email, string password)
    {
        var user = _userRepository.GetByEmail(email);

        if (user is null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        return user;
    }
}
