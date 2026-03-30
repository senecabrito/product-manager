using ProductManager.Data.Models;

namespace ProductManager.Tests.Data.Model
{
    public class ProductTests
    {
        private const string ValidName = "Teclado Mecânico";
        private const decimal ValidPrice = 250.50m;
        private const int ValidQuantity = 15;
        private const ProductCategory ValidCategory = ProductCategory.Electronics;

        [Fact]
        public void Constructor_WithValidParameters_ShouldInstantiateCorrectly()
        {
            // Arrange & Act
            var product = new Product(ValidName, ValidPrice, ValidQuantity, ValidCategory);

            // Assert
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.Equal(ValidName, product.Name);
            Assert.Equal(ValidPrice, product.Price);
            Assert.Equal(ValidQuantity, product.Quantity);
            Assert.Equal(ValidCategory, product.Category);
        }

        [Theory]
        [InlineData(null, "O nome do produto não pode ser vazio.")]
        [InlineData("", "O nome do produto não pode ser vazio.")]
        [InlineData("   ", "O nome do produto não pode ser vazio.")]
        [InlineData("A", "O nome do produto deve ter pelo menos 3 caracteres válidos.")]
        [InlineData(" AB ", "O nome do produto deve ter pelo menos 3 caracteres válidos.")] // Testa o Trim() com tamanho curto
        public void Constructor_WithInvalidName_ShouldThrowArgumentException(string? invalidName, string expectedMessage)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                new Product(invalidName!, ValidPrice, ValidQuantity, ValidCategory));

            Assert.Contains(expectedMessage, exception.Message);
        }

        [Fact]
        public void Constructor_WithNameExceeding100Characters_ShouldThrowArgumentException()
        {
            var longName = new string('A', 101);

            var exception = Assert.Throws<ArgumentException>(() =>
                new Product(longName, ValidPrice, ValidQuantity, ValidCategory));

            Assert.Contains("O nome não pode ter mais que 100 caracteres.", exception.Message);
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-100.00)]
        public void Constructor_WithNegativePrice_ShouldThrowArgumentOutOfRangeException(decimal invalidPrice)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Product(ValidName, invalidPrice, ValidQuantity, ValidCategory));

            Assert.Contains("O preço não pode ser negativo.", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-50)]
        public void Constructor_WithNegativeQuantity_ShouldThrowArgumentOutOfRangeException(int invalidQuantity)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Product(ValidName, ValidPrice, invalidQuantity, ValidCategory));

            Assert.Contains("A quantidade não pode ser negativa.", exception.Message);
        }

        [Fact]
        public void Constructor_WithNameContainingExtraSpaces_ShouldTrimName()
        {
            var nameWithSpaces = "   Monitor Ultrawide   ";

            var product = new Product(nameWithSpaces, ValidPrice, ValidQuantity, ValidCategory);

            Assert.Equal("Monitor Ultrawide", product.Name);
        }
    }
}
