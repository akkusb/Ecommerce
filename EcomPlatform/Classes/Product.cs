using System;
namespace EcomPlatform
{
    public class Product
    {
        public string productCode;
        public int price;
        public int stock;
        public int originalPrice;
        public Campaign campaign;

        public Product(string productCode, int price, int stock)
        {
            this.productCode = productCode ?? throw new ArgumentNullException(nameof(productCode));
            this.price = price;
            this.originalPrice = price;
            this.stock = stock;
            this.campaign = null;
        }

        public override string ToString()
        {
            return ("Product " + productCode + " info; price " + price + ", stock " + stock);
        }
    }
}
