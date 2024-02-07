

using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class InventoryService
    {
        private readonly DataContext _dbContext;

        public InventoryService(ProductRepository productRepository, DataContext dbContext)
        {
            _productRepository = productRepository;
            _dbContext = dbContext;
        }
        private readonly ProductRepository _productRepository;

        public InventoryService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        //GET ALL
        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products ?? new List<ProductEntity>(); // Return an empty list if products is null
        }
        // Add method to include logic to check if a product already exists before adding product

        //ADD PRODUCT
        public async Task<ProductEntity> AddProductAsync(ProductEntity product)
        {
            return await _productRepository.CreateAsync(product);
        }

        // UPDATE PRODUCT
        public async Task<bool> UpdateProductAsync(ProductEntity product)
        {

            return await _productRepository.UpdateAsync(product);
        }
        // UPDATE STOCK LEVEL
        public async Task<bool> UpdateStockLevelAsync(int ProductId, int quantity)
        {
            var product = await _productRepository.GetOneAsync(p => p.ProductId == ProductId);
            if (product != null)
            { 
                product.QuantityInStock = quantity;
                return await _productRepository.UpdateAsync(product);
            }
            return false;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            
            return await _productRepository.DeleteAsync(productId);
        }

        public async Task<ManufacturerEntity> EnsureManufacturerExists(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            var manufacturer = await _dbContext.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName == name);
            if (manufacturer == null)
            {
                manufacturer = new ManufacturerEntity { ManufacturerName = name, Address = "Default Address", ContactInfo = "Default Contact Info" };
                _dbContext.Manufacturers.Add(manufacturer);
                await _dbContext.SaveChangesAsync();
            }

            return manufacturer;
        }

        public async Task<ManufacturerEntity> GetManufacturerByIdAsync(int manufacturerId)
        {
            return await _dbContext.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerId == manufacturerId);
        }
        public async Task<ManufacturerEntity> FindManufacturerByNameAsync(string name)
        {
            return await _dbContext.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName == name);
        }
    }
}
