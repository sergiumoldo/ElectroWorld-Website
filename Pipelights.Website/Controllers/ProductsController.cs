﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pipelights.Database.Models;
using Pipelights.Website.BusinessService;
using Pipelights.Website.BusinessService.Models;
using Pipelights.Website.Models;
using Pipelights.Website.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Pipelights.Website.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IBlobService _blobService;
        private readonly ILampService _lampService;
        private readonly ICategoryService _categoryService;
        public ProductsController(ILogger<ProductsController> logger, ILampService lampService, ICategoryService categoryService, IBlobService blobService)
        {
            _logger = logger;
            _lampService = lampService;
            _categoryService = categoryService;
            _blobService = blobService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetMultiple("SELECT * FROM c");
            ViewBag.Categories = categories;

            var latestProducts = _lampService.GetMultiple(false);

            var onlyProducts = new List<ProductDetailsDto>();

            foreach (var product in latestProducts)
            {
                if (!product.IsService)
                {
                    onlyProducts.Add(product);
                }
            }
            return View(onlyProducts);
        }
        public IActionResult Filter(string Id)
        {
            var categories = _categoryService.GetMultiple("SELECT * FROM c");
            ViewBag.Categories = categories;

            var products = _lampService.GetMultiple($"SELECT * FROM c WHERE Array_Contains(c.Categories, '{Id}')", false);

            return View(products);
        }


        [HttpPost]
        public IActionResult Search(SearchDto model)
        {
            var searchString = model.searchValue;

            if (searchString == null || searchString.Length <= 2)
            {
                return RedirectToAction("Index", "Products");
            }

            var query = "SELECT * FROM c WHERE LOWER(c.Name) LIKE '%" + searchString.ToLower() + "%'";

            var productsDtoSearched = _lampService.GetMultiple(query, false);

            if (productsDtoSearched == null || productsDtoSearched.Count() == 0)
            {
                ViewBag.Search = "Nu s-a gasit niciun rezultat";
            }
            else
            {
                ViewBag.Search = "Rezultatele cautarii";
            }

            return View(productsDtoSearched);
        }

        public IActionResult DetailsForService(string id)
        {
            var service = _lampService.GetById(id);

            return View(service);
        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Products");
            }

            var lamp = _lampService.GetById(id);


            if (lamp.IsService)
            {
                HttpContext.Session.SetString("Servicii", lamp.Id);
                return RedirectToAction("DetailsForService", "Products");
            }


            TempData["Id"] = lamp.Id;

            var cat = lamp.CategoriesNew;
            var isOnStock = lamp.IsOnStock;

            if (cat != null && cat.Contains("becuri"))
            {
                ViewBag.Becuri = "becuri";
            }

            if (isOnStock)
            {
                ViewBag.IsOnStock = "true";
            }

            if (lamp == null)
            {
                return RedirectToAction("Index", "Products");
            }
            return View(lamp);
        }

        public IActionResult Sort(string id)
        {
            var value = HttpContext.Session.GetString("sortId");

            if (value == "oameni")
            {
                if (id == "price")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'oameni') ORDER BY c.Price ASC", false);
                    return View(orderedBy);
                }
                else if (id == "priceDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'oameni') ORDER BY c.Price DESC", false);
                    return View(orderedBy);
                }
                else if (id == "name")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'oameni') ORDER BY c.Name ASC", false);
                    return View(orderedBy);
                }
                else if (id == "nameDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'oameni') ORDER BY c.Name Desc", false);
                    return View(orderedBy);
                }
            }
            else if (value == "leather")
            {
                if (id == "price")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'leather') ORDER BY c.Price ASC", false);
                    return View(orderedBy);
                }
                else if (id == "priceDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'leather') ORDER BY c.Price DESC", false);
                    return View(orderedBy);
                }
                else if (id == "name")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'leather') ORDER BY c.Name ASC", false);
                    return View(orderedBy);
                }
                else if (id == "nameDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'leather') ORDER BY c.Name Desc", false);
                    return View(orderedBy);
                }
            }
            else if (value == "roboti")
            {
                if (id == "price")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'roboti') ORDER BY c.Price ASC", false);
                    return View(orderedBy);
                }
                else if (id == "priceDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'roboti') ORDER BY c.Price DESC", false);
                    return View(orderedBy);
                }
                else if (id == "name")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'roboti') ORDER BY c.Name ASC", false);
                    return View(orderedBy);
                }
                else if (id == "nameDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'roboti') ORDER BY c.Name Desc", false);
                    return View(orderedBy);
                }
            }
            else if (value == "lampi")
            {
                if (id == "price")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'lampi') ORDER BY c.Price ASC", false);
                    return View(orderedBy);
                }
                else if (id == "priceDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'lampi') ORDER BY c.Price DESC", false);
                    return View(orderedBy);
                }
                else if (id == "name")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'lampi') ORDER BY c.Name ASC", false);
                    return View(orderedBy);
                }
                else if (id == "nameDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c Where Array_Contains(c.Categories, 'lampi') ORDER BY c.Name Desc", false);
                    return View(orderedBy);
                }
            }
            else if (value == "toateprodusele")
            {
                if (id == "price")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c ORDER BY c.Price ASC", false);
                    return View(orderedBy);
                }
                else if (id == "priceDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c ORDER BY c.Price DESC", false); ;
                    return View(orderedBy);
                }
                else if (id == "name")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c ORDER BY c.Name ASC", false);
                    return View(orderedBy);
                }
                else if (id == "nameDesc")
                {
                    var orderedBy = _lampService.GetMultiple("SELECT * FROM c ORDER BY c.Name DESC", false);
                    return View(orderedBy);
                }
            }

            return RedirectToAction("Index", "Products");
        }

        public IActionResult FilterByCategory(string id)
        {
            CountFromCategories();


            if (id == "music")
            {
                var result = _lampService.GetMusic(false, 100);
                ViewBag.FilterCategory = "Muzicieni";


                return View(result);
            }
            else if (id == "food")
            {
                var result = _lampService.GetFood(false, 100);
                ViewBag.FilterCategory = "Înfometaţi";


                return View(result);
            }
            else if (id == "drink")
            {
                var result = _lampService.GetDrink(false, 100);
                ViewBag.FilterCategory = "Îsetaţi";


                return View(result);
            }
            else if (id == "travel")
            {
                var result = _lampService.GetTravel(false, 100);
                ViewBag.FilterCategory = "Calatori";


                return View(result);
            }
            else if (id == "hobby")
            {
                var result = _lampService.GetHobby(false, 100);
                ViewBag.FilterCategory = "Hobby";


                return View(result);
            }
            else if (id == "job")
            {
                var result = _lampService.GetJob(false, 100);
                ViewBag.FilterCategory = "Meseriasi";


                return View(result);
            }
            else if (id == "business")
            {
                var result = _lampService.GetBusiness(false, 100);
                ViewBag.FilterCategory = "Business";


                return View(result);
            }
            else if (id == "leather")
            {
                var result = _lampService.GetPipeLeather(false, 100);
                ViewBag.FilterCategory = "Pipe & Leather";


                return View(result);
            }
            else if (id == "twoBulbs")
            {
                var result = _lampService.GetTwoBulbs(false, 100);
                ViewBag.FilterCategory = "Doua becuri";


                return View(result);
            }
            else if (id == "other")
            {
                var result = _lampService.GetOther(false, 100);
                ViewBag.FilterCategory = "Altele";


                return View(result);
            }

            return RedirectToAction("Index?Id=oameni", "Products");
        }

        public void CountFromCategories()
        {


            var countMusic = _lampService.GetMusic(false, 100).Count();
            ViewBag.countMusic = countMusic;

            var countFood = _lampService.GetFood(false, 100).Count();
            ViewBag.countFood = countFood;

            var countDrink = _lampService.GetDrink(false, 100).Count();
            ViewBag.countDrink = countDrink;

            var countTravel = _lampService.GetTravel(false, 100).Count();
            ViewBag.countTravel = countTravel;

            var countHobby = _lampService.GetHobby(false, 100).Count();
            ViewBag.countHobby = countHobby;

            var countJob = _lampService.GetJob(false, 100).Count();
            ViewBag.countJob = countJob;

            var countBusiness = _lampService.GetBusiness(false, 100).Count();
            ViewBag.countBusiness = countBusiness;

            var countLeather = _lampService.GetPipeLeather(false, 100).Count();
            ViewBag.countLeather = countLeather;

            var countTwoBulbs = _lampService.GetTwoBulbs(false, 100).Count();
            ViewBag.countTwoBulbs = countTwoBulbs;

            var countOther = _lampService.GetOther(false, 100).Count();
            ViewBag.countOther = countOther;
        }

        public IActionResult OpenFile(string url, string id)
        {
            var fileStream = _blobService.OpenFile( "lamps", url, _logger).Result;

            if (fileStream != null)
            {
                return File(fileStream, "application/pdf", $"{id}_FisaTehnica.pdf");
            }

            // Handle the case where the file stream is null or there's an error
            return RedirectToAction("Edit", "Admin", new { id = id });
        }


    }
}
