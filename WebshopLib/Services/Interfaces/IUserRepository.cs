using WebshopLib.Model;

namespace WebshopLib.Services.Interfaces
{
    public interface IUserRepository
    {
        Person Add(Person user, string userId);
        IEnumerable<Person> GetAll();
        Person? GetById(string id);
        void UpdateAdminId(string userId);
    }
}