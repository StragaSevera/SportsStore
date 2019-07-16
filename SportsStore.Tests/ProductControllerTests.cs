using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            // Arrange
            var repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());
            var controller = new ProductController(repo.Object) {PageSize = 3};

            // Act
            var result = (ViewResult) controller.List(null, 2);
            var model = (ProductsListViewModel) result.ViewData.Model;

            // Assert
            var prodArray = model.Products.ToArray();
            Assert.Equal(2, prodArray.Length);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }.AsQueryable());
            var controller = new ProductController(repo.Object) {PageSize = 3};

            // Act
            var result = (ViewResult) controller.List(null, 2);
            var model = (ProductsListViewModel) result.ViewData.Model;

            // Assert
            PagingInfo pagingInfo = model.PagingInfo;
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            var controller = new ProductController(repo.Object) {PageSize = 3};

            // Act
            var result = (ViewResult) controller.List("Cat2", 1);
            var model = (ProductsListViewModel) result.ViewData.Model;

            // Assert
            Assert.Equal("Cat2", model.CurrentCategory);

            var prodArray = model.Products.ToArray();
            Assert.Equal(2, prodArray.Length);
            Assert.Equal("P2", prodArray[0].Name);
            Assert.Equal("Cat2", prodArray[0].Category);
            Assert.Equal("P4", prodArray[1].Name);
            Assert.Equal("Cat2", prodArray[1].Category);
        }

        [Fact]
        public void Can_Filter_And_Send_Pagination_View_Model()
        {
            // Arrange
            var repo = new Mock<IProductRepository>();
            repo.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());
            var controller = new ProductController(repo.Object) {PageSize = 3};

            // Act
            var result = (ViewResult) controller.List("Cat2", 1);
            var model = (ProductsListViewModel) result.ViewData.Model;

            // Assert
            PagingInfo pagingInfo = model.PagingInfo;
            Assert.Equal(1, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.ItemsPerPage);
            Assert.Equal(2, pagingInfo.TotalItems);
            Assert.Equal(1, pagingInfo.TotalPages);
        }
    }
}