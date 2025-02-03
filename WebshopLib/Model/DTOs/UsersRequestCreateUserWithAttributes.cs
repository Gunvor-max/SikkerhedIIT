using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model.DTOs
{
    public record UsersRequestCreateUserWithAttributes(string FirstName, string LastName, string Email, string PhoneNumber, RequestAddress AddressObj)
    {
    }
}
