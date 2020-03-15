using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces
{
    public interface ICategoryRepository : IAsyncRepository<CatalogCategory>
    {
        IQueryable<CatalogCategory> GetItemsAsync();

        void CreateCategory(string categoryName);

        void EditCategory(int categoryId, string typeName);
        void DeleteCategory(int categoryId);

        CatalogCategory GetById(int categoryId);

        CatalogType GetTypeById(int id);
        void ChangeName(int categoryId, string typeName);
        void DeleteType(int categoryId, int id);
        void ChangeNameType(int categoryId, int id, string typeName);

    }
}
