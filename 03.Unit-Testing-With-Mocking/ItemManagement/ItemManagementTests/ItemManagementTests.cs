using NUnit.Framework;
using Moq;
using ItemManagementApp.Services;
using ItemManagementLib.Repositories;
using ItemManagementLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace ItemManagement.Tests
{
    [TestFixture]
    public class ItemServiceTests
    {
        private Mock<IItemRepository> _mockRepository;
        private ItemService _itemService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IItemRepository>();
            _itemService = new ItemService(_mockRepository.Object);

        }

        [Test]
        public void AddItem_ShouldCallAddItemOnRepository()
        {
            
            _itemService.AddItem("Book");

            _mockRepository.Verify(repo => repo.AddItem(It.Is<Item>(item => item.Name == "Book")), Times.Once);

        }

        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            var expectedItems = new List<Item>
            {
             new Item { Id = 1, Name = "Pen"},
             new Item { Id = 2, Name = "Pencil"},
             new Item { Id = 3, Name = "Eraser"}
            };

            _mockRepository.Setup(repo => repo.GetAllItems()).Returns(expectedItems);
            var result = _itemService.GetAllItems();

            Assert.That(result, Is.EquivalentTo(expectedItems));
            Assert.That(result.Count(), Is.EqualTo(3));

        }

        [Test]
        public void GetItemById_ShouldReturnItem()
        {
            var expectedItem = new Item { Id = 1, Name = "Pen" };

            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns(expectedItem);
            var result = _itemService.GetItemById(1);

            Assert.That(result, Is.EqualTo(expectedItem));
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Pen"));

        }

        [Test]
        public void UpdateItem_ShouldCallUpdateItemOnRepository()
        {
            var item = new Item { Id = 1, Name = "Old" };

            _mockRepository.Setup(repo => repo.GetItemById(1)).Returns(item);
            _itemService.UpdateItem(1, "New");
            Assert.That(item.Name, Is.EqualTo("New"));
        }

        [Test]
        public void DeleteItem()
        { 
        
            _itemService.DeleteItem(1);
            _mockRepository.Verify(repo => repo.DeleteItem(1), Times.Once);
        }

        [TestCase("Pen", true)]
        [TestCase("asdasdasdasddde", false)]
        [TestCase("", false)]
        [TestCase(null, false)]
        public void ValidateItem_ShouldReturnTrueForValidateName(string name, bool expected)
        { 
            var result = _itemService.ValidateItemName(name);
            Assert.That(result, Is.EqualTo(expected));

        
        }
       
    }
}