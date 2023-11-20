using Microsoft.EntityFrameworkCore;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Context;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence.Persistence;
public class UserPersist : GeralPersist, IUserPersist //herda de GeralPersist e implementa IUserPersist
{
    private readonly ProEventosContext _context;
    public UserPersist(ProEventosContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        IQueryable<User> query = _context.Users.AsNoTracking().OrderBy(u => u.Id);

        return await query.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.AsNoTracking().OrderBy(u => u.UserName)
            .SingleOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
    }
}