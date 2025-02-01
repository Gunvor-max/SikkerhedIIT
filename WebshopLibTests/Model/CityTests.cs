using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebshopLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace WebshopLib.Model.Tests
{
    [TestClass()]
    public class CityTests
    {
        private City city;

        [TestInitialize]
        public void BeforeEachTest()
        {
            //Arrange
            city = new City();
        }

        [TestMethod()]
        public void SetId_Ok()
        {
            //Act
            var expectedResult = 1;
            city.Id = 1;

            //Assert
            Assert.AreEqual(expectedResult,city.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetId_NotOk()
        {
            //Act
            city.Id = -1;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow("123")]
        [DataRow("1234")]
 
        public void SetName_Ok(string name)
        {
            //Act
            city.Name = name;

            //Assert
            Assert.AreEqual(name, city.Name);
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("12")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetName_NotOk(string name)
        {
            //Act
            city.Name = name;

            //Assert
            Assert.Fail();
        }
    }
}