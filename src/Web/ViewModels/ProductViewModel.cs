using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Наименование товара")]
        public string Name { get; set; }
        public string PictureUri { get; set; }
        [Required(ErrorMessage = "Не указана цена")]
        public string Price { get; set; }
        [Required]
        public int CategoryTypeId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Не указано количество товара")]
        public int UnitsInStock { get; set; }

        public CategoryViewModel Category { get; set; }
        public CategoryViewModel CategoryType { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        public string StatusMessage { get; set; }
    }
}
