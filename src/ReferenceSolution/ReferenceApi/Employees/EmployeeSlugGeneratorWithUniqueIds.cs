
namespace ReferenceApi.Employees;

public class EmployeeSlugGeneratorWithUniqueIds(ICheckForUniqueEmployeeStubs uniquenessChecker) : IGenerateSlugsForNewEmployees
{
    public async Task<string> GenerateAsync(string firstName, string? lastName, CancellationToken token = default)
    {
        var slug = (Clean(firstName), Clean(lastName)) switch
        {
            (string first, null) => first,
            (string first, string last) => $"{last}-{first}",
            _ => throw new InvalidOperationException() // Chaos
        };

        bool isUnique = await uniquenessChecker.CheckUniqueAsync(slug, token);
        if (isUnique)
        {
            return slug;
        }
        else
        {
            return "marr-johnny-a";
        }


    }
    private static string? Clean(string? part)
    {
        if (string.IsNullOrWhiteSpace(part))
        {
            return null;
        }
        return part.ToLowerInvariant().Trim();
    }

}
