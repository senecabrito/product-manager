using System.Text.Json;
using ProductManager.Business.DTOs;
using ProductManager.Data.Models;

namespace ProductManager.Tests.Business.DTOs
{
    public class ProductDTOTests
    {
        [Fact]
        public void Serialize_ShouldApplyJsonAttributesCorrectly()
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

            // 1. Garante que o JsonIgnore funcionou (o ID não deve estar no JSON)
            Assert.DoesNotContain(dto.Id.ToString(), json);
            Assert.DoesNotContain("\"Id\"", json, StringComparison.OrdinalIgnoreCase);

            // 2. Garante que o Enum foi convertido para string e não para int
            Assert.Contains("\"Category\":\"Electronics\"", json);
        }
    }
}
