using Microsoft.EntityFrameworkCore;
using ServiceGovernance.Repository.EntityFramework.Entities;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.EntityFramework
{
    /// <summary>
    /// Abstraction for the repository db context
    /// </summary>
    public interface IRepositoryDbContext
    {
        /// <summary>
        /// Gets or sets the api descriptions.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        DbSet<ApiDescription> Apis { get; set; }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
