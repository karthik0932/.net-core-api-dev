using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tech_Arch_360.Models;
using System.Threading.Tasks;

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
        if (await _context.UserMasters.AnyAsync(x => x.UserName == userRegister.Username))
            return BadRequest("Username is already taken");

        // Validate RoleName and retrieve or create RoleMaster entry
        var role = await _context.RoleMasters.FirstOrDefaultAsync(r => r.RoleName == userRegister.RoleName);
        if (role == null)
        {
            role = new RoleMaster { RoleName = userRegister.RoleName };
            _context.RoleMasters.Add(role);
            await _context.SaveChangesAsync();  // Save RoleMaster entry
        }

        // Validate TenantId
        var tenant = await _context.TenantMasters.FindAsync(userRegister.TenantId);
        if (tenant == null)
            return BadRequest("Invalid Tenant ID");

        var user = new UserMaster
        {
            UserName = userRegister.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(userRegister.Password),
            RoleId = role.RoleId,  // Assign RoleId from RoleMaster
            TenantId = userRegister.TenantId,
            CreatedOn = DateTime.UtcNow,
            IsActive = true
        };

        _context.UserMasters.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        var user = await _context.UserMasters.SingleOrDefaultAsync(x => x.UserName == userLogin.Username);

        if (user == null)
        {
            return Unauthorized(new { Error = "Invalid username or password." });
        }

        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
        {
            return Unauthorized(new { Error = "Invalid username or password." });
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { Token = tokenHandler.WriteToken(token) });
    }


    [HttpGet("details")]
    [Authorize]
    public async Task<IActionResult> GetUserDetails()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.Name)?.Value;
        if (userIdClaim == null)
        {
            return Unauthorized("User identity not found.");
        }

        var userId = int.Parse(userIdClaim);

        // Include the RoleMaster entity to access RoleName
        var user = await _context.UserMasters
                                 .Include(u => u.Role)
                                 .SingleOrDefaultAsync(u => u.UserId == userId);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.UserId,
            user.UserName,
            RoleName = user.Role?.RoleName, // Access RoleName from the related Role entity
            user.TenantId
        });
    }
}

public class UserRegisterDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RoleName { get; set; }
    public int TenantId { get; set; } // Added TenantId
}

public class UserLoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
