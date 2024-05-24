
using Alba;
using Hypertheory.TestAttributes;
using ReferenceApi.ContractTests.Fixtures;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.SystemTests.Employees;
[IntegratedTest]
public class HiringEmployees : IClassFixture<SystemsTestFixture>
{

    private readonly IAlbaHost Host;
    private readonly Func<string, bool> CheckLogFor;
    public HiringEmployees(SystemsTestFixture fixture)
    {
        Host = fixture.Host;
        CheckLogFor = fixture.LogContains;
    }

    [Theory]
    [ClassData(typeof(EmployeesSampleData))]
    public async Task CanHireNewEmployees(EmployeeCreateRequest candidateEmployee, string expectedId)
    {


        var expectedEmployee = new EmployeeResponseItem
        {
            Id = expectedId,
            FirstName = candidateEmployee.FirstName,
            LastName = candidateEmployee.LastName
        };


        var postResponse = await Host.Scenario(api =>
        {
            api.Post.Json(candidateEmployee).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        var postResponseEmployee = await postResponse.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(postResponseEmployee);

        Assert.Equal(expectedEmployee, postResponseEmployee);

        var getResponse = await Host.Scenario(api =>
        {
            api.Get.Url($"/employees/{postResponseEmployee.Id}");
            api.StatusCodeShouldBeOk();
        });

        var getResponseEmployee = await getResponse.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(getResponseEmployee);

        Assert.Equal(postResponseEmployee, getResponseEmployee);
        Assert.Equal(expectedEmployee, getResponseEmployee);
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


