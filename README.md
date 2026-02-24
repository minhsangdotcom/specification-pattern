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
public class ListUserSpecification : Specification<User>
{
    public ListUserSpecification(DateTime startDate, DateTime endTime, Gender gender)
    {
        Query
            .Where(x => x.DateOfBirth >= startDate && x.DateOfBirth <= endTime)
            .Where(x => x.Gender == gender)
            .AsNoTracking();
    }
}
```

```csharp
public class ListUserSpecification : Specification<User>
{
    public ListUserSpecification(string term, int page, int pageSize)
    {
        Query
            .Include(x => x.Roles)
            .Where(x => x.FirstName.Contains(term))
            .OrderBy(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .AsSplitQuery();
    }
}
```

```csharp
public class ListUserSpecification : Specification<User>
{
    public ListUserSpecification(string term)
    {
        Query.Where(x => x.FirstName.Contains(term)).AsNoTracking();

        string key = SpecificationCacheKeyGenerator.Create(this, term);
        Query.EnableCache(key);
    }
}

```
