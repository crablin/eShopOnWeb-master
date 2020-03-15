using System.Xml.Serialization;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogItem : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int UnitsInStock { get; set; }
        public int CatalogTypeId { get; set; }
        [XmlIgnore]
        public CatalogType CatalogType { get; set; }
        public int CatalogCategoryId { get; set; }
        [XmlIgnore]
        public CatalogCategory CatalogCategory { get; set; }
        [XmlIgnore]
        public int CatalogBrandId { get; set; }
        [XmlIgnore]
        public CatalogBrand CatalogBrand { get; set; }
    }
}