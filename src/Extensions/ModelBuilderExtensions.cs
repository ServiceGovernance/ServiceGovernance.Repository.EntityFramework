using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Repository.EntityFramework.Entities;

namespace ServiceGovernance.Repository.EntityFramework
{
    /// <summary>
    /// Extension methods to define the database schema for the registry stores
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures the service registry model
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureServiceRepository(this ModelBuilder modelBuilder, RepositoryStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
                modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<ApiDescription>(service =>
            {
                service.ToTable(storeOptions.ApiDescription);
                service.HasKey(x => x.Id);

                service.Property(x => x.ServiceId).HasMaxLength(200).IsRequired();
                service.Property(x => x.ApiDocument).HasMaxLength(int.MaxValue);

                service.HasIndex(x => x.ServiceId).IsUnique();
            });
        }
    }
}
