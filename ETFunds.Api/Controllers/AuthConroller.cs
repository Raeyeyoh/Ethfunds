using System.Security.Claims;
using ETFunds.Application.Dtos;
using ETFunds.Application.User.Commands;
using ETFunds.Application.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETFunds.Api.Controllers;

[ApiController]
[Route("api/Users")]
public class AuthController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
      [FromBody] CreateUserRequest request,
      CancellationToken ct)
    {
        var result = await mediator.Send(
            new RegisterUserCommand(request),
            ct);

        if (!result)
        {
            return BadRequest(new
            {
                detail = "Registration failed."
            });
        }

        return Ok(new
        {
            message = "Registration successful."
        });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserRequest request,
        [FromServices] IWebHostEnvironment env,
         [FromServices] IAntiforgery antiforgery,
        CancellationToken ct)
    {
        var result = await mediator.Send(
            new LoginUserCommand(request),
            ct);

        if (!result.Success)
        {
            return Unauthorized(new
            {
                detail = "Invalid email or password."
            });
        }

        Response.Cookies.Append(
            "access_token",
            result.AccessToken!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !env.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

        Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !env.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);

        Response.Cookies.Append(
            "XSRF-TOKEN",
            tokens.RequestToken!,
            new CookieOptions
            {
                HttpOnly = false,
                Secure = !env.IsDevelopment(),
                SameSite = SameSiteMode.Lax
            });
        return Ok(new
        {
            message = "Login successful."
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(
        CancellationToken ct)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null ||
            !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var user = await mediator.Send(
            new GetCurrentUserQuery(userId),
            ct);

        if (user == null)
            return Unauthorized();

        return Ok(user);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    [FromServices] IWebHostEnvironment env,
    CancellationToken ct)
    {
        var refreshToken =
            Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new
            {
                detail = "Refresh token missing."
            });
        }

        var result = await mediator.Send(
            new RefreshTokenCommand(refreshToken),
            ct);

        if (result == null || !result.Success)
        {
            return Unauthorized(new
            {
                detail = "Invalid or expired refresh token."
            });
        }

        Response.Cookies.Append(
            "access_token",
            result.AccessToken!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !env.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

        Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken!,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !env.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

        return Ok(new
        {
            message = "Token refreshed successfully."
        });
    }
}