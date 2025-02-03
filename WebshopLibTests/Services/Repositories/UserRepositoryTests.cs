using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebshopLib.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebshopLib.Services.Interfaces;
using WebshopLib.Model;

namespace WebshopLib.Services.Repositories.Tests
{
    [TestClass()]
    public class UserRepositoryTests
    {
        IUserRepository _repo;

        [TestInitialize]
        public void Init() 
        {
            _repo = new UserRepository();
        }

        //[TestMethod()]
        //public void AddTest()
        //{
        //    Person person = new Person(0,"Test","Testing","thismail@mail.dk","11223344",new Address());
        //    var result = _repo.Add(person);
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod()]
        //public void GetAllTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetByEmailTest()
        //{
        //    Assert.Fail();
        //}
    }
}