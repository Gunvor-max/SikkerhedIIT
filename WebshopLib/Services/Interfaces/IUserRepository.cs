using WebshopLib.Model;

namespace WebshopLib.Services.Interfaces
{
    public interface IUserRepository
    {
        Person Add(Person user);
        IEnumerable<Person> GetAll();
        Person? GetByEmail(string email);
    }
}