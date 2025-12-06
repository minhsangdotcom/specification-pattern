namespace Specification.Models
{
    public class SpecCacheMetadata(
        string? entityType,
        List<WhereMeta> wheres,
        List<IncludeMeta> includes,
        List<SortMeta> sorts,
        int skip,
        int take,
        bool isNoTracking,
        bool isSplitQuery,
        string? specCacheKey,
        string? selector,
        List<string>? queryParameters
    )
    {
        public string? EntityType { get; set; } = entityType;

        public List<WhereMeta> Wheres { get; set; } = wheres;
        public List<IncludeMeta> Includes { get; set; } = includes;
        public List<SortMeta> Sorts { get; set; } = sorts;

        public int Skip { get; set; } = skip;
        public int Take { get; set; } = take;

        public bool IsNoTracking { get; set; } = isNoTracking;
        public bool IsSplitQuery { get; set; } = isSplitQuery;

        public string? SpecCacheKey { get; set; } = specCacheKey;

        public string? Selector { get; set; } = selector;

        public List<string>? QueryParameters { get; set; } = queryParameters;
    }
}
