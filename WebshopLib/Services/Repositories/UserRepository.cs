using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebshopLib.Model;
using WebshopLib.Services.Interfaces;

namespace WebshopLib.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository()
        {

        }

        public IEnumerable<Person> GetAll()
        {
            List<Person> _userlist = new List<Person>();
            string query = "SELECT p.Id, p.FirstName, p.LastName, p.Email, p.PhoneNumber, p.AddressId, a.Street, a.HouseNumber, a.CityId, c.Name FROM SII_Users p JOIN SII_Address a ON p.AddressId = a.Id JOIN SII_City c ON a.CityId = c.Id;";

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Person user = ReadItem(reader);
                            _userlist.Add(user);
                        }
                    }
                }
            }

            return _userlist.AsReadOnly();
        }

        public Person? GetByEmail(string email)
        {
            Person? user = new();
            string query = "SELECT p.Id, p.FirstName, p.LastName, p.Email, p.PhoneNumber, p.AddressId, a.Street, a.HouseNumber, a.CityId, c.Name FROM SII_Users p JOIN SII_Address a ON p.AddressId = a.Id JOIN SII_City c ON a.CityId = c.Id WHERE p.Email = @Email";

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user = ReadItem(reader);
                        }
                    }
                }
            }

            return user;
        }



        public Person Add(Person user)
        {
            string CityQuery = "INSERT INTO SII_City (Name) VALUES (@Name); SELECT SCOPE_IDENTITY();";
            string AddressQuery = "INSERT INTO SII_Address (Street, HouseNumber, CityId) VALUES (@Street, @HouseNumber, @CityId); SELECT SCOPE_IDENTITY();";
            string UserQuery = "INSERT INTO SII_Users (FirstName, LastName, Email, PhoneNumber, AddressId) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @AddressId); SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(Secret.Connectionstring))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //SII_City
                        SqlCommand CityCommand = new SqlCommand(CityQuery, connection, transaction);
                        CityCommand.Parameters.AddWithValue("@Name", user.AddressObj.CityObj.Name);
                        int cityId = Convert.ToInt32(CityCommand.ExecuteScalar());

                        //SII_Address
                        SqlCommand AddressCommand = new SqlCommand(AddressQuery, connection, transaction);
                        AddressCommand.Parameters.AddWithValue("@Street", user.AddressObj.Street);
                        AddressCommand.Parameters.AddWithValue("@HouseNumber", user.AddressObj.HouseNumber);
                        AddressCommand.Parameters.AddWithValue("@CityId", cityId);
                        int addressId = Convert.ToInt32(AddressCommand.ExecuteScalar());

                        //SII_Users
                        SqlCommand UserCommand = new SqlCommand(UserQuery, connection, transaction);
                        UserCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
                        UserCommand.Parameters.AddWithValue("@LastName", user.LastName);
                        UserCommand.Parameters.AddWithValue("@Email", user.Email);
                        UserCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        UserCommand.Parameters.AddWithValue("@AddressId", addressId);
                        int userId = Convert.ToInt32(UserCommand.ExecuteScalar());

                        // Commit transaction
                        transaction.Commit();

                        // return the newly created Id's from IDENTITY
                        user.Id = userId;
                        user.AddressObj.Id = addressId;
                        user.AddressObj.CityObj.Id = cityId;
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if any error occurs
                        transaction.Rollback();
                        throw new ArgumentException("User was not created: " + ex.Message);
                    }
                }
            }
            return user;
        }



        private Person ReadItem(SqlDataReader reader)
        {
            Person person = new Person();
            Address address = new Address();
            City city = new City();
            person.AddressObj = address;
            person.AddressObj.CityObj = city;

            person.Id = reader.GetInt32(0);
            person.FirstName = reader.GetString(1);
            person.LastName = reader.GetString(2);
            person.Email = reader.GetString(3);
            person.PhoneNumber = reader.GetString(4);
            address.Id = reader.GetInt32(5);
            address.Street = reader.GetString(6);
            address.HouseNumber = reader.GetInt32(7);
            city.Id = reader.GetInt32(8);
            city.Name = reader.GetString(9);

            return person;
        }
    }
}
