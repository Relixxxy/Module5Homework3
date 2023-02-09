using AutoMapper;
using Catalog.Host.Configurations;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Microsoft.Extensions.Options;

namespace Catalog.Host.Mapping;

public class CatalogItemPictureResolver : IMemberValueResolver<CatalogItem, CatalogItemDto, string, object>
{
    private readonly CatalogConfig _config;

    public CatalogItemPictureResolver(IOptionsSnapshot<CatalogConfig> config)
    {
        _config = config.Value;
    }

    public object Resolve(CatalogItem source, CatalogItemDto destination, string sourceMember, object destMember, ResolutionContext context)
    {
        return $"{_config.Host}/{_config.ImgUrl}/{sourceMember}";
    }
}