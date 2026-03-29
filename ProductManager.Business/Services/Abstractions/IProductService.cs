using ProductManager.Business.DTOs;

namespace ProductManager.Business.Services.Abstractions
{
    public interface IProductService
    {
        List<ProductDTO> FindAll();
        ProductDTO FindById(Guid id);
        ProductDTO Create(ProductDTO product);
        ProductDTO Update(ProductDTO product);
        void Delete(Guid id);
    }
}
