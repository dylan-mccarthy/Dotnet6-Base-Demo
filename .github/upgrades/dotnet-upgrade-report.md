# .NET 9.0 Upgrade Report

## Project target framework modifications

| Project name                                   | Old Target Framework | New Target Framework | Commits   |
|:-----------------------------------------------|:--------------------:|:--------------------:|-----------|
| src/RazorDotnet6/RazorDotnet6.csproj           | net6.0               | net9.0               | b5f7661f  |
| src/APIDotnet6/APIDotnet6.csproj               | net6.0               | net9.0               | 64642cf5  |

## NuGet Packages

| Package Name                           | Old Version | New Version | Commit Id  |
|:---------------------------------------|:-----------:|:-----------:|-----------|
| Microsoft.EntityFrameworkCore.InMemory | 6.0.33      | 9.0.9       | 10d7b995  |
| Microsoft.EntityFrameworkCore.SqlServer| 6.0.33      | 9.0.9       | 10d7b995  |
| Microsoft.EntityFrameworkCore.InMemory | 6.0.33      | 9.0.9       | 71319778  |
| Microsoft.EntityFrameworkCore.SqlServer| 6.0.33      | 9.0.9       | 71319778  |

## All commits

| Commit ID  | Description                                     |
|:-----------|:------------------------------------------------|
| 98992baa   | Commit upgrade plan                             |
| b5f7661f   | Update RazorDotnet6.csproj to target .NET 9.0    |
| 10d7b995   | Update EF Core packages to 9.0.9 in RazorDotnet6.csproj |
| 64642cf5   | Update APIDotnet6.csproj to target .NET 9.0      |
| 71319778   | Update EF Core packages in APIDotnet6.csproj     |

## Project feature upgrades

(No individual feature upgrade sections were required for this plan.)

## Next steps

- Run the solution build and execute any test suites (add tests if missing) to validate runtime behavior.
- Review EF Core 9 breaking changes: adjust query patterns, logging, and configuration if needed.
- Monitor application in lower environments for performance or behavioral differences.
