using Auth.Models;
using Event.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Routes;

public static class AuthRoute
{
    public static void AuthEndpoints(this WebApplication app)
    {
        app.MapPost("/login", ([FromBody] LoginRequest request, EventContext context, IConfiguration config) =>
        {
            var user = context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.Unauthorized();
            }

            var token = GenerateJwtToken(user, config);
            return Results.Ok(new { token });
        })
      .WithTags("Login");

        app.MapPost("/register", ([FromBody] LoginRequest request, EventContext context) =>
        {
            if (context.Users.Any(u => u.Username == request.Username))
                return Results.BadRequest("Usuário já existe");

            var user = new UserModel
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            context.Users.Add(user);
            context.SaveChanges();

            return Results.Created($"/users/{user.Id}", user);
        })
      .WithTags("Login");
    }

    private static string GenerateJwtToken(UserModel user, IConfiguration configuration)
    {
        var secret = configuration["JwtSettings:SecretKey"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)); // guarde no appsettings
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public record LoginRequest(string Username, string Password);
}
