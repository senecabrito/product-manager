using Moq;
using ProductManager.Business.DTOs;
using ProductManager.Business.Services.Impl;
using ProductManager.Data.Models;
using ProductManager.Data.Repositories.Abstractions;

namespace ProductManager.Tests.Business.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        private const string ValidName = "Teclado Mecânico";
        private const decimal ValidPrice = 300.00m;
        private const int ValidQuantity = 20;
        private const ProductCategory ValidCategory = ProductCategory.Electronics;

        public ProductServiceTests()
        {
            // O construtor roda antes de cada teste, garantindo um Mock limpo toda vez
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        private Product CreateValidEntity()
        {
            return new Product(ValidName, ValidPrice, ValidQuantity, ValidCategory);
        }

        private ProductDTO CreateValidDTO(Guid id)
        {
            return new ProductDTO
            {
                Id = id,
                Name = ValidName,
                Price = ValidPrice,
                Quantity = ValidQuantity,
                Category = ValidCategory
            };
        }

        [Fact]
        public void FindAll_ShouldReturnMappedDtoList()
        {
            var entityList = new List<Product> { CreateValidEntity(), CreateValidEntity() };

            // "Ensina" o mock: quando chamarem FindAll(), retorne essa lista
            _mockRepo.Setup(repo => repo.FindAll()).Returns(entityList);

            var result = _service.FindAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(entityList[0].Name, result[0].Name);

            _mockRepo.Verify(repo => repo.FindAll(), Times.Once);
        }

        [Fact]
        public void FindById_WhenProductExists_ShouldReturnMappedDto()
        {
            var entity = CreateValidEntity();
            _mockRepo.Setup(repo => repo.FindById(entity.Id)).Returns(entity);

            var result = _service.FindById(entity.Id);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.Name, result.Name);
            _mockRepo.Verify(repo => repo.FindById(entity.Id), Times.Once);
        }

        [Fact]
        public void FindById_WhenProductDoesNotExist_ShouldBubbleUpException()
        {
            var randomId = Guid.NewGuid();

            // Simulamos o repositório lançando a exceção que criamos na camada Data
            _mockRepo.Setup(repo => repo.FindById(randomId))
                     .Throws(new KeyNotFoundException("The product was not found."));

            // O Service não trata a exceção (não tem try/catch), então ela deve subir
            var exception = Assert.Throws<KeyNotFoundException>(() => _service.FindById(randomId));
            Assert.Equal("The product was not found.", exception.Message);
        }

        [Fact]
        public void Create_ShouldMapToEntitySaveAndReturnMappedDto()
        {
            var inputDto = CreateValidDTO(Guid.Empty);
            var savedEntity = CreateValidEntity();

            // Qualquer produto que for passado no Create do repo, retorna a entidade salva
            _mockRepo.Setup(repo => repo.Create(It.IsAny<Product>())).Returns(savedEntity);

            var result = _service.Create(inputDto);

            Assert.NotNull(result);
            Assert.Equal(savedEntity.Id, result.Id);
            _mockRepo.Verify(repo => repo.Create(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Update_ShouldMapToEntityUpdateAndReturnMappedDto()
        {
            var existingId = Guid.NewGuid();
            var updateDto = CreateValidDTO(existingId);
            var updatedEntity = CreateValidEntity();

            typeof(Product).GetProperty("Id")?.SetValue(updatedEntity, existingId);

            _mockRepo.Setup(repo => repo.Update(It.IsAny<Product>())).Returns(updatedEntity);

            var result = _service.Update(updateDto);

            Assert.NotNull(result);
            Assert.Equal(existingId, result.Id);
            _mockRepo.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            var idToDelete = Guid.NewGuid();

            // Como Delete retorna void, só precisamos configurar para não fazer nada (comportamento padrão do Moq)
            _mockRepo.Setup(repo => repo.Delete(idToDelete));

            _service.Delete(idToDelete);

            _mockRepo.Verify(repo => repo.Delete(idToDelete), Times.Once);
        }
    }
}
