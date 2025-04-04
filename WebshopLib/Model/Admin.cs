﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model
{
    public class Admin : Person
    {
        #region Instance Fields
        private int accessLevel = 1;
        private List<DateTime> logins;
        #endregion

        #region Properties
        public int AccessLevel { get => accessLevel; }
        public List<DateTime> Logins { get => logins; set => logins = value; }
        #endregion

        #region Constructors
        public Admin():this(0,"DefaultFirstname","DefaultLastname","Default@email.dk","12345678",new Address(),new List<DateTime>())
        {
            
        }

        public Admin(int id, string firstName, string lastName, string email, string phoneNumber, Address adress, List<DateTime> logins) :base(id, firstName, lastName, email, phoneNumber, adress)
        {
            Logins = logins;
        }
        #endregion
    }
}
