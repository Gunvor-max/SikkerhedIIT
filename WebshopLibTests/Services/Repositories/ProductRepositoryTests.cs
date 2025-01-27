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
        [TestMethod()]
        public void GetALLTestOk()
        {
            //Arrange
            ProductRepository repo = new ProductRepository();

            //Act
            var thelist = repo.GetAll();

            //Assert
            Assert.AreEqual(1, thelist.Count());
        }

        [TestMethod()]
        public void TestOfSetCatagoryOK()
        {
            //Arrange
            ProductRepository repo = new ProductRepository();
            Product product = new Product(0, new Guid(), "LetMælk", "Arla", "Køl", true, 1000, 13.95m, new Stock(0, 10, DateTime.Now));
            var expectedresult = "Køl";
            var actualresult = product.Category;

            //Assert
            Assert.AreEqual(expectedresult,actualresult);
        }

        //[TestMethod()]
        //public void AddTestDBOK()
        //{
        //    //Arrange
        //    ProductRepository repo = new ProductRepository();
        //    Product product = new Product(0,new Guid(),"LetMælk","Arla","Køl",true,1000,13.95m,new Stock(0,10,DateTime.Now));

        //    //Act
        //    repo.Add(product);
        //    var thelist = repo.GetAll();

        //    foreach(var item in thelist)
        //    {
        //        Console.WriteLine(item);
        //    }

        //    //Assert
        //    Assert.AreEqual(1,thelist.Count());
        //}
    }
}