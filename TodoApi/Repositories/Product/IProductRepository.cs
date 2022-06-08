using TodoApi.Models;


namespace TodoApi.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProductsByQuery(string sku, string price, string name, string description, string manufacturer, string type);
        Product? GetProductById(int id);
        bool SkuTaken(Product product);
        Product PostProduct(Product product);
        Product UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
