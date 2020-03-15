using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.Extensions.Configuration;

namespace Microsoft.eShopWeb.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IAsyncRepository<Order> _orderRepository;
        private readonly IAsyncRepository<Basket> _basketRepository;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
        private readonly IConfiguration _configuration;


        public OrderService(IAsyncRepository<Basket> basketRepository,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<Order> orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            _basketRepository = basketRepository;
            _itemRepository = itemRepository;
        }

        public async Task CreateOrderAsync(int basketId)
        {
            var basket = await _basketRepository.GetByIdAsync(basketId);
            Guard.Against.NullBasket(basketId, basket);
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var catalogItem = await _itemRepository.GetByIdAsync(item.CatalogItemId);
                catalogItem.UnitsInStock -= item.Quantity;
                var itemOrdered = new CatalogItemOrdered(catalogItem.Id, catalogItem.Name, catalogItem.PictureUri);
                var orderItem = new OrderItem(itemOrdered, item.UnitPrice, item.Quantity);
                items.Add(orderItem);
            }

            
            string connectionString =
                _configuration.GetConnectionString("IdentityConnection");
            
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.TableMappings.Add("Table", "AspNetUsers");
                connection.Open();
                SqlCommand command = new SqlCommand(
                    $"SELECT * FROM [dbo].[AspNetUsers] WHERE [dbo].[AspNetUsers].[UserName] = '{basket.BuyerId}';",
                    connection);
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command;
                DataSet dataSet = new DataSet("AspNetUsers");
               
                adapter.Fill(dataSet);
                connection.Close();

                DataTable table = dataSet.Tables["AspNetUsers"];
                string city = table.Select()[0]["City"].ToString();
                string country = table.Select()[0]["Country"].ToString();
                string street = table.Select()[0]["Street"].ToString();
                var hn = table.Select()[0]["HomeNumber"];
                var flat = table.Select()[0]["Flat"];

                var order = new Order(basket.BuyerId, new Address(street,city,hn.ToString(),country,flat.ToString()), items);
                await _orderRepository.AddAsync(order);
            }
                
        }
    }
}
