using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Halo.Tests
{
    using Cyxor.Serialization;

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }

    [TestClass]
    public class AccountControllerUnitTest
    {
        [TestMethod]
        public void AddNewAccount()
        {
            var item = new Item { Id = 6, Name = "Item 6", Value = 76.34 };

            var serializer = new Serializer(item);

            var newItem = serializer.ToObject<Item>();

            Assert.AreNotEqual(item, newItem);
            Assert.AreEqual(item.Id, newItem.Id);
            Assert.AreEqual(item.Name, newItem.Name);
            Assert.AreEqual(item.Value, newItem.Value);
        }
    }
}
