using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTO;
namespace DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Description = p.Description,
                    StockQuantity = p.StockQuantity
                }).ToListAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return null;

            return new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                StockQuantity = product.StockQuantity
            };
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Price = productDto.Price,
                Description = productDto.Description,
                StockQuantity = productDto.StockQuantity,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Products.AddAsync(product);
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.ProductId);
            if (product == null) return;

            product.ProductName = productDto.ProductName;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.StockQuantity = productDto.StockQuantity;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }
        public async Task<IEnumerable<Product>> GetProductsByPriceAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Select(p => new Product
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Description = p.Description,
                    StockQuantity = p.StockQuantity
                }).ToListAsync();
        }
    }
}
