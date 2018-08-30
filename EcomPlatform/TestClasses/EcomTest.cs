using NUnit.Framework;
using System;
namespace EcomPlatform
{
    [TestFixture()]
    public class EcomTest
    {

        [Test()]
        public static void CreateProductTest()
        {
            string productCode = "P1";
            int price = 100;
            int stock = 1000;
            Product product = Ecom.CreateProduct(productCode, price, stock);

            Assert.AreEqual("Product P1 info; price 100, stock 1000", product.ToString());

        }
    }
}
