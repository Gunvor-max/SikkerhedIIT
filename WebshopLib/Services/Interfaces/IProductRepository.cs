using WebshopLib.Model;

namespace WebshopLib.Services.Repositories
{
    public interface IProductRepository
    {
        Product Add(Product item);
        bool ReserveProduct(string productId);
        bool RemoveReservedProduct(string productId);
        Product GetById(string productId);
        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetFiltered(string filter);
    }
}