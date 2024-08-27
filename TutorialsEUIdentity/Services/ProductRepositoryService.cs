using TutorialsEUIdentity.Data;
using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.Services
{
    public interface IProductRepositoryService
    {
        public bool Add(ProductModel product);
    }

    public class ProductRepositoryService : IProductRepositoryService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductRepositoryService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public bool Add(ProductModel product)
        {
            try
            {
                _applicationDbContext.Product.Add(product);
                _applicationDbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
