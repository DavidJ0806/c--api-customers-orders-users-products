using TodoApi.Models;
using TodoApi.Contexts;

namespace TodoApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public readonly ApiDb db;
        public ProductRepository(ApiDb context)
        {
            this.db = context;
        }
        public IEnumerable<Product> GetProductsByQuery(string sku, string price, string name, string description, string manufacturer, string type)
        {
            return db.Products
            .Where(
              product => (sku == null || sku == product.Sku) && (price == null || price == product.Price.ToString())
              && (name == null || name == product.Name) && (description == null || description == product.Description)
              && (manufacturer == null || manufacturer == product.Manufacturer) && (type == null || type == product.Type)
            );
        }
        public Product? GetProductById(int id)
        {
            return db.Products.Find(Convert.ToInt64(id));
        }
        public bool SkuTaken(Product product)
        {
            return db.Products.Any(Dbproduct => (Dbproduct.Id != product.Id) && (Dbproduct.Sku == product.Sku));
        }
        public Product PostProduct(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return product;
        }
        public Product UpdateProduct(Product product)
        {
            var entry = db.Products.First(p => p.Id == product.Id);
            db.Entry(entry).CurrentValues.SetValues(product);
            db.SaveChanges();
            return product;
        }
        public void DeleteProduct(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}