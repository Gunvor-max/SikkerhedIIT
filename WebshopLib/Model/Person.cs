using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Person
    {
        #region Instance Fields
        private int id;
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        #endregion

        #region Properties
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

        public string PhoneNumber
        {
            get => phoneNumber; set
            {
                if(value.Length != 8) 
                {

                    throw new ArgumentOutOfRangeException("Phonenumber must be 8 chars in length");
                }
                phoneNumber = value;
            }
        }
        public Address AddressObj { get; set; }
        #endregion

        #region Constructors
        public Person():this(0,"DefaultFirstname","DefaultLastname","DefaultEmail","12345678",new Address())
        {
            
        }
        public Person(int id, string firstName, string lastName, string email, string phoneNumber, Address addressObj)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            AddressObj = addressObj;
        }
        #endregion
    }
}
