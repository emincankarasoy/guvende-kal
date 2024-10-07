using GK.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GK.Persistance
{
    public static class ServiceConfigurator
    {
        public static void RegisterPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql());
        }
    }
}
