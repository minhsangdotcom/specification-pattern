namespace Specification.Models;

public class PaginationResult<T>(int totalPage, IQueryable<T> results)
{
    public int TotalPage { get; set; } = totalPage;

    public IQueryable<T> Results { get; set; } = results;
}
