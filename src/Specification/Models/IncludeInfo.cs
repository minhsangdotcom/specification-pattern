namespace Specification.Models;

public class IncludeInfo : ExpressionInfo
{
    public InCludeType InCludeType { get; set; }
    public Type? PreviousPropertyType { get; set; }
}

public sealed class IncludeMeta
{
    public string Expression { get; set; } = string.Empty;

    public string? PropertyType { get; set; }
    public string? EntityType { get; set; }
    public string? PreviousPropertyType { get; set; }

    public InCludeType? InCludeType { get; set; }
}
