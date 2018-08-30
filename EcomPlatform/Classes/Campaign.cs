using System;
using System.Collections.Generic;

namespace EcomPlatform
{
    public class Campaign
    {
        public string name;
        public string productCode;
        public int duration;
        public int limit;
        public int targetSalesCount;

        public bool statusIsActive;
        public int totalSalesCount;
        public int totalSalesPrice;
        public int initialStock;

        public int remainingTime;
        public List<Order> orders = new List<Order>();

        public Campaign(string name, string productCode, int duration, int limit, int targetSalesCount, int initialStock)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.productCode = productCode ?? throw new ArgumentNullException(nameof(productCode));
            this.duration = duration;
            this.limit = limit;
            this.targetSalesCount = targetSalesCount;
            this.initialStock = initialStock;

            this.statusIsActive = true;
            this.totalSalesCount = 0;
            this.totalSalesPrice = 0;

            this.remainingTime = duration;

        }

        public override string ToString()
        {
            string statusText = statusIsActive ? "Active" : "Ended";
            return ("Campaign " + name + " info; Status " + statusText + ", Target Sales " + targetSalesCount + ", Total Sales " + totalSalesCount + ", Turnover " + GetTurnover() + ", Average Item Price " + GetAverageItemPrice());
        }

        public double GetAverageItemPrice()
        {
            return (double)totalSalesPrice / (double)totalSalesCount;
        }

        public double GetTurnover()
        {
            return (double)totalSalesCount / (((double)initialStock + (initialStock - totalSalesCount)) / 2.0);
        }
    }
}
