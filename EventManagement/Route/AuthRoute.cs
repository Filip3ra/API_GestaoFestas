using Auth.Models;
using Event.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Event.Request;

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

    app.MapDelete("/user/{id:guid}", [Microsoft.AspNetCore.Authorization.Authorize] async (Guid id, EventContext context) =>
    {
      var user = await context.Users.FindAsync(id);

      if (user is null)
      {
        return Results.NotFound("Usuário não encontrado.");
      }

      context.Users.Remove(user);
      await context.SaveChangesAsync();

      return Results.Ok("Usuário removido com sucesso.");
    })
    .WithTags("Login");

    app.MapGet("/List", [Microsoft.AspNetCore.Authorization.Authorize] async (EventContext context) =>
    {
      var users = await context.Users.ToListAsync();
      return Results.Ok(users);
    })
    .WithTags("Login");


  app.MapPatch("{id:guid}", [Microsoft.AspNetCore.Authorization.Authorize] async (Guid id, [FromBody] UserUpdateRequest req, EventContext context) =>
  {
      var user = await context.Users.FindAsync(id);
      if (user == null)
          return Results.NotFound("Usuário não encontrado.");

      // Verifica se o novo nome de usuário está em uso por outro usuário
      if (!string.IsNullOrWhiteSpace(req.Username))
      {
          bool usernameTaken = context.Users.Any(u => u.Username == req.Username && u.Id != id);
          if (usernameTaken)
              return Results.BadRequest("Nome de usuário já está em uso.");

          user.Username = req.Username;
      }

      if (!string.IsNullOrWhiteSpace(req.Password))
      {
          user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
      }

      await context.SaveChangesAsync();
      return Results.Ok("Usuário atualizado com sucesso.");
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

}
