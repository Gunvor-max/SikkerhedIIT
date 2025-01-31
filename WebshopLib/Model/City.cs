using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class City
    {
        #region Instance fields
        private int id;
        private string name;
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
        public string Name
        {
            get => name; set
            {
                if(string.IsNullOrWhiteSpace(value) || value.Length < 3)
                {
                    throw new ArgumentOutOfRangeException("City name must be at least 3 letters");
                }
                name = value;
            }
        }
        #endregion

        #region Constructors 
        public City():this(0,"DefaultName")
        {
            
        }

        public City(int id, string name)
        {
            Id = id;
            Name = name;
        }
        #endregion
    }
}
