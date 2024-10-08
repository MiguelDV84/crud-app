﻿using crud_app.Data;
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
            var password = BCrypt.Net.BCrypt.HashPassword(user.Password_hash);
            user.Password_hash = password;
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

        public async Task<bool> Login(string? username, string? email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Debes proporcionar al menos un nombre de usuario o un correo electrónico.");
            }

            // Buscar el usuario basado en el username o email
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                (username != null && u.Username == username) ||
                (email != null && u.Email == email)
            );

            // Verificar si el usuario existe
            if (user == null)
            {
                return false; // Usuario no encontrado
            }
            var token = this._jwtToken.TokenGenerator(username,email,password);
            return BCrypt.Net.BCrypt.Verify(password, user.Password_hash);
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

    }
}
