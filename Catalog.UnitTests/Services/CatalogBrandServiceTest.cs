using Catalog.Host.Data.Entities;
using Infrastructure.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _brandService;

    private readonly Mock<ICatalogBrandRepository> _catalogbrandRepository;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly CatalogBrand _testItem = new ()
    {
        Id = 1,
        Brand = "Name"
    };

    public CatalogBrandServiceTest()
    {
        _catalogbrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _brandService = new CatalogBrandService(_catalogbrandRepository.Object, _dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arragne
        var testResult = 1;

        _catalogbrandRepository.Setup(s => s.Add(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Add(_testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogbrandRepository.Setup(s => s.Add(It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Add(_testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogbrandRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogbrandRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogbrandRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Update(_testItem.Id, _testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogbrandRepository.Setup(s => s.Update(
             It.IsAny<int>(),
             It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _brandService.Update(_testItem.Id, _testItem.Brand);

        // assert
        result.Should().Be(testResult);
    }
}
