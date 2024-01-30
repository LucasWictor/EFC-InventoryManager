

using Infrastructure.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class InventoryService
    {
        private readonly ProductRepository _productRepository;

        public InventoryService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Add method to include logic to check if a product already exists before adding product
        public async Task<ProductEntity> AddProductAsync(ProductEntity product)
        {
            return await _productRepository.CreateAsync(product);
        }

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
    }
}
