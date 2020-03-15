using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace Microsoft.eShopWeb.Infrastructure.Data
{
    public class CatalogItemRepository : EfRepository<CatalogItem>, ICatalogItemRepository
    {
        public CatalogItemRepository(CatalogContext dbContext) : base(dbContext)
        {
            
        }

        public Task<CatalogItem> GetByIdWithItemsAsync(int id)
        {
            return _dbContext.CatalogItems.Include(o => o.CatalogType).Include(p => p.CatalogCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<CatalogItem> GetItemsAsync()
        {
            return _dbContext.CatalogItems.Include(o => o.CatalogType).Include(p => p.CatalogCategory);
        }

        public void Create(int categoryId, int typeId, string name, decimal price, string url, string description, int unitsInStock)
        {
            var product = new CatalogItem()
            {
                Id = new int(),
                CatalogCategoryId = categoryId,
                CatalogTypeId = typeId,
                CatalogBrandId = 6,
                Name = name,
                Price = price,
                UnitsInStock = unitsInStock,
                PictureUri = url ?? "/images/products/empty.jpg",
                Description = description
            };
            _dbContext.CatalogItems.Add(product);
            _dbContext.SaveChanges();
        }

        public void Delete(int productId)
        {
            var product = _dbContext.CatalogItems
                .FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                _dbContext.CatalogItems.Remove(product);
                _dbContext.SaveChanges();
            }
        }

        public void Edit(int productId, int categoryId, int typeId, string name, decimal price, string url, string description, int unitsInStock)
        {
            var product = _dbContext.CatalogItems.Find(productId);
            if (product != null)
            {
                product.CatalogCategoryId = categoryId;
                product.CatalogTypeId = typeId;
                product.Name = name;
                product.Price = price;
                product.UnitsInStock = unitsInStock;
                product.PictureUri = url ?? "/images/products/empty.jpg";
                product.Description = description;
                _dbContext.SaveChanges();
            }
        }

        public CatalogItem GetById(int productId)
        {
            return _dbContext.CatalogItems.Find(productId);
        }
    }
}
