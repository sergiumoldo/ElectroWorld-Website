﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pipelights.Database.Models;
using Pipelights.Website.BusinessService;
using Pipelights.Website.BusinessService.Models;
using Pipelights.Website.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;
using Pipelights.Database.Services;
using Newtonsoft.Json;

namespace Pipelights.Website.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILampService _lampService;
        
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IBlobService _blobService;
        private readonly ILogger<AdminController> _logger;
        private readonly IVoucherService _voucherService;
        private readonly ITestimonialService _testimonialService;


        public AdminController(ILampService lampService, ILogger<AdminController> logger, IBlobService blobService,
            ICategoryService categoryService, IOrderService orderService, IVoucherService voucherService, ITestimonialService testimonialService)
        {
          
            _lampService = lampService;
            _logger = logger;
            _blobService = blobService;
            _categoryService = categoryService;
            _orderService = orderService;
            _voucherService = voucherService;
           _testimonialService= testimonialService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string username, string password)
        {
            if (username.Equals("parola") && password.Equals("admin"))
            {
                HttpContext.Session.SetString("username", "parola");
                HttpContext.Session.SetString("password", "admin");

                return RedirectToAction("Dashboard", "Admin");
            }

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Edit(string id)
        {
            IEnumerable<CategoryEntity> categoriesDto = _categoryService.GetMultiple("SELECT * FROM c");
            ViewBag.Categories = categoriesDto;
            if (id == null)
            {
                return View();
            }
            var productDetailsDto = _lampService.GetById(id);
            

            return View(productDetailsDto);
        }

        public IActionResult EditService(string id)
        {
           
            if (id == null)
            {
                return View();
            }
            var productDetailsDto = _lampService.GetById(id);


            return View(productDetailsDto);
        }



        public IActionResult RemovePhoto(string url, string id)
        {
            var x =
            _blobService.RemoveFile(url, "lamps", _logger).Result;

            return RedirectToAction("Edit", "Admin", new { id = id });
        }

        


        [HttpPost]
        public IActionResult Edit(ProductDetailsDto model)
        {
            var isNew = false;
            if (string.IsNullOrEmpty(model.Id))
            {
                isNew = true;
                model.Id = model.Name.Replace(" ", string.Empty);
            }


            var dbModel = new ProductEntity
            {
                id = model.Id,
                Name = model.Name,
                Description = model.Description,
                TechnicalData = model.TechnicalData,
                Price = model.Price,
                PriceReduced = model.PriceReduced,
                IsInactive = model.IsInactive,
                IsOnStock = model.IsOnStock,
                IsService = model.IsService,
                Category = model.Categories,
                Categories = model.CategoriesNew
            };


            if (isNew)
            {
                var result = _lampService.AddAsync(dbModel);
            }
            else
            {
                var result = _lampService.UpdateAsync(dbModel.id, dbModel);
            }

            if(model.IsService == true)
            {
                return RedirectToAction("EditService", "Admin", new { id = model.Id });
            }
            return RedirectToAction("Edit", "Admin", new { id = model.Id });
        }

       

        public IActionResult EditCategory(string id)
        {
            if (id == null)
            {
                return View(null);
            }
            var categoryDetailsDto = _categoryService.GetById(id);

            return View(categoryDetailsDto);

        }


        [HttpPost]
        public IActionResult EditCategory(CategoryDetailsDto model)
        {
            var categoryEntity = new CategoryEntity
            {
                id = model.id,
                Name = model.Name,
            };
            _categoryService.UpdateAsync(model.id, categoryEntity);
            return RedirectToAction("CategoriesDashboard", "Admin");
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryDetailsDto model)
        {
            var categoryEntity = new CategoryEntity
            {
                id = model.id,
                Name = model.Name,
            };
            _categoryService.AddAsync(categoryEntity);
            return RedirectToAction("CategoriesDashboard", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteCategory(string id)
        {
            _categoryService.DeleteAsync(id);
            
            return RedirectToAction("CategoriesDashboard", "Admin");
        }

        public IActionResult InsertVoucher()
        {
            return View();
        }

        public IActionResult EditViewForVoucher(string id)
        {
            if (id == null)
            {
                return View(null);
            }
            var voucherDetailsDto = _voucherService.GetById(id);

            return View(voucherDetailsDto);

        }


        [HttpPost]
        public IActionResult EditVoucher(VoucherDetailsDto model)
        {
            var voucherEntity = new VoucherEntity
            {
                id = model.id,
                Name = model.Name,
                Discount = model.Discount,
                Percentage= model.Percentage,
                CreationDate = model.CreationDate,
                ExpiringDate = model.ExpiringDate,
                isActive = model.isActive,
                isUsed = model.isUsed,
                isSingleUse = model.isSingleUse
            };
            _voucherService.UpdateAsync(model.id, voucherEntity);
            return RedirectToAction("VouchersDashboard", "Admin");
        }

        [HttpPost]
        public IActionResult AddVoucher(VoucherDetailsDto model)
        {
            var voucherEntity = new VoucherEntity
            {
                id = model.id,
                Name = model.Name,
                Discount= model.Discount,
                Percentage= model.Percentage,
                CreationDate= model.CreationDate,
                ExpiringDate= model.ExpiringDate,
                isActive=model.isActive,
                isUsed = model.isUsed,
                isSingleUse = model.isSingleUse
            };
            _voucherService.AddAsync(voucherEntity);
            return RedirectToAction("VouchersDashboard", "Admin");
        }

        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("username");
            var password = HttpContext.Session.GetString("password");

            if(username == null || password == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            if (username.Equals("parola") && password.Equals("admin"))
            {
                return View();
            }

            return RedirectToAction("Index", "Admin");
            
        }

        public IActionResult ProductsDashboard()
        {
            IEnumerable<ProductDetailsDto> productsDto = _lampService.GetMultiple("SELECT * FROM c", true).OrderBy(l => l.IsInactive);

            var productList = new List<ProductDetailsDto>();
            foreach(var product in productsDto)
            {
                if (!product.IsService)
                {
                    productList.Add(product);
                }
            }

            return View(productList);
        }

        public IActionResult ServiceDashboard()
        {
            IEnumerable<ProductDetailsDto> productsDto = _lampService.GetMultiple("SELECT * FROM c", true).OrderBy(l => l.IsInactive);

            var serviceList = new List<ProductDetailsDto>();
            foreach (var product in productsDto)
            {
                if (product.IsService)
                {
                    serviceList.Add(product);
                }
            }

            return View(serviceList);
        }

       

        public IActionResult CategoriesDashboard()
        {
            IEnumerable<CategoryEntity> categoryDto = _categoryService.GetMultiple("SELECT * FROM c");

            return View(categoryDto);
        }

        public IActionResult TestimonialDashboard()
        {
            IEnumerable<TestimonialEntity> categoryDto = _testimonialService.GetMultiple("SELECT * FROM c");

            return View(categoryDto);
        }

        public IActionResult DeleteTestimonial(string id)
        {
            _testimonialService.DeleteAsync(id);

            return RedirectToAction("TestimonialDashboard", "Admin");
        }


        public IActionResult VouchersDashboard()
        {
            IEnumerable<VoucherEntity> voucherDto = _voucherService.GetMultiple("SELECT * FROM c");

            return View(voucherDto);
        }

        public IActionResult OrdersDashboard()
        {
            IEnumerable<OrderEntity> ordersDto = _orderService.GetMultiple("SELECT * FROM c order by c._ts desc");

            return View(ordersDto);
        }

        public IActionResult OrdersDashboardDetails(string id)
        {
            OrderEntity orderDto = _orderService.GetById(id);

            return View("OrdersDashboardDetails", orderDto);
        }

       
        public IActionResult DeleteOrderManually(string id)
        {
            OrderEntity orderById = _orderService.GetById(id);
            var orderId = orderById.id;

            _orderService.DeleteOrder(id);

            return RedirectToAction("OrdersDashboard", "Admin");
        }


        public IActionResult EditOrder(string id)
        {
            OrderEntity order = _orderService.GetById(id);

            return View(order);
        }

        [HttpPost]
        public IActionResult EditOrder(OrderEntity model)
        {
            _orderService.EditOrder(model);

            return RedirectToAction("OrdersDashboard", "Admin");
        }

        public IActionResult Insert()
        {
           var allProducts  =  _lampService.GetMultiple("SELECT * FROM c", false);

            List<string> idList = new List<string>();

            foreach(var product in allProducts)
            {
                idList.Add(product.Id);
            }
            ViewBag.idList = idList;

            return View();
        }

        [HttpPost]
        public IActionResult Insert(OrderEntity model, string CartProductsJson)
        {
            // Deserialize the JSON string back into a list of CartProductsEntity
            List<CartProductsEntity> cartProducts = JsonConvert.DeserializeObject<List<CartProductsEntity>>(CartProductsJson);

            // Assign the cartProducts list to the model's CartProducts property
            model.CartProducts = cartProducts;

            var productsSum = 0;

            foreach(var product in cartProducts)
            {
                int.TryParse(product.Price, out int intPrice);
                productsSum += intPrice * product.Quantity;
            }

            model.Subtotal = productsSum;

            _orderService.InsertOrder(model);

            return RedirectToAction("OrdersDashboard", "Admin");
        }

        [HttpPost]
        public IActionResult UploadImage(List<IFormFile> postedFiles, string id)
        {

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                var stream = postedFile.OpenReadStream();
                var result = _blobService.SaveFileToBlobStorage(stream, $"{id.ToLower()}/{postedFile.FileName}", "lamps", _logger).Result;
            }

            return RedirectToAction("Edit", "Admin", new { id = id });
        }

        


    }
}
