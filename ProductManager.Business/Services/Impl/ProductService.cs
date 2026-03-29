using Mapster;
using ProductManager.Business.DTOs;
using ProductManager.Business.Services.Abstractions;
using ProductManager.Data.Models;
using ProductManager.Data.Repositories.Abstractions;

namespace ProductManager.Business.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public List<ProductDTO> FindAll()
        {

            return _repository.FindAll().Adapt<List<ProductDTO>>();
        }

        public ProductDTO FindById(Guid id)
        {
            return _repository.FindById(id).Adapt<ProductDTO>();
        }

        public ProductDTO Create(ProductDTO product)
        {
            var entity = product.Adapt<Product>();
            entity = _repository.Create(entity);
            return entity.Adapt<ProductDTO>();
        }

        public ProductDTO Update(ProductDTO product)
        {
            var entity = product.Adapt<Product>();
            entity = _repository.Update(entity);
            return entity.Adapt<ProductDTO>();
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

    }
}
