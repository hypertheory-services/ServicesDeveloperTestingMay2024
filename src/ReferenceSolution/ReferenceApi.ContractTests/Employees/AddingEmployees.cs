﻿
using Alba;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Employees;
public class AddingEmployees : IClassFixture<HostFixture>
{

    private readonly IAlbaHost Host;
    public AddingEmployees(HostFixture fixture)
    {
        Host = fixture.Host;
    }

    [Theory]
    [ClassData(typeof(EmployeesSampleData))]
    public async Task CanHireNewEmployees(EmployeeCreateRequest request, string expectedId)
    {
        // Given
        // A Host Per Test (Host Per Class, Collections)

        var expected = new EmployeeResponseItem
        {
            Id = expectedId,
            FirstName = request.FirstName,
            LastName = request.LastName!
        };


        var response = await Host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        var responseMessage = await response.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(responseMessage);

        Assert.Equal(expected, responseMessage);

    }

    [Fact]
    public async Task ValidationsAreChecked()
    {
        var request = new EmployeeCreateRequest { FirstName = "", LastName = "" }; // BAD Employee


        var response = await Host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(400);
        });
    }
}
