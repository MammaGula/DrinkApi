using System.Threading;
using System.Threading.Tasks;
using DrinkApi.Data.Interfaces;

namespace DrinkApi.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}

// DrinkRepository.Add/Update/Delete → just marks the entity as Added/Modified/Removed in the DbContext (nothing is written to the DB yet)

// DrinkService (the caller) → calls the repository method, then calls _unitOfWork.SaveChangesAsync() itself, e.g. in DrinkService.cs:60-61: