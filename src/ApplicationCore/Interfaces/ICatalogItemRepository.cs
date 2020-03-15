using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces
{
    public interface ICatalogItemRepository : IAsyncRepository<CatalogItem>
    {
        Task<CatalogItem> GetByIdWithItemsAsync(int id);

        IEnumerable<CatalogItem> GetItemsAsync();
        void Create(int categoryId, int typeId, string name, decimal price, string url, string description, int unitsInStock);
        void Delete(int productId);

        void Edit(int productId, int categoryId, int typeId, string name, decimal price, string url,
            string description, int unitsInStock);

        CatalogItem GetById(int productId);
    }
}
