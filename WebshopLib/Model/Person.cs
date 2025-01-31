using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Person
    {
        private int id;
        private string firstName;
        private string lastName;
        private string email;

        public int Id
        {
            get => id; set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Id must be 0 or a positive number");
                }
                id = value;
            }
        }
        public string FirstName
        {
            get => firstName; set
            {
                if(string.IsNullOrWhiteSpace(value) || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("First name must contain at least 2 letters");
                } 
                firstName = value;
            }
        }
        public string LastName
        {
            get => lastName; set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("Last name must contain at least 2 letters");
                }
                lastName = value;
            }
        }
        public string Email
        {
            get => email; set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains('@') || !value.Contains('.'))
                {
                    throw new ArgumentOutOfRangeException("Email cannot be null and must contain a @ symbol a . symbol");
                }
                email = value;
            }
        }
        public Address AddressObj { get; set; }
    }
}
