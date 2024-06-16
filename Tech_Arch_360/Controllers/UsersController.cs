using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tech_Arch_360.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly Tech_Arc_360Context _context;
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public UsersController(Tech_Arc_360Context context, IConfiguration configuration, SymmetricSecurityKey key)
    {
        _context = context;
        _configuration = configuration;
        _key = key;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegister)
    {
        if (await _context.Users.AnyAsync(x => x.Username == userRegister.Username))
            return BadRequest("Username is already taken");

#pragma warning disable CS8601 // Possible null reference assignment.
        var user = new User
        {
            Username = userRegister.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegister.Password),
            Role = userRegister.Role
        };


        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Username == userLogin.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.PasswordHash))
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role,value: user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetUserDetails()
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var userId = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
#pragma warning restore CS8604 // Possible null reference argument.
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Role
        });
    }
}

public class UserRegisterDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
}

public class UserLoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}