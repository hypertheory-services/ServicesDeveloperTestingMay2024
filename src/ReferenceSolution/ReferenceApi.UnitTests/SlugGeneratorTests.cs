
using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorTests
{

    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    [InlineData("Cher", "", "cher")]

    public void Avacado(string firstName, string lastName, string expected)
    {
        // Given
        var slugGenerator = new EmployeeSlugGenerator();


        // When
        string slug = slugGenerator.Generate(firstName, lastName);


        // Then
        Assert.Equal(expected, slug);
    }
}
