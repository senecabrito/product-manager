using Microsoft.EntityFrameworkCore;
using ProductManager.Data.Database.Context;
using ProductManager.Data.Models;
using ProductManager.Data.Repositories.Impl;

namespace ProductManager.Tests.Data.Repositories
{
    public class ProductRepositoryTests
    {
        // Cria um contexto limpo e isolado para cada teste usando um nome único (Guid)
        private MSSQLContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<MSSQLContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MSSQLContext(options);
        }

        private Product CreateValidProduct()
        {
            return new Product("Monitor Gamer", 1500.00m, 10, ProductCategory.Electronics); // Ajuste o Enum se necessário
        }

        [Fact]
        public void Create_ShouldAddProductToDatabase()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var product = CreateValidProduct();

            var result = repository.Create(product);

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(1, context.Set<Product>().Count());
        }

        [Fact]
        public void FindById_WhenProductExists_ShouldReturnProduct()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var product = CreateValidProduct();
            repository.Create(product);

            var result = repository.FindById(product.Id);

            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
        }

        [Fact]
        public void FindById_WhenProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);

            var exception = Assert.Throws<KeyNotFoundException>(() => repository.FindById(Guid.NewGuid()));
            Assert.Equal("The product was not found.", exception.Message);
        }

        [Fact]
        public void FindAll_ShouldReturnAllProducts()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            repository.Create(new Product("Produto 1", 100m, 1, ProductCategory.Electronics));
            repository.Create(new Product("Produto 2", 200m, 2, ProductCategory.Electronics));

            var results = repository.FindAll();

            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void Update_WhenProductExists_ShouldModifyDatabase()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var originalProduct = CreateValidProduct();
            repository.Create(originalProduct);

            // Simula um produto vindo da requisição com os dados alterados
            var updatedProduct = new Product("Monitor Atualizado", 2000m, 5, ProductCategory.Electronics);

            // Truque de teste: Como o ID tem 'private set', usa Reflection para forçar 
            // o mesmo ID no objeto novo, simulando o que a sua API faria.
            typeof(Product).GetProperty("Id")?.SetValue(updatedProduct, originalProduct.Id);

            repository.Update(updatedProduct);

            var dbProduct = context.Set<Product>().Find(originalProduct.Id);
            Assert.Equal("Monitor Atualizado", dbProduct!.Name);
            Assert.Equal(2000m, dbProduct.Price);
        }

        [Fact]
        public void Update_WhenProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var product = CreateValidProduct();

            var exception = Assert.Throws<KeyNotFoundException>(() => repository.Update(product));
            Assert.Equal("The product was not found.", exception.Message);
        }

        [Fact]
        public void Delete_WhenProductExists_ShouldRemoveFromDatabase()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var product = CreateValidProduct();
            repository.Create(product);

            repository.Delete(product.Id);

            Assert.Equal(0, context.Set<Product>().Count());
        }

        [Fact]
        public void Delete_WhenProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);

            var exception = Assert.Throws<KeyNotFoundException>(() => repository.Delete(Guid.NewGuid()));
            Assert.Equal("The product was not found.", exception.Message);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Exists_ShouldReturnCorrectBoolean(bool shouldExist)
        {
            using var context = GetInMemoryContext();
            var repository = new ProductRepository(context);
            var product = CreateValidProduct();

            if (shouldExist)
            {
                repository.Create(product);
            }

            var idToSearch = shouldExist ? product.Id : Guid.NewGuid();

            var exists = repository.Exists(idToSearch);

            Assert.Equal(shouldExist, exists);
        }
    }
}
