using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Data.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public Guid Id { get; private set; }

        [Required]
        [Column("name", TypeName = "varchar(100)")] 
        public string Name { get; private set; } = string.Empty;

        [Required]
        [Column("price", TypeName = "decimal(18,2)")] 
        public decimal Price { get; private set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; private set; }

        [Required]
        [Column("category")]
        public ProductCategory Category { get; private set; }

        protected Product() { }

        public Product(string name, decimal price, int quantity, ProductCategory category)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do produto não pode ser vazio.", nameof(name));

            if (name.Trim().Length < 3)
                throw new ArgumentException("O nome do produto deve ter pelo menos 3 caracteres válidos.", nameof(name));

            if (name.Length > 100)
                throw new ArgumentException("O nome não pode ter mais que 100 caracteres.", nameof(name));

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), "O preço não pode ser negativo.");

            if (quantity < 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "A quantidade não pode ser negativa.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Price = price;
            Quantity = quantity;
            Category = category;
        }
    }
}
