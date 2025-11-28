using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApp.Lib.HealthChecks
{
    public class ApiAliveHealthCheck : IHealthCheck
    {
        public string? ApiUri;
        public string? ApiPath = "/api/alive";

        public ApiAliveHealthCheck(IConfiguration configuration)
        {
            ApiUri = configuration["Api:Uri"];
            ApiPath = configuration["Api:Path"];
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var client = new HttpClient();

            try
            {
                using HttpResponseMessage response = await client.GetAsync($"{ApiUri}{ApiPath}");
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                if (body == "true")
                {
                    return HealthCheckResult.Healthy("Api available.");
                }
                else
                {
                    return HealthCheckResult.Degraded("Api in unknown state.");
                }
            }
            catch //(Exception ex)
            {
                return HealthCheckResult.Unhealthy("Api not available.");
            }

        }

    }
}
