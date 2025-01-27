using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebshopLib.Model;
using Microsoft.Data.SqlClient;

namespace WebshopLib.Services.Repositories
{
    public class ProduktRepository
    {
        private List<Produkt> _produktList = new List<Produkt>();

        public ProduktRepository()
        {
            Add(new KølVæske(0,new Guid(),"Mælk",13.50,"Arla",1000));
            Add(new KølVæske(0,new Guid(),"Yougurt",24,"Cheasy",1000));
            Add(new KølVæske(0,new Guid(),"Hytteost",24,"Cheasy",500));
        }

        public IEnumerable<Produkt> GetAll()
        {
            string query = "SELECT * FROM KølVæske";
            Produkt produkt = new KølVæske();

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            produkt = ReadItem(reader);
                            _produktList.Add(produkt);
                        }
                    }
                }
            }

            return _produktList;
        }

        public Produkt Add(Produkt item)
        {
            string queryString = "insert into KølVæske Values(@Id,@Varenummer,@Name,@Price,@Model)";
            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@Id", item.Id);
                command.Parameters.AddWithValue("@Varenummer", item.Varenummer);
                command.Parameters.AddWithValue("@Name", item.Name);
                command.Parameters.AddWithValue("@Price", item.Price);
                command.Parameters.AddWithValue("@Model", item.Model);
                int rows = command.ExecuteNonQuery();
                if (rows != 1)
                {
                    throw new ArgumentException("item blev ikke oprettet");
                }

            }
            return item;
        }

        private Produkt ReadItem(SqlDataReader reader)
        {
            KølVæske item = new KølVæske();

            item.Id = reader.GetInt32(0);
            item.Varenummer = reader.GetGuid(1);
            item.Name = reader.GetString(2);
            item.Price = reader.GetDouble(3);
            item.Model = reader.GetString(4);
            item.Mængde = reader.GetInt32(5);

            return item;
        }
    }
}
