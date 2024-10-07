using GK.Application.Models.Configuration;
using Microsoft.Extensions.Configuration;

namespace GK.Application.Engines
{
    public static class ConfigurationLoader
    {
        public static void LoadApplicationConfigurations(this ConfigurationManager manager)
        {
            DatabaseConfiguration.DBConnectionString = manager.GetConnectionString("PostgreSQL") ?? throw new ArgumentNullException("DB Connection String Bulunamadı! : [PostgreSQL]");
        }
    }
}
