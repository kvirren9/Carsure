// This class acts as a simple in-memory repository for users.
// This should be removed as soon as a more permanent data storage solution is implemented.
using Carsure.Models;

namespace Carsure.Data;

public class InMemoryUserRepository
{
    private int _nextId = 1;

    private readonly List<User> _users = [];

    public IReadOnlyList<User> GetAll() => _users.ToList();

    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);

    public User? GetByEmail(string email) =>
        _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

    public bool EmailExists(string email) => GetByEmail(email) is not null;

    public void Add(User user)
    {
        user.Id = _nextId++;
        _users.Add(user);
    }
}
