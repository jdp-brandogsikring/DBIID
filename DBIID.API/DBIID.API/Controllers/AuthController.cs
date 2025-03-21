using DBIID.Application.Features.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;
    private readonly IMediator mediator;

    public AuthController(JwtService jwtService, IMediator mediator)
    {
        _jwtService = jwtService;
        this.mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {

        var result = await mediator.Send(new LoginCommand()
        {
            Email = model.Email,
            Password = model.Password
        });

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Message);
        }

        return Ok(result.Value);

    }
}

        //string token = _jwtService.GenerateToken(result.Value.UserId.ToString(), result.Value.Email, "Admin");
public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}