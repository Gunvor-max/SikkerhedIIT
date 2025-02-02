using WebshopLib.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebshopLib.Model.Tests
{
    [TestClass()]
    public class PersonTests
    {
        private Person person;

        [TestInitialize]
        public void BeforeEachTest()
        {
            //Arrange
            person = new Person();
        }

        [TestMethod()]
        public void SetId_Ok()
        {
            //Act
            var expectedResult = 1;
            person.Id = 1;

            //Assert
            Assert.AreEqual(expectedResult, person.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetId_NotOk()
        {
            //Act
            person.Id = -1;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow("123")]
        [DataRow("1234")]

        public void SetFirstName_Ok(string firstName)
        {
            //Act
            person.FirstName = firstName;

            //Assert
            Assert.AreEqual(firstName, person.FirstName);
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("1")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetFirstName_NotOk(string firstname)
        {
            //Act
            person.FirstName = firstname;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]
        [DataRow("12")]
        [DataRow("123")]

        public void SetLastName_Ok(string lastName)
        {
            //Act
            person.LastName = lastName;

            //Assert
            Assert.AreEqual(lastName, person.LastName);
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("1")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetLastName_NotOk(string lastName)
        {
            //Act
            person.LastName = lastName;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]

        public void SetEmail_Ok()
        {
            //Act
            person.Email = "test@email.dk";
            var expectedResult = "test@email.dk";

            //Assert
            Assert.AreEqual(expectedResult, person.Email);
        }

        [TestMethod()]
        [DataRow("testmaildk")]
        [DataRow("testmail.dk")]
        [DataRow("test@maildk")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetEmail_NotOk(string email)
        {
            //Act
            person.Email = email;

            //Assert
            Assert.Fail();
        }

        [TestMethod()]

        public void SetPhoneNumber_Ok()
        {
            //Act
            person.PhoneNumber = "00000000";
            var expectedResult = 8;

            //Assert
            Assert.AreEqual(expectedResult, person.PhoneNumber.Count());
        }

        [TestMethod()]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("1234567")]
        [DataRow("123456789")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetPhoneNumber_NotOk(string phoneNumber)
        {
            //Act
            person.PhoneNumber = phoneNumber;

            //Assert
            Assert.Fail();
        }
    }
}