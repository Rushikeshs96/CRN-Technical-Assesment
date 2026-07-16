using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName
            });
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return new ProductDto { Id = product.Id, ProductName = product.ProductName };
        }

        public async Task<ProductDto> CreateAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                CreatedBy = "Admin",
                CreatedOn = DateTime.UtcNow
            };

            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            productDto.Id = product.Id;
            return productDto;
        }

        public async Task<bool> UpdateAsync(int id, ProductDto productDto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            product.ProductName = productDto.ProductName;
            product.ModifiedBy = "Admin";
            product.ModifiedOn = DateTime.UtcNow;

            _repository.Update(product);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            _repository.Delete(product);
            return await _repository.SaveChangesAsync() > 0;
        }
    }
}
