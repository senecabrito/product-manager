using ProductManager.Data.Models;

namespace ProductManager.Data.Repositories.Abstractions
{
    public interface IProductRepository
    {
        List<Product> FindAll();
        Product FindById(Guid id);
        Product Create(Product item);
        Product Update(Product item);
        void Delete(Guid id);
        bool Exists(Guid id);
    }
}
