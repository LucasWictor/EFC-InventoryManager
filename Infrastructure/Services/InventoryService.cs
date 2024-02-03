

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
        //GET ALL
        public async Task<IEnumerable<ProductEntity>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
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
            // Implementation depends on your repository's method for deleting
            return await _productRepository.DeleteAsync(productId);
        }
    }
}
