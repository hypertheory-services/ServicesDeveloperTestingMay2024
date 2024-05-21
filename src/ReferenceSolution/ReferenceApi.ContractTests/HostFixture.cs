using Alba;


namespace ReferenceApi.ContractTests;
public class HostFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;

    public async Task InitializeAsync()
    {
        Host = await AlbaHost.For<Program>();
    }
    public async Task DisposeAsync()
    {
        await Host.DisposeAsync();
    }

}
