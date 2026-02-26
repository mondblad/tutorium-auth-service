using Microsoft.EntityFrameworkCore;

namespace Tutorium.AuthService.Infrastructure.Postgres.Repositories
{
    public interface BaseEnty
    {
        public int Id { get; }
    }

    public abstract class BaseRepo<TEnty> where TEnty : class, BaseEnty 
    {
        private readonly PgContext _context;
        protected DbSet<TEnty> Set => _context.Set<TEnty>();

        public BaseRepo(PgContext context)
        {
            _context = context;
        }

        #region CRUD

        private async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task Add(TEnty newEntry)
        {
            await Set.AddAsync(newEntry);
            await SaveChangesAsync();
        }


        public async Task Update(int id)
        {
            /*await Set.Where(t => t.Id == id).ExecuteUpdateAsync(t => t
                .SetProperty()
                .SetProperty()
            );*/
        }

        public async Task<List<TEnty>> Get()
        {
            return await Set.ToListAsync();
        }

        #endregion CRUD
    }
}
