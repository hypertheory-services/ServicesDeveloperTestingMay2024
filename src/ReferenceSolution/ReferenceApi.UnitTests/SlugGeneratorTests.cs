using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorTests
{

    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    [InlineData("Cher", "", "cher")]
    [InlineData(" Joe", "Von Schmidt  ", "von_schmidt-joe", Skip = "Waiting")]
    [InlineData("Johnny", "Marr", "marr-johnny")]
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

    [Theory]
    [InlineData("Johnny", "Marr", "marr-johnny-a")]
    public void DuplicatesCreateUniqueSlugs(string firstName, string lastName, string expected)
    {
        var slugGenerator = new EmployeeSlugGenerator();

        var slug = slugGenerator.Generate(firstName, lastName);

        Assert.Equal(expected, slug);
    }
}
