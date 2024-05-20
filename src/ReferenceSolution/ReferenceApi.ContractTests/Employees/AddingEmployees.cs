
using Alba;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Employees;
public class AddingEmployees
{

    [Fact]
    public async Task Bannana()
    {
        // Given
        // A Host Per Test (Host Per Class, Collections)
        var request = new EmployeeCreateRequest
        {
            FirstName = "Boba",
            LastName = "Fett"
        };

        var expected = new EmployeeResponseItem
        {

            Id = "fett-boba",
            FirstName = "Boba",
            LastName = "Fett"
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
