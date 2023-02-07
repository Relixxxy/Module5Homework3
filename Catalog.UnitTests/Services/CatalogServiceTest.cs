using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;
using Infrastructure.Services.Interfaces;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly CatalogService _catalogService;

    private readonly Mock<ICatalogBrandRepository> _brandRepository;
    private readonly Mock<ICatalogTypeRepository> _typeRepository;
    private readonly Mock<ICatalogItemRepository> _catalogRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    private readonly CatalogItem _catalogItemSuccess = new ()
    {
        Name = "TestName"
    };

    private readonly CatalogItemDto _catalogItemDtoSuccess = new ()
    {
        Name = "TestName"
    };

    private readonly CatalogBrand _catalogBrandSuccess = new ()
    {
        Brand = "TestName"
    };

    private readonly CatalogBrandDto _catalogBrandDtoSuccess = new ()
    {
        Brand = "TestName"
    };

    private readonly CatalogType _catalogTypeSuccess = new ()
    {
        Type = "TestName"
    };

    private readonly CatalogTypeDto _catalogTypeDtoSuccess = new ()
    {
        Type = "TestName"
    };

    public CatalogServiceTest()
    {
        _brandRepository = new Mock<ICatalogBrandRepository>();
        _typeRepository = new Mock<ICatalogTypeRepository>();
        _catalogRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogRepository.Object, _brandRepository.Object, _typeRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        _catalogRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        PaginatedItems<CatalogItem> item = null!;

        _catalogRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(item);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogByIdAsync_Success()
    {
        // arrange
        var testCatelogyId = 1;

        _catalogRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_catalogItemSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByIdAsync(testCatelogyId);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogByIdAsync_Failed()
    {
        // arrange
        var testCatelogyId = 1;

        _catalogRepository.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CatalogItem)null!);

        // act
        var result = await _catalogService.GetCatalogByIdAsync(testCatelogyId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogByBrandAsync_Success()
    {
        // arrange
        var testCatelogyBrand = "Brand Name";
        var itemsResults = new List<CatalogItem>()
        {
            _catalogItemSuccess
        };

        _catalogRepository.Setup(s => s.GetByBrandAsync(It.IsAny<string>())).ReturnsAsync(itemsResults);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByBrandAsync(testCatelogyBrand);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogByBrandAsync_Failed()
    {
        // arrange
        var testCatelogyBrand = "Bad Name";
        List<CatalogItem> itemsResults = null!;

        _catalogRepository.Setup(s => s.GetByBrandAsync(It.IsAny<string>())).ReturnsAsync(itemsResults);

        // act
        var result = await _catalogService.GetCatalogByBrandAsync(testCatelogyBrand);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogByTypeAsync_Success()
    {
        // arrange
        var testCatelogyType = "Type Name";
        var itemsResults = new List<CatalogItem>()
        {
            _catalogItemSuccess
        };

        _catalogRepository.Setup(s => s.GetByTypeAsync(It.IsAny<string>())).ReturnsAsync(itemsResults);
        _mapper.Setup(s => s.Map<CatalogItemDto>(It.Is<CatalogItem>(i => i.Equals(_catalogItemSuccess)))).Returns(_catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogByTypeAsync(testCatelogyType);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogByTypeAsync_Failed()
    {
        // arrange
        var testCatelogyType = "Bad Name";
        List<CatalogItem> itemsResults = null!;

        _catalogRepository.Setup(s => s.GetByTypeAsync(It.IsAny<string>())).ReturnsAsync(itemsResults);

        // act
        var result = await _catalogService.GetCatalogByTypeAsync(testCatelogyType);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Success()
    {
        // arrange
        var itemsResults = new List<CatalogBrand>()
        {
            _catalogBrandSuccess
        };

        _brandRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(itemsResults);
        _mapper.Setup(s => s.Map<CatalogBrandDto>(It.Is<CatalogBrand>(i => i.Equals(_catalogBrandSuccess)))).Returns(_catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Failed()
    {
        // arrange
        List<CatalogBrand> itemsResults = null!;

        _brandRepository.Setup(s => s.GetBrandsAsync()).ReturnsAsync(itemsResults);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Success()
    {
        // arrange
        var itemsResults = new List<CatalogType>()
        {
            _catalogTypeSuccess
        };

        _typeRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(itemsResults);
        _mapper.Setup(s => s.Map<CatalogTypeDto>(It.Is<CatalogType>(i => i.Equals(_catalogTypeSuccess)))).Returns(_catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Failed()
    {
        // arrange
        List<CatalogType> itemsResults = null!;

        _typeRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(itemsResults);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().BeNull();
    }
}
