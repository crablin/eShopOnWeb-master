using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microsoft.eShopWeb.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
     // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly ICatalogItemRepository _catalogItemRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(ICatalogItemRepository catalogItemRepository, ICategoryRepository categoryRepository)
        {
            _catalogItemRepository = catalogItemRepository;
            _categoryRepository = categoryRepository;

        }

        [Authorize(Roles = "admin")]
        [HttpGet()]
        public IActionResult GetItems()
        {
            var items = _catalogItemRepository.GetItemsAsync();

            var viewModel = items
                .Select(o => new ProductViewModel()
                {
                    Id = o.Id,
                    Name = o.Name,
                    Price = o.Price.ToString(),
                    PictureUri = o.PictureUri,
                    CategoryTypeId = o.CatalogTypeId,
                    CategoryId = o.CatalogCategoryId,
                    Category = new CategoryViewModel() { Id = o.CatalogCategoryId, Type = _categoryRepository.GetById(o.CatalogCategoryId).Category},
                    CategoryType = new CategoryViewModel() { Id = o.CatalogTypeId, Type = _categoryRepository.GetTypeById(o.CatalogTypeId).Type}
                });
            return View(viewModel);
        }

        [HttpGet("{itemId}")]
        public IActionResult GetById(int itemId)
        {
            var item = _catalogItemRepository.GetByIdWithItemsAsync(itemId).Result;

            var viewModel =  new ProductViewModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price.ToString(),
                    PictureUri = item.PictureUri,
                    CategoryTypeId = item.CatalogTypeId,
                    CategoryId = item.CatalogCategoryId,
                    Description = item.Description,
                    Category = new CategoryViewModel() { Id = item.CatalogCategoryId, Type = _categoryRepository.GetById(item.CatalogCategoryId).Category },
                    CategoryType = new CategoryViewModel() { Id = item.CatalogTypeId, Type = _categoryRepository.GetTypeById(item.CatalogTypeId).Type }
                };
            return View(viewModel);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            IEnumerable<CatalogCategory> categories = _categoryRepository.GetItemsAsync();
            ViewBag.Categories = new SelectList(categories,"Id","Category");
            IList<CatalogType> types = new List<CatalogType>();
            foreach (var items in categories.Select(x => x.CategoryItems))
            {
                foreach (var type in items)
                {
                    types.Add(type);
                } 
            }
            ViewBag.CategoryTypes = new SelectList(types, "Id", "Type");
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost()]
        public IActionResult Create(ProductViewModel model)
        {
            if (model.CategoryId  == 0 || model.CategoryTypeId == 0 || model.Name == null || model.Price == null)
            {
                return RedirectToAction("GetItems");
            }
            _catalogItemRepository.Create(model.CategoryId, model.CategoryTypeId, model.Name, decimal.Parse(model.Price, CultureInfo.InvariantCulture), model.PictureUri, model.Description, model.UnitsInStock);
            return RedirectToAction("GetItems");
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(int productId)
        {
            _catalogItemRepository.Delete(productId);
            return RedirectToAction("GetItems");
        }
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int productId, int categoryTypeId, int categoryId)
        {
            var product = _catalogItemRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel model = new ProductViewModel { 
                Id = product.Id,
                Name = product.Name,
                Price = product.Price.ToString(),
                PictureUri = product.PictureUri,
                CategoryTypeId = categoryTypeId,
                CategoryId = categoryId,
                Description = product.Description,
                UnitsInStock = product.UnitsInStock
            };
            ViewBag.Price = String.Format(product.Price % 1 == 0 ? "{0:0}" : "{0:0.00}", product.Price);
            return View(model);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(ProductViewModel model, string lastPrice)
        {
            if (model.CategoryId == 0 || model.CategoryTypeId == 0 || model.Name == null || model.Price == null)
            {
                return RedirectToAction("GetItems");
            }
            _catalogItemRepository.Edit(model.Id, model.CategoryId, model.CategoryTypeId, model.Name, decimal.Parse(model.Price ?? lastPrice.Replace(",","."), CultureInfo.InvariantCulture), model.PictureUri, model.Description, model.UnitsInStock);
            return RedirectToAction("GetItems");
        }
    }
}
