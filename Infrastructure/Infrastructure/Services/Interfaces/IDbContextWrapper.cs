using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Services.Interfaces;

public interface IDbContextWrapper<T>
     where T : DbContext
{
     T DbContext { get; }
     Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}