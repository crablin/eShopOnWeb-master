using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.eShopWeb.Web.Extensions
{
    public static class MainDataSet
    {
        
        //public MainDataSet()
        //{
        //    string connectionString = GetConnectionString();
        //    ConnectToData(connectionString);
        //    ConnectToDataReport2(connectionString);
        //}
        public static void ConnectToData(string connectionString, string dataPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            //Create a SqlConnection to the Northwind database.
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                //Create a SqlDataAdapter for the Suppliers table.
                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "Catalog");

                // Open the connection.
                connection.Open();
                Console.WriteLine("The SqlConnection is open.");

                // Create a SqlCommand to retrieve Suppliers data.
                SqlCommand command = new SqlCommand(
                    "SELECT [dbo].[Catalog].[Name]," +
                        "[dbo].[Catalog].[CatalogCategoryId], [dbo].[Catalog].[Price],[dbo].[Catalog].[UnitsInStock],"+
                "SUM([dbo].[OrderItems].[Units]) [Units], [dbo].[OrderItems].[ItemOrdered_CatalogItemId]" +
                "FROM[dbo].[Catalog] LEFT JOIN[dbo].[OrderItems] ON[dbo].[Catalog].[Id] = [dbo].[OrderItems].[ItemOrdered_CatalogItemId]"+
                "GROUP BY[dbo].[Catalog].[Name], [dbo].[Catalog].[CatalogCategoryId],[dbo].[Catalog].[Price],[dbo].[OrderItems].[ItemOrdered_CatalogItemId], [dbo].[Catalog].[UnitsInStock];",
                    connection);
                command.CommandType = CommandType.Text;

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Catalog");
                adapter.Fill(dataSet);

                // Create a second Adapter and Command to get
                // the Products table, a child table of Suppliers. 
                SqlDataAdapter productsAdapter = new SqlDataAdapter();
                productsAdapter.TableMappings.Add("Table", "CatalogCategory");

                SqlCommand productsCommand = new SqlCommand(
                    "SELECT * FROM [dbo].[CatalogCategory];",
                    connection);
                productsAdapter.SelectCommand = productsCommand;

                // Fill the DataSet.
                productsAdapter.Fill(dataSet);

               // Close the connection.
                connection.Close();
                Console.WriteLine("The SqlConnection is closed.");

                // Create a DataRelation to link the two tables
                // based on the CatalogCategoryId.
                DataColumn childColumn =
                    dataSet.Tables["Catalog"].Columns["CatalogCategoryId"];
                DataColumn parentColumn =
                    dataSet.Tables["CatalogCategory"].Columns["Id"];
                DataRelation relation =
                    new System.Data.DataRelation("CatalogCatalogCategories",
                    parentColumn, childColumn);
                dataSet.Relations.Add(relation);
                Console.WriteLine(
                    "The {0} DataRelation has been created.",
                    relation.RelationName);
                using (TextWriter writer = new StreamWriter(dataPath))
                {
                    serializer.Serialize(writer, dataSet);
                }
            }
        }

        public static void ConnectToDataReport2(string connectionString, string dataPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
            //Create a SqlConnection to the Northwind database.
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                //Create a SqlDataAdapter for the Suppliers table.
                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "Order");

                // Open the connection.
                connection.Open();
                Console.WriteLine("The SqlConnection is open.");

                // Create a SqlCommand to retrieve Suppliers data.
                SqlCommand command = new SqlCommand(
                    "SELECT [dbo].[Orders].*, CAST([dbo].[Orders].[OrderDate] AS datetime) [DateOrder],[dbo].[AspNetUsers].[Email]," +
                    "[dbo].[AspNetUsers].[FirstName], [dbo].[AspNetUsers].[SecondName] " +
                    "FROM [dbo].[Orders]" +
                    "LEFT JOIN [dbo].[AspNetUsers] ON [dbo].[Orders].[BuyerId] = [dbo].[AspNetUsers].[UserName];",
                    connection);
                command.CommandType = CommandType.Text;

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Orders");
                adapter.Fill(dataSet);

                // Create a second Adapter and Command to get
                // the Products table, a child table of Suppliers. 
                SqlDataAdapter productsAdapter = new SqlDataAdapter();
                productsAdapter.TableMappings.Add("Table", "OrderItems");

                SqlCommand productsCommand = new SqlCommand(
                    "SELECT * FROM [dbo].[OrderItems];",
                    connection);
                productsAdapter.SelectCommand = productsCommand;

                // Fill the DataSet.
                productsAdapter.Fill(dataSet);

                // Close the connection.
                connection.Close();
                Console.WriteLine("The SqlConnection is closed.");

                // Create a DataRelation to link the two tables
                // based on the CatalogCategoryId.
                DataColumn childColumn =
                    dataSet.Tables["OrderItems"].Columns["OrderId"];
                DataColumn parentColumn =
                    dataSet.Tables["Order"].Columns["Id"];
                DataRelation relation =
                    new System.Data.DataRelation("Order_Items",
                    parentColumn, childColumn);
                dataSet.Relations.Add(relation);
                Console.WriteLine(
                    "The {0} DataRelation has been created.",
                    relation.RelationName);

                using (TextWriter writer = new StreamWriter(dataPath))
                {
                    serializer.Serialize(writer, dataSet);
                }
            }
        }

         private static string GetConnectionString()
        {
            // To avoid storing the connection string in your code, 
            // you can retrieve it from a configuration file.

            return "Server=localhost;Integrated Security=true;Initial Catalog=BuildShopOnWeb.CatalogDb;"
            ;
        }
    }
}
