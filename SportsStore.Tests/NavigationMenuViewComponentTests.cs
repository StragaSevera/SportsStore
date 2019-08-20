using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Models;
using SportsStore.Views.Shared.Components.NavigationMenu;
using Xunit;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            var repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat3"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat4"}
            }.AsQueryable());
            var target = new NavigationMenuViewComponent(repo.Object);
            // Act
            ViewDataDictionary data = ((ViewViewComponentResult) target.Invoke()).ViewData;
            var results = (IEnumerable<string>) data.Model;
            // Assert
            Assert.True(new[] {"Cat1", "Cat2", "Cat3", "Cat4"}.SequenceEqual(results));
        }
    }
}