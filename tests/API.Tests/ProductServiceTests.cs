using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfProductDtos()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, ProductName = "P1" },
                new Product { Id = 2, ProductName = "P2" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("P1", result.First().ProductName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDto_WhenExists()
        {
            var product = new Product { Id = 1, ProductName = "Test" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Test", result.ProductName);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _service.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDtoWithId()
        {
            var dto = new ProductDto { ProductName = "New" };
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);
            _mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("New", result.ProductName);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnTrue_WhenProductExists()
        {
            var existing = new Product { Id = 1, ProductName = "Old" };
            var dto = new ProductDto { ProductName = "Updated" };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.UpdateAsync(1, dto);

            Assert.True(result);
            _mockRepo.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _service.UpdateAsync(1, new ProductDto());

            Assert.False(result);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenProductExists()
        {
            var existing = new Product { Id = 1 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.Delete(It.IsAny<Product>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
            _mockRepo.Verify(r => r.Delete(It.IsAny<Product>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
