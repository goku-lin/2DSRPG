using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeProject.Data;
using ResumeProject.DTOs;
using ResumeProject.Services;

namespace ResumeProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext db, JwtTokenService jwtTokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == request.Password);
        if (user is null)
        {
            return Unauthorized("用户名或密码错误");
        }

        var token = jwtTokenService.CreateToken(user);
        return Ok(new LoginResponse(token, user.UserName, user.Role));
    }
}
