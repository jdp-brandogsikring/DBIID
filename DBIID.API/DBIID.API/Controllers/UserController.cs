using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

//[Authorize]
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        // ✅ Log the Authorization Header
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            Console.WriteLine("❌ No Authorization header found.");
            return Unauthorized("Authorization header is missing.");
        }

        string authorizationHeader = Request.Headers["Authorization"];
        Console.WriteLine($"🔍 Received Authorization Header: {authorizationHeader}");

        // ✅ Extract the token
        string token = authorizationHeader.StartsWith("Bearer ") ? authorizationHeader.Substring(7) : authorizationHeader;
        Console.WriteLine($"🔍 Extracted Token: {token}");

        if (!User.Identity.IsAuthenticated)
        {
            Console.WriteLine("❌ User is NOT authenticated!");
            return Unauthorized("User authentication failed.");
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine($"✅ User authenticated! ID: {userId}");

        return Ok(new { Message = "User authenticated!", UserId = userId });
    }
}