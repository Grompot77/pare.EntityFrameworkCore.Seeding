using pare.Sample.Entities;
using System.Collections.Generic;
using System.Linq;

namespace pare.Sample.Seeding
{
    public static class Data
    {
        private static Dictionary<string, Country> _countries;
        internal static Dictionary<string, Country> Countries
        {
            get
            {
                if (_countries != null)
                    return _countries;
                var list = new List<Country>
            {
                new Country{Description = "British Indian Ocean Territory",Alpha2Code = "IO"},
                new Country{Description = "British Virgin Islands",Alpha2Code = "VG"},
                new Country{Description = "Burundi",Alpha2Code = "BI"},
                new Country{Description = "Cambodia",Alpha2Code = "KH"},
                new Country{Description = "Cameroon",Alpha2Code = "CM"},
                new Country{Description = "Canada",Alpha2Code = "CA"},
                new Country{Description = "Central African Republic",Alpha2Code = "CF"},
                new Country{Description = "Chad",Alpha2Code = "TD"},
                new Country{Description = "Chile",Alpha2Code = "CL"},
                new Country{Description = "China",Alpha2Code = "CN"},
                new Country{Description = "Christmas Island",Alpha2Code = "CX"}
            };
                return _countries = list.ToDictionary(l => l.Alpha2Code);
            }
        }

        private static Dictionary<string, Customer> _customers;
        internal static Dictionary<string, Customer> Customers
        {
            get
            {
                if (_customers != null)
                    return _customers;
                var list = new List<Customer>
            {
                new Customer{Firstname="Joe", Surname="Blogs", Country = Countries["CA"]},
                new Customer{Firstname="Mary", Surname="Summer", Country = Countries["TD"]},
                new Customer{Firstname="Chris", Surname="Exhausted", Country = Countries["VG"]}
            };
                //If we did countries first, then the id will be populated and we can use it for assignment
                list.ForEach(l => l.CountryId = l.Country.Id);
                return _customers = list.ToDictionary(l => $"{l.Firstname} {l.Surname}".Trim());
            }
        }

        private static Dictionary<string, Product> _products;
        internal static Dictionary<string, Product> Products
        {
            get
            {
                if (_products != null)
                    return _products;
                var list = new List<Product>
            {
                new Product{Description="Socks", Cost=5m},
                new Product{Description="Shirt", Cost=10m},
                new Product{Description="Pants", Cost=20m}
            };
                return _products = list.ToDictionary(l => l.Description);
            }
        }

        internal static List<OrderItem> GetSomeOrderItems()
        {
            var list = new List<OrderItem>();
            var counter = 0;
            counter++;
            list.Add(new OrderItem { Product = Products["Socks"], Qty = 1, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            list.Add(new OrderItem { Product = Products["Shirt"], Qty = 2, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            counter++;
            list.Add(new OrderItem { Product = Products["Socks"], Qty = 6, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            list.Add(new OrderItem { Product = Products["Shirt"], Qty = 4, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            list.Add(new OrderItem { Product = Products["Pants"], Qty = 2, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            counter++;
            list.Add(new OrderItem { Product = Products["Shirt"], Qty = 9, Order = Orders[$"{new string('0', 8) + counter}".GetLast(8)] });
            list.ForEach(l =>
            {
                l.ProductId = l.Product.Id;
                l.OrderId = l.Order.Id;
            });
            return list;
        }

        private static Dictionary<string, Order> _orders;
        internal static Dictionary<string, Order> Orders
        {
            get
            {
                if (_orders != null)
                    return _orders;
                var counter = 0;
                counter++;
                var list = new List<Order>
            {
                new Order
                {
                    Customer = Customers["Joe Blogs"],
                    Number = $"{new string('0',8) + counter++}".GetLast(8),
                },
                new Order
                {
                    Customer = Customers["Chris Exhausted"],
                    Number = $"{new string('0',8) + counter++}".GetLast(8),
                },
                new Order
                {
                    Customer = Customers["Mary Summer"],
                    Number = $"{new string('0',8) + counter++}".GetLast(8),
                }
            };
                return _orders = list.ToDictionary(l => l.Number);
            }
        }
    }
}