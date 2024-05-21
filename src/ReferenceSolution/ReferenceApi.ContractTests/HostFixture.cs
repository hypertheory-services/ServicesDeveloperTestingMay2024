using Alba;
using Microsoft.AspNetCore.TestHost;


namespace ReferenceApi.ContractTests;
public class HostFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;

    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>(config =>
        {
            config.ConfigureTestServices(services =>
            {
                //var testFakeButtress = Substitute.For<ICheckForUniqueEmployeeStubs>();
                //testFakeButtress.CheckUniqueAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(true);
                //services.AddScoped<ICheckForUniqueEmployeeStubs>(sp => testFakeButtress);
            });
        });
    }
    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }

}
