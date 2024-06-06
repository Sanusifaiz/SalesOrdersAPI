using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SalesOrders.Data;
using SalesOrders.Domains.DTOs;
using SalesOrders.Domains.Models;
using SalesOrders.Services.IServices;
using SalesOrders.Utilities.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesOrders.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _config = configuration;
            _logger = logger;
        }
        public async Task<ResponseMessage<UserRegResponseDTO>> Register(string username, string password)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (existingUser != null)
                {
                    _logger.LogWarning("User registration attempt with existing username: {Username}", username);
                    return ResponseMessage<UserRegResponseDTO>.Fail("Username already exists, please try again");
                }

                var user = new User
                {
                    Username = username,
                    PasswordHash = Utils.HashPassword(password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User registered successfully: {Username}", username);
                return ResponseMessage<UserRegResponseDTO>.Ok("User registered successfully, proceed to login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering a user: {Username}", username);
                throw;
            }
        }

        public async Task<ResponseMessage<UserRegResponseDTO>> Login(string username, string password)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user == null || user.PasswordHash != Utils.HashPassword(password))
                {
                    _logger.LogWarning("Invalid login attempt for username: {Username}", username);
                    return ResponseMessage<UserRegResponseDTO>.Fail("Invalid user name or password, please try again");
                }

                // Generate JWT token
                JwtSecurityTokenHandler tokenHandler;
                SecurityToken token;
                GenerateToken(user, out tokenHandler, out token);

                _logger.LogInformation("User logged in successfully: {Username}", username);
                var loginToken = tokenHandler.WriteToken(token);

                return ResponseMessage<UserRegResponseDTO>.Ok("User logged in successfully", new UserRegResponseDTO { Username = username, Token = loginToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in a user: {Username}", username);
                throw;
            }
        }

        /// <summary>
        /// JWT token generator 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="token"></param>
        private void GenerateToken(User? user, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            token = tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
