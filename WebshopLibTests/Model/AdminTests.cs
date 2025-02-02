using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebshopLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model.Tests
{
    [TestClass()]
    public class AdminTests
    {
        private Admin admin;

        [TestInitialize]
        public void BeforeEachTest()
        {
            //Arrange
            admin = new Admin();
        }

        [TestMethod()]
        public void GetAccessLevel_Ok()
        {
            //Act
            var expectedResult = 1;
            var actualResult = admin.AccessLevel;

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod()]
        public void AddLoginToList_Ok()
        {
            //Arrange
            var loginYesterDay = DateTime.Now.AddDays(-1);

            //Act
            var expectedResult = admin.Logins.Count() + 1;
            admin.Logins.Add(loginYesterDay);

            //Assert
            Assert.AreEqual(expectedResult, admin.Logins.Count());
        }
    }
}