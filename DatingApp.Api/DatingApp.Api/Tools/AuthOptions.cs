using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Api.Tools
{
    public class AuthOptions
    {
        private readonly byte[] _configKey;

        public AuthOptions(IConfiguration configuration)
        {
            _configKey = Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value);
        }

        
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(_configKey);
        }
    }
}