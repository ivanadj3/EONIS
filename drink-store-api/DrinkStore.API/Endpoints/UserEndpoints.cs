using DrinkStore.API.Db;
using DrinkStore.API.Dto;
using DrinkStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkStore.API.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api");

            group.MapPost("/sign-up", SignUpAsync);
        }

        private static async Task<IResult> SignUpAsync(DrinkStoreDbContext dbContext, ReqSignupDto dto, IConfiguration config)
        {
            var exists = await dbContext.Users.AnyAsync(x => x.Email == dto.Email);

            if (exists)
            {
                return Results.BadRequest(new
                {
                    message = "Email already exists"
                });
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Address = dto.Address,
                Email = dto.Email,
                PasswordHash = hash
            };

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
