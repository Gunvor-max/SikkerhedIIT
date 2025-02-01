using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class SystemAdmin : Person
    {
        #region Instance fields
        private int accessLevel = 2;
        private List<DateTime> lastLogin;
        #endregion

        #region Properties
        public int AccessLevel { get => accessLevel; }
        public List<DateTime> LastLogin { get => lastLogin; set => lastLogin = value; }
        #endregion

        #region Constructors
        public SystemAdmin() : this(0, "DefaultFirstname", "DefaultLastname", "Defaultemail", "12345678", new Address(), new List<DateTime>())
        {

        }

        public SystemAdmin(int id, string firstName, string lastName, string email, string phoneNumber, Address adress, List<DateTime> logins) : base(id, firstName, lastName, email, phoneNumber, adress)
        {
            LastLogin = lastLogin;
        }
        #endregion
    }
}
