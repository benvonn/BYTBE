using CornHoleRevamp.Data;
using CornHoleRevamp.Models;
using CornHoleRevamp.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CornHoleRevamp.Controllers
{

    

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordServices _passwordServices;
        private readonly IConfiguration _config;

        public UsersController(AppDbContext context, PasswordServices passwordServices, IConfiguration config)
        {
            _context = context;
            _passwordServices = passwordServices;
            _config = config;
        }
        private string GenerateJwtToken(User user)
        {
            var key = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
                throw new InvalidOperationException("JWT key not configured.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim("id", user.Id.ToString()),
        new Claim("name", user.Name)
};

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // offline valid for 7 days
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] UserRequestDto request)
        {
            // Find user by name
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.Name);
            var offlineToken = GenerateJwtToken(user);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Verify password
            bool isValid = _passwordServices.VerifyPassword(user.PasswordHash, request.Passcode);

            if (!isValid)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }


            // Return user data (without password hash)
            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                offlineToken
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserRequestDto request)
        {
            // Hash the password before saving
            var user = new User
            {
                Name = request.Name,
                PasswordHash = _passwordServices.HashPassword(request.Passcode),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _config["Jwt:Key"];
            var offlineToken = GenerateJwtToken(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, new { id = user.Id, name = user.Name, offlineToken });
        }
    }
}