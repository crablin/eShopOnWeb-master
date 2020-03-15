using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.eShopWeb.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                //catalogContext.Database.Migrate();

                if (!catalogContext.CatalogBrands.Any())
                {
                    catalogContext.CatalogBrands.AddRange(
                        GetPreconfiguredCatalogBrands());

                    await catalogContext.SaveChangesAsync();
                }

                if (!catalogContext.CatalogCategories.Any())
                {
                    catalogContext.CatalogCategories.AddRange(
                        GetPreconfiguredCategories());

                    await catalogContext.SaveChangesAsync();
                }

                if (!catalogContext.CatalogItems.Any())
                {
                    catalogContext.CatalogItems.AddRange(
                        GetPreconfiguredItems());

                    await catalogContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand() { Brand = "Grohe"},
                new CatalogBrand() { Brand = "Santek" },
                new CatalogBrand() { Brand = "DONNA VANNA" },
                new CatalogBrand() { Brand = "Кнауф" },
                new CatalogBrand() { Brand = "BOSCH" },
                new CatalogBrand() { Brand = "Другие" }
            };
        }

        static IEnumerable<CatalogCategory> GetPreconfiguredCategories()
        {
            return new List<CatalogCategory>()
            {
                new CatalogCategory() { Category = "Стройматериалы", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 1, Type = "Герметики"},
                    new CatalogType() {Id = 2, Type = "Изоляция"},
                    new CatalogType() {Id = 3, Type = "Клеи"}
                }},
                new CatalogCategory() { Category = "Сантехника", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 4, Type = "Ванны"},
                    new CatalogType() {Id = 5, Type = "Раковины"},
                    new CatalogType() {Id = 6, Type = "Смесители"}
                }},
                new CatalogCategory() { Category = "Плитка и напольные покрытия", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 7, Type = "Плитка"},
                    new CatalogType() {Id = 8, Type = "Напольные покрытия"}
                }},
                new CatalogCategory() { Category = "Инструменты", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 9, Type = "Электроинструмент"}
                }},
                new CatalogCategory() { Category = "Краски", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 10, Type = "Штукатурка"}
                }},
                new CatalogCategory() { Category = "Электрика и освещение", CategoryItems = new List<CatalogType>()
                {
                    new CatalogType() {Id = 11, Type = "Прожекторы"}
                }},
            };
        }
        //static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        //{
        //    return new List<CatalogType>()
        //    {
        //        new CatalogType() { Type = "Стройматериалы"},
        //        new CatalogType() { Type = "Сантехника" },
        //        new CatalogType() { Type = "Плитка и напольные покрытия" },
        //        new CatalogType() { Type = "Инструменты" },
        //        new CatalogType() { Type = "Электрика и освещение"},
        //        new CatalogType() { Type = "Краски" },
        //        new CatalogType() { Type = "Декоративные материалы" }
        //    };
        //}

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem() {CatalogCategoryId=2, CatalogTypeId=6,CatalogBrandId=1, Description = "Однорычажный латунный смеситель с коротким изливом", Name = "Смеситель для ванны Grohe", Price = 190.05M, PictureUri = "http://catalogbaseurltobereplaced/images/products/1.png", UnitsInStock = 10},
                new CatalogItem() {CatalogCategoryId=2, CatalogTypeId=4,CatalogBrandId=3, Description = "Ванна из стали с антибактериальным эмалированным покрытием и защитой от шума.", Name = "DONNA VANNA Ванна стальная", Price= 178.09M, PictureUri = "http://catalogbaseurltobereplaced/images/products/2.png", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=1, CatalogTypeId=1,CatalogBrandId=4, Description = "Кнауф Унтерпутц 25 кг", Name = "Штукатурка цементная фасадная Кнауф", Price = 8.34M, PictureUri = "http://catalogbaseurltobereplaced/images/products/3.png", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=2, CatalogTypeId=5,CatalogBrandId=2, Description = "Раковина Santek Бриз 55 см", Name = "Раковина Santek Бриз 55 см", Price = 34.91M, PictureUri = "http://catalogbaseurltobereplaced/images/products/4.png", UnitsInStock = 10 },
                                   
                new CatalogItem() {CatalogCategoryId=4, CatalogTypeId=9,CatalogBrandId=5, Description = "Двухскоростная аккумуляторная дрель-шуруповерт для сверления и работ с крепежом", Name = "Дрель-шуруповерт BOSCH GSR 180-Li", Price = 261.73M, PictureUri = "http://catalogbaseurltobereplaced/images/products/drell.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=4, CatalogTypeId=9,CatalogBrandId=6, Description = "Универсальный электролобзик с запасом прочности для бытового и профессионального применения", Name = "Лобзик электрический ELITECH", Price= 141.89M, PictureUri = "http://catalogbaseurltobereplaced/images/products/lobzik.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=3, CatalogTypeId=7,CatalogBrandId=6, Description = "Плитка ПВХ Tarkett Art Vinil,VERSION,457,2 х 457,2 х 3мм", Name = "Плитка ПВХ Tarkett", Price = 34.55M, PictureUri = "http://catalogbaseurltobereplaced/images/products/plitka_grey.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=3, CatalogTypeId=8,CatalogBrandId=6, Description = "Усиленный многослойный линолеум на вспененной основе", Name = "Линолеум бытовой усиленный Diva", Price = 14.44M, PictureUri = "http://catalogbaseurltobereplaced/images/products/linoleum.jpg", UnitsInStock = 10 },
                                  
                new CatalogItem() {CatalogCategoryId=5, CatalogTypeId=10,CatalogBrandId=6, Description = "Цвет – ослепительно белый, 10л", Name = "Краска Dulux 3D White", Price = 87.34M, PictureUri = "http://catalogbaseurltobereplaced/images/products/kraska_1.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=5, CatalogTypeId=10,CatalogBrandId=6, Description = "Для внутренних работ, Для наружных работ", Name = "Краска Фаворит МА-15 синяя 25кг", Price= 64, PictureUri = "http://catalogbaseurltobereplaced/images/products/kraska_2.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=6, CatalogTypeId=11,CatalogBrandId=6, Description = "Галогенный прожектор с фиксированным креплением Navigator белого цвета, мощностью 150Вт.", Name = "Прожектор галогенный", Price = 10, PictureUri = "http://catalogbaseurltobereplaced/images/products/light.jpg", UnitsInStock = 10 },
                new CatalogItem() {CatalogCategoryId=3, CatalogTypeId=8,CatalogBrandId=6, Description = "Трава искусственная Dundee NP 11мм 2м", Name = "Трава искусственная Dundee", Price = 32.99M, PictureUri = "http://catalogbaseurltobereplaced/images/products/grass.jpg", UnitsInStock = 10 }

            };
        }
    }
}
