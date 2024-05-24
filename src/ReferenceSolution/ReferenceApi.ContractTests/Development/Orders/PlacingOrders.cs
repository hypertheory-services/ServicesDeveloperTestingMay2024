

using Alba;
using Hypertheory.TestAttributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ReferenceApi.ContractTests.Fixtures;
using ReferenceApi.Order;
using System.Text.Json;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ReferenceApi.ContractTests.Development.Orders;

public class TimeBoundTestFixture : SystemsTestFixture
{
    public DateTimeOffset TestTime;
    public FakeTimeProvider FakeTime = null!;
    protected override void ConfigureTestServices(IServiceCollection services)
    {
        var clockTimeForTest = new DateTimeOffset(new DateTime(1969, 4, 20), TimeSpan.FromHours(-4));
        TestTime = clockTimeForTest;


        var fakeTime = new FakeTimeProvider(clockTimeForTest);
        FakeTime = fakeTime;
        services.AddSingleton<TimeProvider>(sp => fakeTime);
    }
   
}


[InDevelopmentTest(Feature ="Orders")]
public class PlacingOrders : IClassFixture<TimeBoundTestFixture>
{
    private readonly WireMockServer _server;
    private readonly FakeTimeProvider _fakeTime;
    private readonly IAlbaHost _host;
    public PlacingOrders(TimeBoundTestFixture fixture)
    {
        _server = fixture.MockApiServer;
        _fakeTime = fixture.FakeTime;
        _host = fixture.Host;
    }
    [Fact]
    public async Task StubbingHttpCalls()
    {
        var expectedBody = new CustomerLoyaltyTypes.LoyaltyDiscountRequest
        {
            OrderTotal = 121.44,
            PurchaseDate = _fakeTime.GetLocalNow(),
        };

        var bodyToSend = JsonSerializer.Serialize(
            expectedBody,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));

        var stubbedApiResponse = new CustomerLoyaltyTypes.LoyaltyDiscountResponse
        {
            DiscountAmount = 420.69
        };
        _server.Given(
            Request
                .Create()
                    .WithPath("/customers/*/purchase-rewards")
                    .UsingPost()
                    .WithBody(new JsonMatcher(bodyToSend)))
                .RespondWith(
                    Response
                    .Create()
                        .WithBodyAsJson(stubbedApiResponse)
            );


        var request = new CreateOrderRequest
        {
            Items = [
               new OrderItemModel {
                    Price = 10.12M,
                    Qty = 12,
                    Sku = "beer"
                }
               ]
        };


        var response = await _host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/orders");
            api.StatusCodeShouldBe(200);
        });

        var actualBody = await response.ReadAsJsonAsync<CreateOrderResponse>();
        Assert.NotNull(actualBody);

        Assert.Equal(420.69M, actualBody.Discount);
    }
}

