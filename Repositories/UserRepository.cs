using crud_app.Data;
using crud_app.Domain;
using Microsoft.EntityFrameworkCore;

namespace crud_app.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        private readonly IJwtToken _jwtToken;

        public UserRepository(UserContext context, IJwtToken jwtToken) 
        {
            _context = context;
            _jwtToken = jwtToken;
        }
    
        public async Task CreateAndSave(User user)
        {
           _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAndSave(int id)
        {
          var user = await _context.Users.FindAsync(id);
            if (user != null) {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            throw new KeyNotFoundException($"Entity with id '{id}' not found");
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                return user;
            }

            throw new KeyNotFoundException($"Entity with id '{id}' not found");
        }


        public async Task UpdateAndSave(User user)
        {
            var userToUpdate = await _context.Users.FindAsync(user.Id) ?? throw new KeyNotFoundException($"Entity with id '{user.Id}' not found");
            userToUpdate.Username = user.Username;
            userToUpdate.Email = user.Email;
            userToUpdate.Password_hash = user.Password_hash;
            userToUpdate.Last_login = user.Last_login;
            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            // Verificar si el usuario existe
            if (user is not null)
            {
                var token = this._jwtToken.TokenGenerator(username, password);

                return token;
            }
            return "Usuario no encontrado";
        }
    }
}
