using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GK.Persistance.Context
{
    internal class DatabaseContext : DbContext
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
