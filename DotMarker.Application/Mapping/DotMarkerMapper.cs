using DotMarker.Application.DTOs;
using DotMarker.Domain.Entities;
using Mapster;

namespace DotMarker.Application.Mapping;

public static class DotMarkerMapper
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<User, UserDto>.NewConfig();
        TypeAdapterConfig<Content, ContentDto>.NewConfig();
    }
}