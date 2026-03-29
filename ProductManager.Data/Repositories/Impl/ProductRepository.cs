using Microsoft.EntityFrameworkCore;
using ProductManager.Data.Database.Context;
using ProductManager.Data.Models;
using ProductManager.Data.Repositories.Abstractions;

namespace ProductManager.Data.Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly MSSQLContext _context;
        private readonly DbSet<Product> _dataset;

        public ProductRepository(MSSQLContext context)
        {
            _context = context;
            _dataset = _context.Set<Product>();
        }

        public List<Product> FindAll()
        {
            return _dataset.ToList();
        }

        public Product FindById(Guid id)
        {
            var item = _dataset.Find(id);
            if (item == null)
            {
                throw new KeyNotFoundException("The product was not found.");
            }
            return item;
        }

        public Product Create(Product item)
        {
            _dataset.Add(item);
            _context.SaveChanges();
            return item;
        }

        public Product Update(Product item)
        {
            var existingItem = _dataset.Find(item.Id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException("The product was not found.");
            }

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(Guid id)
        {
            var existingItem = _dataset.Find(id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException("The product was not found.");
            }

            _dataset.Remove(existingItem);
            _context.SaveChanges();
        }

        public bool Exists(Guid id)
        {
            return _dataset.Any(e => e.Id == id);
        }
    }
}