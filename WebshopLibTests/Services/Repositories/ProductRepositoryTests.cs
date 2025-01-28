using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebshopLib.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebshopLib.Model;

namespace WebshopLib.Services.Repositories.Tests
{
    [TestClass()]
    public class ProductRepositoryTests
    {
        //[TestMethod()]
        //public void GetALLTestOk()
        //{
        //    //Arrange
        //    IProductRepository repo = new ProductRepository();

        //    //Act
        //    var thelist = repo.GetAll();

        //    //Assert
        //    Assert.AreEqual(1, thelist.Count());
        //}

        [TestMethod()]
        public void TestOfSetCatagoryOK()
        {
            //Arrange
            Product product = new Product(0, Guid.NewGuid(), "Skyr m. skovbær", "Cheasy", "Køl", true, 1000, 32.95m, "https://www.nemlig.com/scommerce/images/skyr-m-skovbaer-0-2.jpg?i=9wpWeSKP/5022797", new Stock(0, 10, DateTime.Now));
            var expectedresult = "Køl";
            var actualresult = product.Category;

            //Assert
            Assert.AreEqual(expectedresult,actualresult);
        }

        [TestMethod()]
        public void AddTestDBOK()
        {
            //Arrange
            IProductRepository repo = new ProductRepository();
            Product product = new Product(0, Guid.NewGuid(), "Skyr m. skovbær", "Cheasy", "Køl", true, 1000, 32.95m, "https://www.nemlig.com/scommerce/images/skyr-m-skovbaer-0-2.jpg?i=9wpWeSKP/5022797", new Stock(0, 50, DateTime.Now));

            //Act
            var expectedresult = repo.GetAll().Count() + 1;
            repo.Add(product);
            var actualresult = repo.GetAll().Count();

            //Assert
            Assert.AreEqual(expectedresult, actualresult);
        }
    }
}