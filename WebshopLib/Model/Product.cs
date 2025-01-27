using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Product
    {
        #region Instance fields
        private int id;
        private string name;
        private decimal price;
        private string brand;
        private string category;
        private bool isLiquid;
        private int weight;
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
        public string Brand
        {
            get => brand; set
            {
                if(string.IsNullOrEmpty(value) || value.Length < 2) 
                {
                    throw new ArgumentOutOfRangeException("Brand must be minimum 2 characters");
                }
                brand = value;
            }
        }

        public string Category
        {
            get => category; set
            {
                if (string.IsNullOrEmpty(value) || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("Category must be minimum 2 characters");
                }
                category = value;
            }
        }

        public bool IsLiquid
        {
            get => isLiquid; set
            {
                isLiquid = value;
            }
        }
        public int Weight
        {
            get => weight; set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Weight cannot be less than 1");
                }
                weight = value;
            }
        }

        public decimal Price
        {
            get => price; set
            {
                if(value < 1)
                {
                    throw new ArgumentOutOfRangeException("Price cannot be less than 1");
                }
                price = value;
            }
        }

        public Stock StockItem { get; set; }
        #endregion


        #region Constructors 
        public Product():this(0,new Guid(),"DefaultName","DefaultBrand","DefaultCatagory",false,1,1,new Stock())
        {
            
        }

        public Product(int id, Guid varenummer, string name, string brand, string catagory, bool isLiquid, int weight, decimal price, Stock stock)
        {
            Id = id;
            Varenummer = varenummer;
            Name = name;
            Brand = brand;
            Category = catagory;
            IsLiquid = isLiquid;
            Weight = weight;
            Price = price;
            StockItem = stock;
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Varenummer)}={Varenummer.ToString()}, {nameof(Name)}={Name}, {nameof(Brand)}={Brand}, {nameof(Category)}={Category}, {nameof(IsLiquid)}={IsLiquid.ToString()}, {nameof(Weight)}={Weight.ToString()}, {nameof(Price)}={Price.ToString()}, {nameof(StockItem)}={StockItem}}}";
        }

        #endregion
    }
}
