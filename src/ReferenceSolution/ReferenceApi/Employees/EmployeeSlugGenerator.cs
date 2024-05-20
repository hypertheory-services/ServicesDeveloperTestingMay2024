
namespace ReferenceApi.Employees;

public class EmployeeSlugGenerator
{
    public string Generate(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            return firstName.ToLowerInvariant();
        }
        return $"{lastName.ToLowerInvariant()}-{firstName.ToLowerInvariant()}";
    }
}
