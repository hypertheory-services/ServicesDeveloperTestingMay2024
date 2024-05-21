

using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorWithUniqueIdsTests
{
    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    [InlineData("Cher", "", "cher")]
    [InlineData(" Joe", "Von Schmidt  ", "von_schmidt-joe", Skip = "Waiting")]
    [InlineData("Johnny", "Marr", "marr-johnny")]
    public async Task GeneratingSlugsForPostToEmployees(string firstName, string lastName, string expected)
    {
        // Given
        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(new AlwaysUniqueDummy());


        // When
        string slug = await slugGenerator.GenerateAsync(firstName, lastName);


        // Then
        Assert.Equal(expected, slug);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(null, null)]
    public async Task InvalidInputs(string? first, string? last)
    {
        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(new AlwaysUniqueDummy());

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await slugGenerator.GenerateAsync(first!, last));
    }
    [Theory]
    [InlineData("Johnny", "Marr", "marr-johnny-a")]
    [InlineData("Jeff", "Gonzalez", "gonzalez-jeff-a")]
    public async Task DuplicatesCreateUniqueSlugs(string firstName, string lastName, string expected)
    {
        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(new NeverUniqueDummy());

        var slug = await slugGenerator.GenerateAsync(firstName, lastName);

        Assert.Equal(expected, slug);
    }
}

public class AlwaysUniqueDummy : ICheckForUniqueEmployeeStubs
{
    public Task<bool> CheckUniqueAsync(string slug, CancellationToken token)
    {
        return Task.FromResult(true);
    }
}


public class NeverUniqueDummy : ICheckForUniqueEmployeeStubs
{
    public Task<bool> CheckUniqueAsync(string slug, CancellationToken token)
    {
        return Task.FromResult(false);
    }
}