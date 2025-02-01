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
    public class AddressTests
    {
        private Address address;

        [TestInitialize]
        public void BeforeEachTest()
        {
            //Arrange
            address = new Address();
        }

        [TestMethod()]
        public void SetId_Ok()
        {
            //Act
            var expectedResult = 1;
            address.Id = 1;

            //Assert
            Assert.AreEqual(expectedResult, address.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetId_NotOk()
        {
            //Act
            address.Id = -1;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow("123")]
        [DataRow("1234")]

        public void SetStreet_Ok(string street)
        {
            //Act
            address.Street = street;

            //Assert
            Assert.AreEqual(street, address.Street);
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("12")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetStreet_NotOk(string street)
        {
            //Act
            address.Street = street;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(199)]

        public void SetHouseNumber_Ok(int houseNumber)
        {
            //Act
            address.HouseNumber = houseNumber;

            //Assert
            Assert.AreEqual(houseNumber, address.HouseNumber);
        }

        [TestMethod()]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(200)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetHouseNumber_NotOk(int houseNumber)
        {
            //Act
            address.HouseNumber = houseNumber;

            //Assert
            Assert.Fail();
        }
    }
}