using Bogus;
using SaleManeger.Models;
using System.Collections.ObjectModel;

namespace SaleManegerTests
{
    [TestFixture]
    public class Tests
    {
        public Faker faker = new Faker();
        public DataBase dataBase;
        [SetUp]
        public void SetUp()
        {
            dataBase = new DataBase();
        }
        [Test]
        public void AddingAndReadingSalesFromDB()
        {
            //Arrange
            ObservableCollection<Sale> salesAdded = new ObservableCollection<Sale>();
            dataBase.DeleteAllFromTable("Sales");
            for (int i = 0; i < 200; i++)
            {
                var saleName = faker.Random.Words(4);
                salesAdded.Add(new Sale(saleName));
                dataBase.AddToTable("Sales", ("saleName", saleName));
            }
            ObservableCollection<Sale> salesRead = dataBase.GetSalesList();

            //Act
            var matchingNames = salesAdded.Zip(salesRead, (s1, s2) => s1.SaleDate == s2.SaleDate);

            //Assert
            Assert.AreEqual(salesAdded.Count, 200);
            Assert.AreEqual(salesRead.Count, 200);
            Assert.IsTrue(matchingNames.All(x => x));
        }
        [Test]
        public void AddingAndReadingProductsFromDataBase()
        {
            //Arrange
            dataBase.DeleteAllFromTable("Products");
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            List<string> codes = new List<string>();
            // Generate cods
            for (int i = 0; i < 200; i++)
            {
                string uniqueString;
                do
                {
                    uniqueString = faker.Random.AlphaNumeric(2);
                } while (codes.Contains(uniqueString));
                codes.Add(uniqueString);
            }

            for (int i = 0; i < 200; i++)
            {
                products.Add(new Product()
                {
                    Code = codes[i],
                    Name = faker.Random.Word(),
                    PricePerKg = faker.Random.Double(0, 2000),
                });
            }

            //Act
            dataBase.AddProductsToDatabase(products);
            var readProducts = dataBase.GetProducts();

            //Assert
            Assert.That(products.Count == readProducts.Count);


        }
    }
}