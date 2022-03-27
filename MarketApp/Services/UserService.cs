using MarketApp.DataTransferObjects;
using MarketApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MarketApp.Services
{
    public class UserService : IUserService
    {
        private readonly MarketContext _context;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConfiguration _configuration;

        public UserService(MarketContext context,
                           IHttpContextAccessor httpAccessor,
                           IConfiguration configuration)
        {
            _context = context;
            _httpAccessor = httpAccessor;
            _configuration = configuration;
        }


        public async Task<string> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user is null)
                throw new ArgumentException(" Username is not exist");

            bool checkUserIdentity = CheckUserIdentity(user, request);

            if (!checkUserIdentity)
            {
                throw new InvalidOperationException("Incorrect password");
            }

            string key = _configuration.GetSection("AppSettings:Token").Value;
            string jwt = GetUserToken(user, key);

            return jwt;
        }

        public string LoginToAdminAccount(UserDto request)
        {
            string username = _configuration.GetSection("Admin:UserName").Value;
            string password = _configuration.GetSection("Admin:Password").Value;

            if (!(username == request.UserName && password == request.Password))
            {
                throw new InvalidOperationException("Incorrect username or password");
            }

            string key = _configuration.GetSection("AppSettings:Token").Value;
            string jwt = GetAdminToken(key);

            return jwt;
        }

        public async Task Register(UserDto request, double money)
        {
            var existUsername = _context.Users.Any(x => x.UserName == request.UserName);

            if (existUsername)
                throw new ArgumentException("Username is already exist");

            CreateUserHash(request.Password, out byte[] hash, out byte[] salt);

            User user = new User()
            {
                UserName = request.UserName,
                PasswordHash = hash,
                PasswordSalt = salt,
                Money = money
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task BuyProduct(int productId, int amount)
        {
            if (amount <= 0)
                throw new ArgumentException(" incorrect amount ");

            var product = await _context.Market.FindAsync(productId);

            if (product is null)
                throw new ArgumentException(" incorrect product id ");


            double price = product.Price * amount;

            if (_httpAccessor.HttpContext is null)
            {
                throw new Exception(" exception ");
            }

            var userIdName = _httpAccessor.HttpContext.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            var userId = int.Parse(userIdName);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (price > user.Money)
                throw new InvalidOperationException(" You dont have enough money to buy it");

            user.Money -= price;

            var currentAmount = product.Amount - amount;

            if (currentAmount <= 0)
                _context.Market.Remove(product);
            else
                product.Amount = currentAmount;

            await _context.SaveChangesAsync();
        }



        private bool CheckUserIdentity(User user, UserDto request)
        {

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var result = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            return user.PasswordHash.SequenceEqual(result);
        }

        private void CreateUserHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();

            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private string GetUserToken(User user, string key)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "User")
            };

            string jwt = CreateJWT(claims, key);
            return jwt;
        }

        private string GetAdminToken(string key)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "Admin")
            };

            string jwt = CreateJWT(claims, key);
            return jwt;
        }

        private string CreateJWT(List<Claim> claims, string key)
        {
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var cred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

    }
}
