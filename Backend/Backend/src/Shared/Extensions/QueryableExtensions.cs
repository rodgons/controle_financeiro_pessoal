using Backend.Shared.Models.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Backend.Shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginationResponse<T>> ToPaginationResponseAsync<T>(
        this IQueryable<T> source,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginationResponse<T>(items, count, request.PageNumber, request.PageSize);
    }
} 