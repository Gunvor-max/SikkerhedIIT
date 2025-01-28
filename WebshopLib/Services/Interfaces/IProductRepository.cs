using WebshopLib.Model;

namespace WebshopLib.Services.Repositories
{
    public interface IProductRepository
    {
        Product Add(Product item);
        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetFiltered(string filter);
    }
}