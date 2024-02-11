using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pipelights.Website.BusinessService;
using Pipelights.Website.BusinessService.Models;
using Pipelights.Website.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Pipelights.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILampService _lampsService;
        private readonly IEmailService _emailService;
        private readonly ICategoryService _categoryService;
        private readonly ITestimonialService _testimonialService;

        public HomeController(ILogger<HomeController> logger, ILampService lampsService, IEmailService emailService, ICategoryService categoryService, ITestimonialService testimonialService)
        {
            _logger = logger;
            _lampsService = lampsService;
            _emailService = emailService;
            _categoryService = categoryService;
            _testimonialService = testimonialService;
        }

        public IActionResult Index()  
        {
            var latestProducts = _lampsService.GetMultiple(false, 4).ToList();

            var testimonials = _testimonialService.GetMultiple("SELECT * FROM c");
            ViewBag.Testimonials = testimonials;

            return View(latestProducts);
        }

        public IActionResult Services()
        {
            var productsDto = _lampsService.GetMultipleServices(false).ToList();

            var servicesList = new List<ProductDetailsDto>();

            foreach(var product in productsDto)
            {
                if (product.IsService)
                {
                    servicesList.Add(product);
                }
            }

            return View(servicesList);
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("emailSended")))
            {
                ViewBag.EmailSent = "Emailul a fost trimis cu succes! Iti vom raspunde in cel mai scurt timp.";
                HttpContext.Session.Remove("emailSended");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public bool sendTheMessage(EmailDto model)
        {
            var emailSent = false;
            if (model != null && model.email != null && model.subject != null && model.message != null)
            {
              emailSent = _emailService.SendEmail("proautomationgroup@gmail.com",
                  model.subject  , $" Primit de la: {model.name}\n" +
                                   $" Email: {model.email}\n" +
                                   $" Mesaj: {model.message}");
            }

            return emailSent;
        }

        [HttpPost]
        public IActionResult SendMessage(EmailDto model)
        {
            var emailSent = sendTheMessage(model);

            if (emailSent)
            {
                HttpContext.Session.SetString("emailSended", "Emailul a fost trimis cu succes! Iti vom raspunde in cel mai scurt timp.");
            }

            return RedirectToAction("Contact", "Home");
        }

        public IActionResult Suggestions()
        {
            string myId = TempData["Id"].ToString();
            List<ProductDetailsDto> list = new List<ProductDetailsDto>();

            var latestProducts = _lampsService.GetSuggestions(false, 100);

            foreach (var product in latestProducts)
            {
                string p = product.Id.ToString();
                if (myId != p)
                {
                    list.Add(product);
                }
            }
            var displayedProducts = list.OrderBy(x => Guid.NewGuid()).Take(4).ToList();

            return PartialView("_SuggestionsPartial", displayedProducts);
        }
        public IActionResult GetServices()
        {
            List<ProductDetailsDto> list = new List<ProductDetailsDto>();

            var services = _lampsService.GetMultipleServices(false, 100);

            foreach(var service in services)
            {
                    list.Add(service);
            }
            
            return PartialView("_ServicesPartial", list);
        }

        public IActionResult PipeLeatherCategoryProducts()
        {
            List<ProductDetailsDto> list = new List<ProductDetailsDto>();

            var latestRobots = _lampsService.GetPipeLeather(false, 100);

            foreach (var leather in latestRobots)
            {
                list.Add(leather);
            }

            var displayedRobots = list.OrderBy(x => Guid.NewGuid()).Take(4).ToList();
            return PartialView("_LeatherCategoryProductsPartial", displayedRobots);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
