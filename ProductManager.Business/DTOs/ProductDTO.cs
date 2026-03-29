using System.Text.Json.Serialization;
using ProductManager.Data.Models;

namespace ProductManager.Business.DTOs
{
    public record ProductDTO
    {
        [JsonIgnore]
        public Guid Id { get; init; }

        public string Name { get; init; } =  string.Empty;

        public decimal Price { get; init; }

        public int Quantity { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductCategory Category { get; init; }
    }
}
