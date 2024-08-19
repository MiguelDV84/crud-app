using crud_app.Domain;

namespace crud_app.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<bool> Login(string username, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAndSave (User user);
        Task UpdateAndSave (User user);
        Task DeleteAndSave (int id);
    }
}
