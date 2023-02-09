using Infrastructure.UnitTests.Mock;

namespace Infrastructure.UnitTests.Mocks;

public class MockService : BaseDataService<MockDbContext>
{
    public MockService(
        IDbContextWrapper<MockDbContext> dbContextWrapper,
        ILogger<MockService> logger)
        : base(dbContextWrapper, logger)
    {
    }

    public async Task RunWithException()
    {
        await ExecuteSafeAsync(() => throw new Exception());
    }

    public async Task RunWithoutException()
    {
        await ExecuteSafeAsync(() => Task.CompletedTask);
    }
}