using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<ProductEntity>
    {
        public ProductRepository(DataContext context) : base(context)
        {
        }

        // Find product by ProductId
        public async Task<ProductEntity> FindProductByIdAsync(int productId)
        {
            return await _context.Set<ProductEntity>()
                                 .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        // search function based on title / description.
        public async Task<IEnumerable<ProductEntity>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Set<ProductEntity>()
                                 .Where(p => p.Title.Contains(searchTerm) || p.Description.Contains(searchTerm))
                                 .ToListAsync();
        }



    }
}