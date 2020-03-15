using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.eShopWeb.Infrastructure.Data
{
    public class CategoryRepository : EfRepository<CatalogCategory>, ICategoryRepository
    {
        public CategoryRepository(CatalogContext dbContext) : base(dbContext)
        {
        }

        public void CreateCategory(string categoryName)
        {
            var lastId = _dbContext.CatalogCategories
                .OrderByDescending(x => x.Id)
                .FirstOrDefault().Id;
            var category = new CatalogCategory()
            {
                Id = lastId + 1,
                CategoryItems = new List<CatalogType>(),
                Category = categoryName
            };
            _dbContext.CatalogCategories.Add(category);
            _dbContext.SaveChanges();
        }

        public void EditCategory(int categoryId, string typeName)
        {
            var category = _dbContext.CatalogCategories.Find(categoryId);
            if (category != null)
            {
                var lastId = _dbContext.CatalogTypes
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault().Id;
                category.CategoryItems.Add(new CatalogType()
                {
                    Id = lastId + 1,
                    Type = typeName
                });
                _dbContext.SaveChanges();
            }
        }

        IQueryable<CatalogCategory> ICategoryRepository.GetItemsAsync()
        {
            return _dbContext.CatalogCategories.Include(x => x.CategoryItems).AsQueryable();
        }

        public void DeleteCategory(int categoryId)
        {
            var category = _dbContext.CatalogCategories.Include(x => x.CategoryItems)
                .FirstOrDefault(x => x.Id == categoryId);
            if (category != null)
            {               
                 _dbContext.CatalogTypes.RemoveRange(category.CategoryItems);
                _dbContext.CatalogCategories.Remove(category);
                _dbContext.SaveChanges();
            }
        }

        public void ChangeName(int categoryId, string typeName)
        {
            var category = _dbContext.CatalogCategories.Find(categoryId);
            if (category != null)
            {
                category.Category = typeName;
                _dbContext.SaveChanges();
            }
        }

        public CatalogCategory GetById(int categoryId)
        {
            return _dbContext.CatalogCategories.Find(categoryId);
        }

        public CatalogType GetTypeById(int id)
        {
            return _dbContext.CatalogTypes.Find(id);
        }

        public void DeleteType(int categoryId, int id)
        {
            var category = _dbContext.CatalogCategories.Include(x => x.CategoryItems)
                .FirstOrDefault(x => x.Id == categoryId);
            if (category != null)
            {
                var type = _dbContext.CatalogTypes.FirstOrDefault(y => y.Id == id);
                if (type != null)
                {
                    _dbContext.CatalogTypes.Remove(type);
                    _dbContext.SaveChanges();
                }
                
            }
        }
        public void ChangeNameType(int categoryId, int id, string typeName)
        {
            var category = _dbContext.CatalogCategories.Include(x => x.CategoryItems)
                .FirstOrDefault(x => x.Id == categoryId);
            if (category != null)
            {
                var type = _dbContext.CatalogTypes.FirstOrDefault(y => y.Id == id);
                if (type != null)
                {
                    type.Type = typeName;
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
