using System.Threading;
using System.Threading.Tasks;

namespace DrinkApi.Data.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
