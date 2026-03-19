using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace DrinkApi.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (roleResult.Succeeded)
                {
                    logger.LogInformation("Created role {Role}", role);
                }
                else
                {
                    logger.LogWarning("Failed to create role {Role}: {Errors}", role, string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        var adminEmail = configuration["DefaultAdmin:Email"];
        var adminPassword = configuration["DefaultAdmin:Password"];

        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            logger.LogWarning("Default admin seeding skipped because DefaultAdmin credentials are not configured.");
        }
        else
        {
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Created default admin user {Email}", adminEmail);
                }
                else
                {
                    logger.LogWarning("Failed to create default admin user {Email}: {Errors}", adminEmail, string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Default admin user already exists: {Email}", adminEmail);
            }
        }

        var userEmail = configuration["DefaultUser:Email"];
        var userPassword = configuration["DefaultUser:Password"];

        if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPassword))
        {
            logger.LogInformation("Default user seeding skipped because DefaultUser credentials are not configured.");
            return;
        }

        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                    logger.LogInformation("Created default user {Email}", userEmail);
                }
                else
                {
                    logger.LogWarning("Failed to create default user {Email}: {Errors}", userEmail, string.Join("; ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Default user already exists: {Email}", userEmail);
            }
        }
    }
}
