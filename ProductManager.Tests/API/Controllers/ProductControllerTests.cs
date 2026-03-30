using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManager.API.Controllers;
using ProductManager.Business.DTOs;
using ProductManager.Business.Services.Abstractions;
using ProductManager.Data.Models;

namespace ProductManager.Tests.API.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly ProductController _controller;

        // Dados padrão para testes
        private const string ValidName = "Teclado Mecânico";
        private const decimal ValidPrice = 300.00m;
        private const int ValidQuantity = 20;
        private const ProductCategory ValidCategory = ProductCategory.Electronics;

        public ProductControllerTests()
        {
            // Inicializa os mocks limpos antes de cada teste
            _mockService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductController>>();

            // Injeta os mocks no Controller
            _controller = new ProductController(_mockService.Object, _mockLogger.Object);
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
        public void FindAll_ShouldReturnOkWithListOfProducts()
        {
            var expectedList = new List<ProductDTO> { CreateValidDTO(Guid.NewGuid()) };
            _mockService.Setup(s => s.FindAll()).Returns(expectedList);

            var result = _controller.FindAll();

            var okResult = Assert.IsType<OkObjectResult>(result); // Garante que retornou HTTP 200 OK
            var returnedList = Assert.IsType<List<ProductDTO>>(okResult.Value); // Garante que o corpo tem a lista
            Assert.Single(returnedList);
        }

        [Fact]
        public void FindById_WhenProductExists_ShouldReturnOkWithProduct()
        {
            var id = Guid.NewGuid();
            var expectedDto = CreateValidDTO(id);
            _mockService.Setup(s => s.FindById(id)).Returns(expectedDto);

            var result = _controller.FindById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(id, returnedDto.Id);
        }

        [Fact]
        public void FindById_WhenProductDoesNotExist_ShouldReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.FindById(id))
                        .Throws(new KeyNotFoundException("The product was not found."));

            var result = _controller.FindById(id);

            Assert.IsType<NotFoundResult>(result); // Garante que retornou HTTP 404 Not Found
        }

        [Fact]
        public void Create_WhenSuccessful_ShouldReturnOkWithCreatedProduct()
        {
            var inputDto = CreateValidDTO(Guid.Empty);
            var expectedDto = CreateValidDTO(Guid.NewGuid()); // Simulando o ID preenchido após criar
            _mockService.Setup(s => s.Create(inputDto)).Returns(expectedDto);

            var result = _controller.Create(inputDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(expectedDto.Id, returnedDto.Id);
        }

        [Fact]
        public void Create_WhenFails_ShouldReturnNotFound()
        {
            var inputDto = CreateValidDTO(Guid.Empty);
            _mockService.Setup(s => s.Create(inputDto)).Returns((ProductDTO)null!);

            var result = _controller.Create(inputDto);

            Assert.IsType<NotFoundResult>(result); // Conforme a lógica do seu controller
        }

        [Fact]
        public void Update_WhenSuccessful_ShouldReturnOkWithUpdatedProduct()
        {
            var inputDto = CreateValidDTO(Guid.NewGuid());
            _mockService.Setup(s => s.Update(inputDto)).Returns(inputDto);

            var result = _controller.Update(inputDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDto = Assert.IsType<ProductDTO>(okResult.Value);
            Assert.Equal(inputDto.Id, returnedDto.Id);
        }

        [Fact]
        public void Update_WhenProductDoesNotExist_ShouldReturnNotFound()
        {
            var inputDto = CreateValidDTO(Guid.NewGuid());
            _mockService.Setup(s => s.Update(inputDto))
                        .Throws(new KeyNotFoundException("The product was not found."));

            var result = _controller.Update(inputDto);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent()
        {
            var id = Guid.NewGuid();

            var result = _controller.Delete(id);

            Assert.IsType<NoContentResult>(result); // Garante que retornou HTTP 204 No Content
            _mockService.Verify(s => s.Delete(id), Times.Once); // Garante que chamou o service pra deletar
        }

        [Fact]
        public void Delete_WhenProductDoesNotExist_ShouldReturnNotFound()
        {
            var id = Guid.NewGuid();
            _mockService.Setup(s => s.Delete(id))
                        .Throws(new KeyNotFoundException("The product was not found."));

            var result = _controller.Delete(id);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
