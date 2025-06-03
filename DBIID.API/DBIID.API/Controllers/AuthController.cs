using DBIID.Application.Features.Auth.Login;
using DBIID.Application.Features.Auth.SendOptRequest;
using DBIID.Application.Features.Auth.VerifyOtpRequest;
using DBIID.Shared.Features.Login;
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
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
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

    [HttpPost("otpRequest")]
    public async Task<IActionResult> OtpRequest([FromBody] SendOtpRequest model)
    {
        var result = await mediator.Send(new OtpRequestCommand()
        {
            ContactMethodId = model.ContactMethodId,
            OtpTransactionId = model.OtpTransactionId,
            UserId = model.UserId
        });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Message);
    }

    [HttpPost("verifyotpRequest")]
    public async Task<IActionResult> VerifyotpRequest([FromBody] VerifyOtpRequest model)
    {
        var result = await mediator.Send(new VerifyOtpRequestCommand()
        {
            OtpCode = model.OtpCode,
            OtpTransactionId = model.OtpTransactionId,
            UserId = model.UserId
        });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        DateTime expire = DateTime.Now.AddMinutes(4);
        string token = _jwtService.GenerateToken(result.Value.Id.ToString(), result.Value.Email, "Admin", expire);

        return Ok(new VerifyOtpResponse
        {
            Token = token,
            Expires = expire,
            Message = "Login successful"
        });

        
    }
}

