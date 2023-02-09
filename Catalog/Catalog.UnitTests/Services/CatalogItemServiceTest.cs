using Catalog.Host.Data.Entities;
using Infrastructure.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogItemServiceTest
{
    private readonly ICatalogItemService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;

    private readonly CatalogItem _testItem = new ()
    {
        Id = 1,
        Name = "Name",
        Description = "Description",
        Price = 1000,
        AvailableStock = 100,
        CatalogBrandId = 1,
        CatalogTypeId = 1,
        PictureFileName = "1.png",
    };

    public CatalogItemServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _mapper = new Mock<IMapper>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogItemService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arragne
        var testResult = 1;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Add(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Add(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Add(_testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogItemRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Delete(_testItem.Id);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arragne
        var testResult = _testItem.Id;

        _catalogItemRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Update(_testItem.Id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arragne
        int? testResult = null;

        _catalogItemRepository.Setup(s => s.Update(
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<decimal>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogService.Update(_testItem.Id, _testItem.Name, _testItem.Description, _testItem.Price, _testItem.AvailableStock, _testItem.CatalogBrandId, _testItem.CatalogTypeId, _testItem.PictureFileName);

        // assert
        result.Should().Be(testResult);
    }
}
