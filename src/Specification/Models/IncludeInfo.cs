namespace Specification.Models;

public class IncludeInfo : ExpressionInfo
{
    public InCludeType InCludeType { get; set; }
    public Type? PreviousPropertyType { get; set; }
}
