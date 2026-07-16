using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDto productDto);
        Task<bool> UpdateAsync(int id, ProductDto productDto);
        Task<bool> DeleteAsync(int id);
    }
}
