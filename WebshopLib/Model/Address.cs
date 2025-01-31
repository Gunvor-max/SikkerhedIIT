using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Address
    {
        #region Instance fields
        private string street;
        private int houseNumber;
        private int id;
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
        public string Street
        {
            get => street; set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException("Street name must be at least 3 letters");
                }
                street = value;
            }
        }
        public int HouseNumber
        {
            get => houseNumber; set
            {
                if(value < 1 || value > 199)
                {
                    throw new ArgumentOutOfRangeException("House number cannot be less than 1 or greater than 199");
                }
                houseNumber = value;
            }
        }
        public City CityObj { get; set; }
        #endregion

        #region Constructors
        public Address():this(0,"DefaultStreet",1,new City())
        {
            
        }

        public Address(int id, string street, int houseNumber, City cityObj)
        {
            Id = id;
            Street = street;
            HouseNumber = houseNumber;
            CityObj = cityObj;
        }
        #endregion
    }
}
