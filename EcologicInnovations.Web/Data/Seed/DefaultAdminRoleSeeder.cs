using EcologicInnovations.Web.Configuration;
using EcologicInnovations.Web.Security;
using Microsoft.AspNetCore.Identity;

namespace EcologicInnovations.Web.Data.Seed;

/// <summary>
/// Seeds the Admin role and optionally creates a default admin user.
/// The logic is idempotent and safe to run multiple times.
/// </summary>
public static class DefaultAdminRoleSeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager,
        SeedOptions seedOptions,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Ensure Admin role exists.
        var adminRoleExists = await roleManager.RoleExistsAsync(AppRoles.Admin);
        if (!adminRoleExists)
        {
            var createRoleResult = await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
            if (!createRoleResult.Succeeded)
            {
                var errors = string.Join("; ", createRoleResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to create Admin role: {errors}");
            }
        }

        // Optional default admin user creation.
        if (string.IsNullOrWhiteSpace(seedOptions.DefaultAdminEmail) ||
            string.IsNullOrWhiteSpace(seedOptions.DefaultAdminPassword))
        {
            return;
        }

        var normalizedEmail = seedOptions.DefaultAdminEmail.Trim();
        var existingUser = await userManager.FindByEmailAsync(normalizedEmail);

        if (existingUser is null)
        {
            var adminUser = new IdentityUser
            {
                UserName = normalizedEmail,
                Email = normalizedEmail,
                EmailConfirmed = true
            };

            var createUserResult = await userManager.CreateAsync(adminUser, seedOptions.DefaultAdminPassword);
            if (!createUserResult.Succeeded)
            {
                var errors = string.Join("; ", createUserResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to create default admin user: {errors}");
            }

            existingUser = adminUser;
        }

        if (!await userManager.IsInRoleAsync(existingUser, AppRoles.Admin))
        {
            var addRoleResult = await userManager.AddToRoleAsync(existingUser, AppRoles.Admin);
            if (!addRoleResult.Succeeded)
            {
                var errors = string.Join("; ", addRoleResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to assign Admin role to default admin user: {errors}");
            }
        }
    }
}
