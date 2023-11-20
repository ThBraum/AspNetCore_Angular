using ProEventos.Domain.Identity;

namespace ProEventos.Persistence.Contratos;
public interface IUserPersist : IGeralPersistence
{
    Task<IEnumerable<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByUsernameAsync(string username);
}