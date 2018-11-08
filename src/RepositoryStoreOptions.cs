using Microsoft.EntityFrameworkCore;
using System;

namespace ServiceGovernance.Repository.EntityFramework
{
    /// <summary>
    /// Options for configuring the repository stores
    /// </summary>
    public class RepositoryStoreOptions
    {
        /// <summary>
        /// Callback to configure the EF DbContext.
        /// </summary>
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
        /// </summary>        
        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>        
        public string DefaultSchema { get; set; }

        /// <summary>
        /// Gets or sets the api description table configuration.
        /// </summary>        
        public string ApiDescription { get; set; } = "ApiDescriptions";
    }
}
