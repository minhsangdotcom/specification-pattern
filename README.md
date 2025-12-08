# Specification.Abstractions

A lightweight, provider-agnostic **Specification pattern abstraction** for .NET.

This package defines the **core specification model**, expression metadata, and cache metadata types

> ⚠️ This package does **NOT** include an evaluator.  
> The EF Core-enabled evaluator lives in a separate package (e.g., `TranMinhSang.Specification.EntityFrameworkCore`).

# Install

```
dotnet add package minhsangdotcom.TheTemplate.SpecificationPattern
```

# :book: Basic Usage

```csharp
public class ActiveUsersSpec : Specification<User>
{
    public ActiveUsersSpec(string fullName)
    {
        Query.Where(x => x.IsActive);
        Query.OrderBy(x => x.FullName == fullName);
        string key = SpecificationCacheKeyGenerator.Create(this, fullName);
        Query.EnableCache(key);
    }
}
```
