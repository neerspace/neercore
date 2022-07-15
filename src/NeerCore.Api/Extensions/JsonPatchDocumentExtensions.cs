using Mapster;
using Microsoft.AspNetCore.JsonPatch;

namespace NeerCore.Api.Extensions;

public static class JsonPatchDocumentExtensions
{
    public static TEntity ApplyTo<TDto, TEntity>(this JsonPatchDocument<TDto> patch, TEntity entity)
            where TDto : class
            where TEntity : notnull
    {
        var dto = entity.Adapt<TDto>();
        patch.ApplyTo(dto);
        dto.Adapt(entity);
        return entity;
    }
}