using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogCategory : BaseEntity, IAggregateRoot
    {
        public CatalogCategory()
        {
            CategoryItems = new List<CatalogType>();
        }
        public string Category { get; set; }

        public virtual ICollection<CatalogType> CategoryItems { get; set; }
    }
}
