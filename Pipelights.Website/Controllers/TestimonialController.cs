using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pipelights.Database.Models;
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
    public class TestimonialController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILampService _lampsService;
        private readonly IEmailService _emailService;
        private readonly ICategoryService _categoryService;
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ILogger<HomeController> logger, ILampService lampsService, IEmailService emailService, ICategoryService categoryService, ITestimonialService testimonialService)
        {
            _logger = logger;
            _lampsService = lampsService;
            _emailService = emailService;
            _categoryService = categoryService;
            _testimonialService = testimonialService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Insert(TestimonialDto model)
        {
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }

            string formattedDate = DateTime.Now.ToString("yyMMdd-HHmmss");

            model.id = formattedDate;

            var testimonial = new TestimonialEntity()
            {
                Description = model.Description,
                Name = model.Name,
                IsInactive = model.IsInactive,
                id = model.id,
                Stars = model.Stars
            };

            _testimonialService.AddAsync(testimonial);

            return RedirectToAction("Index", "Home");
        }

    }
}
