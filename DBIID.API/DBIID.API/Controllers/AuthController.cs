using Microsoft.AspNetCore.Mvc;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Email == "admin@example.com" && model.Password == "password")
        {
            string token = _jwtService.GenerateToken("1", model.Email, "Admin");

            Console.WriteLine($"✅ Generated JWT Token: {token}"); // ✅ Log token
            return Ok(new { Token = token });
        }
        return Unauthorized("Invalid credentials.");
    }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}