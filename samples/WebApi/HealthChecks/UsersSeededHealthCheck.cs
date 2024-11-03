using System.Linq;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using WebApi.Domain;

namespace WebApi;

public class UsersSeededHealthCheck : IHealthCheck
{
  private readonly IServiceProvider _serviceProvider;

  public UsersSeededHealthCheck(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public async Task<HealthCheckResult> CheckHealthAsync(
    HealthCheckContext context,
    CancellationToken cancellationToken = default
  )
  {
    var seededUsersToExpect = new string[] {
      Users.AdminUser,
      Users.AliceUser,
      Users.BobUser,
      // "MyDegradedUser",
      // "MyUnhealthyUser"
    };

    var manager = _serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var expectedUser in seededUsersToExpect)
    {
      if (expectedUser == "MyDegradedUser")
      {
        return HealthCheckResult.Degraded($"Degraded user '{expectedUser}' detected.");
      }
      else
      {
        var user = await manager.FindByNameAsync(expectedUser);
        if (user is null)
        {
          return new HealthCheckResult(
              context.Registration.FailureStatus,
              $"User '{expectedUser}' could not been retrieved."
            );
        }
      }
    }

    return HealthCheckResult.Healthy("All seeded users could be retrieved.");
  }
}