using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = "admin")] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet()]
        public IActionResult GetItems()
        {
            var categories = _categoryRepository.GetItemsAsync();

            var viewModel = categories
                .Select(o => new CategoryViewModel()
                {
                    Id = o.Id,
                    Type = o.Category,
                    CategoryItems = o.CategoryItems.Select(oi => new CatalogTypeViewModel()
                    {
                        Id = oi.Id,
                        Type = oi.Type
                    }).ToList()

                });
            return View(viewModel);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetById(int categoryId)
        {
            var categories = _categoryRepository.GetItemsAsync().Where(x => x.Id == categoryId);

            var viewModel = categories
                .Select(o => new CategoryViewModel()
                {
                    Id = o.Id,
                    Type = o.Category,
                    CategoryItems = o.CategoryItems.Select(oi => new CatalogTypeViewModel()
                    {
                        Id = oi.Id,
                        Type = oi.Type
                    }).ToList()

                });
            return View(viewModel);
        }

        [HttpGet("")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost()]
        public IActionResult Create(CategoryViewModel model)
        {
            _categoryRepository.CreateCategory(model.Type);
            return RedirectToAction("GetItems");
        }

        [HttpGet("categoryId")]
        public IActionResult CreateType(int categoryId)
        {
            ViewData["categoryId"] = categoryId;
            return View();
        }

        [HttpPost("categoryId")]
        public IActionResult CreateType(CatalogTypeViewModel model)
        {
            _categoryRepository.EditCategory(model.categoryId, model.Type);
            return RedirectToAction("GetItems");
        }

        [HttpPost]
        public IActionResult Delete(int categoryId)
        {
            _categoryRepository.DeleteCategory(categoryId);
            return RedirectToAction("GetItems");
        }

        public IActionResult ChangeName(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel model = new CategoryViewModel { Id = category.Id, Type = category.Category};
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeName(CategoryViewModel model)
        {
            _categoryRepository.ChangeName(model.Id, model.Type);
            return RedirectToAction("GetItems");
        }

        [HttpPost]
        public IActionResult DeleteType(int categoryId, int id)
        {
            _categoryRepository.DeleteType(categoryId, id);
            return RedirectToAction("GetById", new { categoryId });
        }

        public IActionResult ChangeTypeName(int id, int categoryId)
        {
            var type = _categoryRepository.GetTypeById(id);
            if (type == null)
            {
                return NotFound();
            }
            CatalogTypeViewModel model = new CatalogTypeViewModel { Id = type.Id, Type = type.Type, categoryId = categoryId };
            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeTypeName(CatalogTypeViewModel model)
        {
            _categoryRepository.ChangeNameType(model.categoryId, model.Id, model.Type);
            return RedirectToAction("GetById", new { model.categoryId });
        }
    }
}
