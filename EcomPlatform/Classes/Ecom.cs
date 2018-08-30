using System;
using System.Collections.Generic;
using System.IO;

namespace EcomPlatform
{
    class Ecom
    {
        public static Dictionary<String, Product> products = new Dictionary<string, Product>();
        public static Dictionary<String, Campaign> campaigns = new Dictionary<string, Campaign>();
        public static DateTime time = DateTime.Today;

        public static void Main(string[] args)
        {
            string baseDirectory = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory)).FullName).FullName;

            // First scenario in the document
            //string scenarioFilePath = baseDirectory + @"/Scenario.txt";
            // Second scenario in the document
            //string scenarioFilePath = baseDirectory + @"/Scenario2.txt";
            // Custom scenario
            string scenarioFilePath = baseDirectory + @"/Scenario3.txt";

            string[] lines = File.ReadAllLines(scenarioFilePath);

            string outputFilePath = baseDirectory + @"/Output.txt";

            using (System.IO.StreamWriter file =
                   new System.IO.StreamWriter(outputFilePath))
            {
                foreach (string line in lines)
                {
                    string s = ProcessLine(line);
                    file.WriteLine(s);
                }
            }
        }

        public static string ProcessLine(string line)
        {
            string[] keywords = line.Split(' ');
            string command = keywords[0];

            string output = "Invalid command"; // Could not parse the line

            switch (command)
            {
                case Constants.CreateProduct:
                    {
                        Product product = CreateProduct(keywords[1], Int32.Parse(keywords[2]), Int32.Parse(keywords[3]));
                        products.Add(product.productCode, product);
                        output = "Product created; code " + product.productCode + ", price " + product.price + ", stock " + product.stock;
                        break;
                    }
                case Constants.GetProductInfo:
                    {
                        Product product = GetProductInfo(keywords[1]);
                        output = product.ToString();
                        break;
                    }
                case Constants.CreateOrder:
                    {
                        string productCode = keywords[1];
                        Product product = products[productCode];
                        Order order = CreateOrder(product, Int32.Parse(keywords[2]));
                        output = "Order created; product " + order.product.productCode + ", quantity " + order.quantity;
                        break;
                    }
                case Constants.CreateCampaign:
                    {
                        Campaign campaign = CreateCampaign(keywords[1], keywords[2], Int32.Parse(keywords[3]), Int32.Parse(keywords[4]), Int32.Parse(keywords[5]));
                        campaigns.Add(campaign.name, campaign);
                        output = "Campaign created; name " + campaign.name + ", product " + campaign.productCode + ", duration " + campaign.duration + ", limit " + campaign.limit + ", target sales count " + campaign.targetSalesCount;
                        break;
                    }
                case Constants.GetCampaignInfo:
                    {
                        Campaign campaign = GetCampaignInfo(keywords[1]);
                        output = campaign.ToString();
                        break;
                    }
                case Constants.IncreaseTime:
                    {
                        IncreaseTime(Int32.Parse(keywords[1]));
                        output = "Time is " + time.ToShortTimeString().Split(' ')[0];
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return output;
        }

        public static Product CreateProduct(string productCode, int price, int stock)
        {
            Product product = new Product(productCode, price, stock);
            return product;
        }

        public static Product GetProductInfo(string productCode)
        {
            return products[productCode];
        }

        public static Order CreateOrder(Product product, int quantity)
        {
            Product productOfOrder = new Product(product.productCode, product.price, product.stock);
            Order order = new Order(productOfOrder, quantity);
            if (product.campaign != null)
            {
                product.campaign.totalSalesCount += quantity;
                product.campaign.totalSalesPrice += product.price * quantity;
                product.campaign.orders.Add(order);
            }
            return order;
        }

        public static Campaign CreateCampaign(string name, string productCode, int duration, int limit, int targetSaleCount)
        {
            Product product = products[productCode];
            Campaign campaign = new Campaign(name, productCode, duration, limit, targetSaleCount, product.stock);
            product.campaign = campaign;
            return campaign;
        }

        public static Campaign GetCampaignInfo(string name)
        {
            return campaigns[name];
        }

        public static void IncreaseTime(int hour)
        {
            time = time.AddHours(hour);
            foreach (Campaign campaign in campaigns.Values)
            {
                campaign.remainingTime -= hour;
                ProcessCampaign(campaign);
            }

        }

        public static void RemoveCampaign(Campaign campaign)
        {
            Product product = products[campaign.productCode];
            campaign.statusIsActive = false;
            product.campaign = null;
            product.price = product.originalPrice;

        }

        public static void ProcessCampaign(Campaign campaign)
        {
            Product product = products[campaign.productCode];
            if (campaign.remainingTime <= 0)
            {
                RemoveCampaign(campaign);
            }
            else // Calculate price
            {

                //For simplicity, used linear discount
                Double remainingTimePercentage = ((Double)campaign.duration - (Double)campaign.remainingTime) / (Double)campaign.duration;
                product.price = product.originalPrice - (int)Math.Ceiling((remainingTimePercentage * (((Double)campaign.limit/100.0) * product.originalPrice)));


                //Double remainingSalePercentage = (Double)campaign.totalSalesCount / (Double)campaign.targetSalesCount;
                //if (remainingSalePercentage > remainingTimePercentage) // Need more discount
                //{

                //}
                //else if (remainingSalePercentage > remainingTimePercentage) // Everything is good, no need for action
                //{

                //}
                //else // Sales are pretty good, increase price
                //{

                //}
            }


        }
    }
}
