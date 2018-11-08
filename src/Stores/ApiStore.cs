using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceGovernance.Repository.Models;
using ServiceGovernance.Repository.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of <see cref="IApiStore"/> that uses EntityFramework
    /// </summary>
    public class ApiStore : IApiStore
    {
        private readonly IRepositoryDbContext _context;
        private readonly ILogger<ApiStore> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public ApiStore(IRepositoryDbContext context, ILogger<ApiStore> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public Task<ServiceApiDescription> FindByServiceIdAsync(string serviceId)
        {
            var service = _context.Apis
               .AsNoTracking()
               .FirstOrDefault(s => s.ServiceId == serviceId);
            var model = service?.ToModel();

            _logger.LogDebug("Api for {serviceId} found in database: {serviceIdFound}", serviceId, model != null);

            return Task.FromResult(model);
        }

        public Task<IEnumerable<ServiceApiDescription>> GetAllAsync()
        {
            var services = _context.Apis
              .AsNoTracking();

            IEnumerable<ServiceApiDescription> models = services.ToModelList();

            return Task.FromResult(models);
        }

        public async Task RemoveAsync(string serviceId)
        {
            var existing = _context.Apis.FirstOrDefault(x => x.ServiceId == serviceId);
            if (existing != null)
            {
                _logger.LogDebug("removing api for {serviceId} service from database", serviceId);

                _context.Apis.Remove(existing);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogInformation("exception removing api for {serviceId} from database: {error}", serviceId, ex.Message);
                }
            }
            else
            {
                _logger.LogDebug("no api for {serviceId} found in database", serviceId);
            }
        }

        public async Task StoreAsync(ServiceApiDescription apiDescription)
        {
            var existing = _context.Apis.SingleOrDefault(x => x.ServiceId == apiDescription.ServiceId);
            if (existing == null)
            {
                _logger.LogDebug("Api for {serviceId} not found in database", apiDescription.ServiceId);

                var entity = apiDescription.ToEntity();
                _context.Apis.Add(entity);
            }
            else
            {
                _logger.LogDebug("Api for {serviceId} found in database", apiDescription.ServiceId);

                apiDescription.UpdateEntity(existing);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("exception updating Api for {serviceId} in database: {error}", apiDescription.ServiceId, ex.Message);
            }
        }
    }
}
