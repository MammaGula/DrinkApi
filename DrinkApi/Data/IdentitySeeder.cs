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
                // If the admin already exists ensure the seeded password matches and the Admin role is assigned
                var isValid = await userManager.CheckPasswordAsync(admin, adminPassword);
                if (!isValid)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                    var resetResult = await userManager.ResetPasswordAsync(admin, token, adminPassword);
                    if (resetResult.Succeeded)
                    {
                        logger.LogInformation("Reset password for default admin {Email}", adminEmail);
                    }
                    else
                    {
                        logger.LogWarning("Failed to reset password for default admin {Email}: {Errors}", adminEmail, string.Join("; ", resetResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("Default admin user already exists: {Email}", adminEmail);
                }
                // Ensure admin role assigned
                if (!await userManager.IsInRoleAsync(admin, "Admin"))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
                    if (addRoleResult.Succeeded)
                        logger.LogInformation("Added Admin role to existing user {Email}", adminEmail);
                    else
                        logger.LogWarning("Failed to add Admin role to {Email}: {Errors}", adminEmail, string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));
                }
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
                // Ensure default user's password matches the seeded password during development
                var isValidUser = await userManager.CheckPasswordAsync(user, userPassword);
                if (!isValidUser)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await userManager.ResetPasswordAsync(user, token, userPassword);
                    if (resetResult.Succeeded)
                    {
                        logger.LogInformation("Reset password for default user {Email}", userEmail);
                    }
                    else
                    {
                        logger.LogWarning("Failed to reset password for default user {Email}: {Errors}", userEmail, string.Join("; ", resetResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogInformation("Default user already exists: {Email}", userEmail);
                }
                // Ensure user role assigned
                if (!await userManager.IsInRoleAsync(user, "User"))
                {
                    var addRoleResult = await userManager.AddToRoleAsync(user, "User");
                    if (addRoleResult.Succeeded)
                        logger.LogInformation("Added User role to existing user {Email}", userEmail);
                    else
                        logger.LogWarning("Failed to add User role to {Email}: {Errors}", userEmail, string.Join("; ", addRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
