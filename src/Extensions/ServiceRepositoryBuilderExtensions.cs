using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Repository.Configuration;
using ServiceGovernance.Repository.EntityFramework;
using ServiceGovernance.Repository.EntityFramework.Stores;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EntityFramework database persistence to service repository
    /// </summary>
    public static class ServiceRepositoryBuilderExtensions
    {
        /// <summary>
        /// Configures EntityFramework implementation of IApiStore
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddRepositoryStore(this IServiceRepositoryBuilder builder, Action<RepositoryStoreOptions> storeOptionsAction = null)
        {
            return builder.AddRepositoryStore<RepositoryDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Configures EntityFramework implementation of IApiStore.
        /// </summary>
        /// <typeparam name="TContext">The IConfigurationDbContext to use.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceRepositoryBuilder AddRepositoryStore<TContext>(this IServiceRepositoryBuilder builder, Action<RepositoryStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IRepositoryDbContext
        {
            builder.Services.AddRepositoryDbContext<TContext>(storeOptionsAction);

            builder.AddApiStore<ApiStore>();

            return builder;
        }

        /// <summary>
        /// Add Repository DbContext to the DI system.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryDbContext(this IServiceCollection services, Action<RepositoryStoreOptions> storeOptionsAction = null)
        {
            return services.AddRepositoryDbContext<RepositoryDbContext>(storeOptionsAction);
        }

        /// <summary>
        /// Add Repository DbContext to the DI system.
        /// </summary>        
        /// <typeparam name="TContext">The IRegistryDbContext to use.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="storeOptionsAction">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryDbContext<TContext>(this IServiceCollection services, Action<RepositoryStoreOptions> storeOptionsAction = null)
            where TContext : DbContext, IRepositoryDbContext
        {
            var options = new RepositoryStoreOptions();
            services.AddSingleton(options);
            storeOptionsAction?.Invoke(options);

            if (options.ResolveDbContextOptions != null)
            {
                services.AddDbContext<TContext>(options.ResolveDbContextOptions);
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder => options.ConfigureDbContext?.Invoke(dbCtxBuilder));
            }

            services.AddScoped<IRepositoryDbContext, TContext>();

            return services;
        }
    }
}
