using Catalog.Host.Data.Entities;
using Infrastructure.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
    private readonly ICatalogTypeService _typeService;

    private readonly Mock<ICatalogTypeRepository> _catalogtypeRepository;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly CatalogType _testItem = new ()
    {
        Id = 1,
        Type = "Name"
    };

    public CatalogTypeServiceTest()
    {
        _catalogtypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _typeService = new CatalogTypeService(_catalogtypeRepository.Object, _dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arragne
        var testResult = 1;

        _catalogtypeRepository.Setup(s => s.Add(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Add(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogtypeRepository.Setup(s => s.Add(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Add(_testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogtypeRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogtypeRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogtypeRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Update(_testItem.Id, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogtypeRepository.Setup(s => s.Update(
             It.IsAny<int>(),
             It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _typeService.Update(_testItem.Id, _testItem.Type);

        // assert
        result.Should().Be(testResult);
    }
}
