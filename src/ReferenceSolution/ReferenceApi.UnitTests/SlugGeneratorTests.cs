using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorTests
{

    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    [InlineData("Cher", "", "cher")]
    [InlineData(" Joe", "  Schmidt  ", "schmidt-joe")]
    public void GeneratingSlugsForPostToEmployees(string firstName, string lastName, string expected)
    {
        // Given
        var slugGenerator = new EmployeeSlugGenerator();


        // When
        string slug = slugGenerator.Generate(firstName, lastName);


        // Then
        Assert.Equal(expected, slug);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(null, null)]
    public void InvalidInputs(string? first, string? last)
    {
        var slugGenerator = new EmployeeSlugGenerator();

        Assert.Throws<InvalidOperationException>(() => slugGenerator.Generate(first!, last));
    }
}
