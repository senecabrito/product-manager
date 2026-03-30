using System.Text.Json.Serialization;
using ProductManager.Data.Models;

namespace ProductManager.Business.DTOs
{
    public record ProductDTO
    {
        [JsonPropertyName("code")]
        public Guid Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } =  string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; init; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductCategory Category { get; init; }
    }
}
