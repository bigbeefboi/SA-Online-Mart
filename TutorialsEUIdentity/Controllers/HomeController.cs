using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TutorialsEUIdentity.Data;
using TutorialsEUIdentity.Models;
using TutorialsEUIdentity.Services;

namespace TutorialsEUIdentity.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IRoleManagerService _roleManagerService;
        private readonly IUserManagerService _userManagerService;

        private readonly ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IRoleManagerService roleManagerService, IUserManagerService userManagerService)
        {
            _logger = logger;
            _roleManagerService = roleManagerService;
            _userManagerService = userManagerService;
            _context = context;
        }
        

        //making use of the dependency injection
        /*
        private readonly IEmailSenderService _service;

        public HomeController(IEmailSenderService service)
        {
            _service = service;
        }
        */

        //redirects to a page based on role or login status


        public async Task<IActionResult> Index()
        {
            //_service.SendEmail("Hello World");
            await _roleManagerService.ensureManagerRoles();
            await _userManagerService.ensureUserRoles();

            /*
            trying to route to different pages based on login status and role status 

            if(User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Create", "Product");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
             */

            return View(await _context.Product.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

       
        public ActionResult ProductDetails(int id)
        {
            var productModel = _context.Product.Find(id);
            return View("Details",productModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
