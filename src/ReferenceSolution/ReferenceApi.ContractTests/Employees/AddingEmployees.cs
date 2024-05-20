
using Alba;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Employees;
public class AddingEmployees
{

    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    public async Task Bannana(string firstName, string lastName, string expectedId)
    {
        // Given
        // A Host Per Test (Host Per Class, Collections)
        var request = new EmployeeCreateRequest
        {
            FirstName = firstName,
            LastName = lastName
        };

        var expected = new EmployeeResponseItem
        {

            Id = expectedId,
            FirstName = firstName,
            LastName = lastName
        };
        var host = await AlbaHost.For<Program>();

        var response = await host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        var responseMessage = await response.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(responseMessage);

        Assert.Equal(expected, responseMessage);

    }
}
