using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public List<CatalogTypeViewModel> CategoryItems { get; set; } = new List<CatalogTypeViewModel>();
    }
}
