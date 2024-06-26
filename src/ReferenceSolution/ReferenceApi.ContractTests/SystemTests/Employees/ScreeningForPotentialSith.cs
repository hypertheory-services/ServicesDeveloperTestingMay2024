﻿
using Alba;
using Hypertheory.TestAttributes;
using Microsoft.Extensions.Logging;
using ReferenceApi.ContractTests.Fixtures;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.SystemTests.Employees;
[IntegratedTest]
public class ScreeningForPotentialSith : IClassFixture<SystemsTestFixture>
{
    private readonly IAlbaHost Host;

    private readonly SystemsTestFixture Fixture;
    public ScreeningForPotentialSith(SystemsTestFixture fixture)
    {
        Host = fixture.Host;
        Fixture = fixture;
    }

    [Fact]
    public async Task LogIsWritten()
    {
        var candidate = new EmployeeCreateRequest { FirstName = "Darth", LastName= "Vader" };
        await Host.Scenario(api =>
        {
            api.Post.Json(candidate).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
           
        });

        Assert.True(Fixture.LogContains(LogLevel.Information,"We Have a Possible Sith Lord Darth Vader"));
       
    }

 
}
[IntegratedTest]
public class ScreeningForNonPotentialSithCandidates : IClassFixture<SystemsTestFixture>
{
    private readonly IAlbaHost Host;

    private readonly SystemsTestFixture Fixture;
    public ScreeningForNonPotentialSithCandidates(SystemsTestFixture fixture)
    {
        Host = fixture.Host;
        Fixture = fixture;
    }

    [Fact]

    public async Task LogIsNotWritten()
    {
        var candidate = new EmployeeCreateRequest { FirstName = "Anakin", LastName = "Skywalker" };
        await Host.Scenario(api =>
        {
            api.Post.Json(candidate).ToUrl("/employees");
            api.StatusCodeShouldBe(201);

        });

        Assert.False(Fixture.LogContains("We Have a Possible Sith Lord"));

    }
}




