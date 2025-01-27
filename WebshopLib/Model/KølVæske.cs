using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class KølVæske:Produkt
    {
        private int mængde;

        public int Mængde
        {
            get => mængde; set
            {
                if(value < 0)
                mængde = value;
            }
        }

        public KølVæske(int id, Guid varenummer, string name, double price, string model,int mængde) : base(id,varenummer,name,price, model)
        {
            Mængde = mængde;
        }

        public KølVæske()
        {
            
        }
    }
}
