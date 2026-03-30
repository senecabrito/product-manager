using System.Text.Json;
using ProductManager.Business.DTOs;
using ProductManager.Data.Models;

namespace ProductManager.Tests.Business.DTOs
{
    public class ProductDTOTests
    {
        [Fact]
        public void Serialize_ShouldIncludeIdAndSerializeCategoryAsString()
        {
            var dto = new ProductDTO
            {
                Id = Guid.NewGuid(),
                Name = "Mouse Gamer",
                Price = 150.00m,
                Quantity = 5,
                Category = ProductCategory.Electronics
            };

            var json = JsonSerializer.Serialize(dto);

            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            Assert.True(root.TryGetProperty("Id", out var idProperty));
            Assert.Equal(dto.Id.ToString(), idProperty.GetString());

            Assert.True(root.TryGetProperty("Category", out var categoryProperty));
            Assert.Equal("Electronics", categoryProperty.GetString());
        }

        [Fact]
        public void Deserialize_ShouldPopulateAllFields()
        {
            var id = Guid.NewGuid();
            var json = $@"{{""Id"":""{id}"",""Name"":""Mouse Gamer"",""Price"":150.00,""Quantity"":5,""Category"":""Electronics""}}";

            var dto = JsonSerializer.Deserialize<ProductDTO>(json);

            Assert.NotNull(dto);
            Assert.Equal(id, dto!.Id);
            Assert.Equal("Mouse Gamer", dto.Name);
            Assert.Equal(150.00m, dto.Price);
            Assert.Equal(5, dto.Quantity);
            Assert.Equal(ProductCategory.Electronics, dto.Category);
        }
    }
}
