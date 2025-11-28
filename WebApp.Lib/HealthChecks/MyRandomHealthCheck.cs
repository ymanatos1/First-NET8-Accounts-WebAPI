using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApp.Lib.HealthChecks
{
    public class MyRandomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return new Random().Next() % 2 == 0
                ? Task.FromResult(HealthCheckResult.Healthy("Good"))
                : Task.FromResult(HealthCheckResult.Degraded("Not so good"));
        }
    }
}
