using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Stock
    {
        #region Instance fields
        private int id;
        private int quantity;
        private DateTime lastpurchased;
        #endregion

        #region Properties
        public int Id { get => id; set => id = value; }
        public int Quantity
        {
            get => quantity; set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Quantity cannot be less than 0");
                }
                quantity = value;
            }
        }
        public DateTime Lastpurchased
        {
            get => lastpurchased; set
            {
                lastpurchased = value;
            }
        }
        #endregion


        #region Constructors
        public Stock():this(0,0,DateTime.Now)
        {
            
        }

        public Stock(int id, int quantity, DateTime lastpurchased)
        {
            Id = id;
            Quantity = quantity;
            Lastpurchased = lastpurchased;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"{{{nameof(Id)}={Id.ToString()}, {nameof(Quantity)}={Quantity.ToString()}, {nameof(Lastpurchased)}={Lastpurchased.ToString()}}}";
        }
        #endregion


    }
}
