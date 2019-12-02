using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using DatingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public async Task<User> Register(User user, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.AddAsync(user);
            await _dataContext.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(us => us.Name == username);
            if (user is null)
            {
                return null;
            }

            return !VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }

        private bool VerifyPassword(string enteredPassword, byte[] actualPasswordHash, byte[] actualPasswordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(actualPasswordSalt);
            var enteredPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword));
            return actualPasswordHash.SequenceEqual(enteredPasswordHash);
        }
        

        public async Task<bool> UserExist(string username)
        {
            return await _dataContext.Users.AnyAsync(us => us.Name == username);
            
        }
    }
}