using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DrinkStore.API.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/auth");

            group.MapPost("/login", LoginAsync);
        }

        private static async Task<IResult> LoginAsync(DrinkStoreDbContext dbContext, ReqLoginDto dto, IConfiguration config)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                return Results.BadRequest(new
                {
                    message = "Invalid credentials"
                });
            }

            var valid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash
            );

            if (!valid)
            {
                return Results.BadRequest(new
                {
                    message = "Invalid credentials"
                });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwt = config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            var jwtToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Results.Ok(new
            {
                token = jwtToken
            });
        }
    }
}

