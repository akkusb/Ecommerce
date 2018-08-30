using System;
namespace EcomPlatform
{
    public class Order
    {
        public int quantity;
        public Product product;

        public Order(Product product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }
    }
}
