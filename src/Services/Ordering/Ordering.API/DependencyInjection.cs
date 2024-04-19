namespace Ordering.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceProvider UseApiServices(this IServiceProvider services)
        {
            return services;
        }
    }
}
