using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Produkt
    {
        #region Instance fields
        private int id;
        private string name;
        private double price;
        private string model;
        private Guid varenummer;
        #endregion

        #region Properties
        public int Id
        {
            get => id; set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Id cannot be less than zero");
                }
                id = value;
            }
        }

        public Guid Varenummer
        {
            get => varenummer; set
            {
                varenummer = value;
            }
        }
        public string Name
        {
            get => name; set
            {
                if(string.IsNullOrEmpty(value) || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("Name must be at least 2 characters");
                }
                name = value;
            }
        }

        public double Price
        {
            get => price; set
            {
                if(value < 0)
                {
                    throw new ArgumentOutOfRangeException("Price cannot be less than 0");
                }
                price = value;
            }
        }
        public string Model
        {
            get => model; set
            {
                if(string.IsNullOrEmpty(value) && value.Length < 2) 
                {
                    throw new ArgumentOutOfRangeException("Description must be minimum 2 characters");
                }
                model = value;
            }
        }
        #endregion

        public Produkt():this(0,new Guid(),"DefaultName",0.1,"DefaultDescription")
        {
            
        }

        public Produkt(int id, Guid varenummer, string name, double price, string model)
        {
            Id = id;
            Varenummer = varenummer;
            Name = name;
            Price = price;
            Model = model;
        }
    }
}
