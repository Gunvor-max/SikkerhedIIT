using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model.DTOs
{
    public record RequestAddress(string Street, int HouseNumber, RequestCity CityObj)
    {
    }
}
