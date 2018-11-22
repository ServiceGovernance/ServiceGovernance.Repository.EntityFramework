using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using ServiceGovernance.Repository.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Repository.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public HomeController(IServiceRepository serviceRegistry)
        {
            _serviceRepository = serviceRegistry ?? throw new ArgumentNullException(nameof(serviceRegistry));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _serviceRepository.GetAllApisAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
