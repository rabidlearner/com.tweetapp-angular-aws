
namespace com.tweetapp.api.Helpers
{
    public static class ConfigureApiVersioning
    {
        public static void AddApiVersioningConfigured(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });
        }
    }
}
