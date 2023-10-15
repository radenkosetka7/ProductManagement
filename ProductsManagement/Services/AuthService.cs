using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ProductsManagement.Data;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProductsManagement.Services
{
    public interface IAuthService
    {
        Task<UserDTO> Register(RegisterRequest registerRequest);
        Task<string> Login(LoginRequest loginRequest);
        Task<UserDTO> GetUser(Guid id);
    }
    public class AuthService : IAuthService
    {
        private readonly ProductManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        private IConfiguration _config;

        public AuthService(ProductManagementDbContext dbContext, IMapper mapper, IConfiguration config)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
        }

        public async Task<string> Login(LoginRequest loginRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == loginRequest.Email);
            if (user == null)
            {
                return "User with given e-mail does not exist!";
            }
            if (!VerifyPasswordHash(loginRequest.Password, user.Password, user.Salt))
            {
                return "Wrong password!";
            }
            return GenerateToken(user);
        }

        public async Task<UserDTO> Register(RegisterRequest registerRequest)
        {
            var userTemp = _dbContext.Users.FirstOrDefault(x => x.Email == registerRequest.Email);
            if(userTemp != null)
            {
                return null;
            }
            var user = _mapper.Map<User>(registerRequest);
            CreatePasswordHash(registerRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Password = passwordHash;
            user.Salt = passwordSalt;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUser(Guid id)
        {
            return (await _dbContext.Users.FindAsync(id)) is var user ?
                 _mapper.Map<UserDTO>(user) :
                 null;
        }

        private string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("id", user.Id.ToString()));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var issuer = _config["Jwt:Issuer"];
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
