using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebshopLib.Model;
using Microsoft.Data.SqlClient;

namespace WebshopLib.Services.Repositories
{
    public class ProductRepository
    {

        public ProductRepository()
        {
        }

        public IEnumerable<Product> GetAll()
        {
            List<Product> _produktList = new List<Product>();
            string query = "SELECT p.Id, p.ItemNumber, p.Name, p.Brand, p.Category, p.IsLiquid, p.Weight, p.Price, s.Stock_Id, s.Quantity, s.LastPurchased FROM SII_TestProduct p JOIN SII_TestStock s ON p.Stock_Id = s.Stock_Id;";

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = ReadItem(reader);
                            _produktList.Add(product);
                        }
                    }
                }
            }

            return _produktList;
        }

        public Product Add(Product item)
        {
            string stockQuery = "INSERT INTO SII_TestStock (Quantity, LastPurchased) VALUES (@Quantity, @LastPurchased); SELECT SCOPE_IDENTITY();";
            string productQuery = "INSERT INTO SII_TestProduct (ItemNumber, Name, Brand, Category, IsLiquid, Weight, Price, Stock_Id) VALUES (@Varenummer, @Name, @Brand, @Category, @IsLiquid, @Weight, @Price, @Stock_Id)";

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert into SII_TestStock and get the generated Stock_Id
                        SqlCommand stockCommand = new SqlCommand(stockQuery, connection, transaction);
                        stockCommand.Parameters.AddWithValue("@Quantity", item.StockItem.Quantity);
                        stockCommand.Parameters.AddWithValue("@LastPurchased", item.StockItem.Lastpurchased);
                        int stockId = Convert.ToInt32(stockCommand.ExecuteScalar());

                        // Insert into SII_TestProduct
                        SqlCommand productCommand = new SqlCommand(productQuery, connection, transaction);
                        productCommand.Parameters.AddWithValue("@Varenummer", item.Varenummer);
                        productCommand.Parameters.AddWithValue("@Name", item.Name);
                        productCommand.Parameters.AddWithValue("@Brand", item.Brand);
                        productCommand.Parameters.AddWithValue("@Category", item.Category);
                        productCommand.Parameters.AddWithValue("@IsLiquid", item.IsLiquid);
                        productCommand.Parameters.AddWithValue("@Weight", item.Weight);
                        productCommand.Parameters.AddWithValue("@Price", item.Price);
                        productCommand.Parameters.AddWithValue("@Stock_Id", stockId);
                        productCommand.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        transaction.Rollback();
                        throw new ArgumentException("Item was not created: " + ex.Message);
                    }
                }
            }
            return item;
        }



        private Product ReadItem(SqlDataReader reader)
        {
            Product product = new Product();
            Stock stock = new Stock();
            product.StockItem = stock;

            product.Id = reader.GetInt32(0);
            product.Varenummer = reader.GetGuid(1);
            product.Name = reader.GetString(2);
            product.Brand = reader.GetString(3);
            product.Category = reader.GetString(4);
            product.IsLiquid = reader.GetBoolean(5);
            product.Weight = reader.GetInt32(6);
            product.Price = reader.GetDecimal(7);
            stock.Id = reader.GetInt32(8);
            stock.Quantity = reader.GetInt32(9);
            stock.Lastpurchased = reader.GetDateTime(10);

            return product;
        }
    }
}
