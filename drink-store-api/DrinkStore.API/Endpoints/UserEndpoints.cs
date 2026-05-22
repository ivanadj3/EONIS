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
            group.MapGet("/users", FetchUsersAsync).RequireAuthorization("AdminOnly");
            group.MapDelete("/users/{id}", DeleteUserByIdAsync).RequireAuthorization("AdminOnly");
            group.MapPost("/change-password", ChangePasswordAsync).RequireAuthorization("AdminOnly");
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
                Phone = dto.Phone,
                PasswordHash = hash
            };

            dbContext.Users.Add(user);

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }

        private static async Task<IResult> FetchUsersAsync(DrinkStoreDbContext dbContext)
        {
            var users = await dbContext.Users.Where(x => x.Role != "Admin").Select(x => new ResUserDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Address = x.Address,
                Phone = x.Phone,
                Role = x.Role,
            }).ToListAsync();

            return Results.Ok(users);
        }

        private static async Task<IResult> DeleteUserByIdAsync(DrinkStoreDbContext dbContext, int id)
        {
            await dbContext.Users.Where(x => x.Id == id).ExecuteDeleteAsync();

            return Results.Ok();
        }

        private static async Task<IResult> ChangePasswordAsync(DrinkStoreDbContext dbContext, ChangePasswordReq dto, IConfiguration config)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == dto.UserId);

            if (user == null)
            {
                return Results.BadRequest(new
                {
                    message = "User not found"
                });
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            user.PasswordHash = hash;

            await dbContext.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
