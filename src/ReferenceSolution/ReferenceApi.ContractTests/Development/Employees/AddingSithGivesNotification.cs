using Alba;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReferenceApi.ContractTests.Fixtures;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Development.Employees;

/// <summary>
/// This is a "custom" fixture that derives from the HostFixture that allows us to 
/// add custom test services.
/// I try to keep the basic "HostFixture" using only "Fakes" (databases, etc.) but not mocked/stubbed
/// internal implementations. The goal is the eventually make everything just use the HostFixture (and
/// thereby, just Fakes) in System Testing.
/// This is a lesser testing code smell, IMO. Like putting on extra cologne because you didn't get a shower.
/// </summary>
public class SithGrayBoxFixture : SystemsTestFixture
{
    public INotifyOfPossibleSithLords MockedSithNotifier = Substitute.For<INotifyOfPossibleSithLords>();

    /// <summary>
    /// We are doing this because we have no way from the "outside" of the black box to know if this
    /// particular thing (notification) happens or not.
    /// The idea is that *eventually* There would be some actual implementation of INotifyOfPossibleSithLords
    /// interface, and we could "fake" that, if needed. For example, it could publish an event to a event stream,
    /// or make an HTTP call that could be faked.
    /// </summary>
    /// <param name="services"></param>
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        services.AddScoped(sp => MockedSithNotifier);
    }
}
[Trait("Stage", "Development")]
public class AddingSithGivesNotification : IClassFixture<SithGrayBoxFixture>
{
    private readonly IAlbaHost Host;
    private readonly INotifyOfPossibleSithLords MockedSithNotifier;

    public AddingSithGivesNotification(SithGrayBoxFixture fixture)
    {
        Host = fixture.Host;
        MockedSithNotifier = fixture.MockedSithNotifier;
        MockedSithNotifier.ClearReceivedCalls();
    }
    [Fact(Skip = "At this point we are seeing it is in the logs and we have unit tested it. Move out of dev?")]
    public async Task Notifies()
    {

        var request = new EmployeeCreateRequest { FirstName = "Darth", LastName = "Vader" };
        await Host.Scenario((api) =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        MockedSithNotifier
            .Received(1)
            .Notify("Darth", "Vader");
    }

    [Fact(Skip = "At this point we are seeing it is in the logs and we have unit tested it. Move out of dev?")]
    public async Task DoesNotNotifyForNonSith()
    {

        var request = new EmployeeCreateRequest { FirstName = "Anakin", LastName = "Skywalker" };
        await Host.Scenario((api) =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        //THEN
        // Assert on what??
        MockedSithNotifier
            .DidNotReceive()
            .Notify(Arg.Any<string>(), Arg.Any<string>());
    }
}
