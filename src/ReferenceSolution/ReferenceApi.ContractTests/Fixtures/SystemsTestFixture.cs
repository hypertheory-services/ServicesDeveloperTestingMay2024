using Alba;
using Meziantou.Extensions.Logging.InMemory;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.PostgreSql;
using WireMock.Server;


namespace ReferenceApi.ContractTests.Fixtures;
public class SystemsTestFixture : IAsyncLifetime
{
    public IAlbaHost Host = null!;
    private InMemoryLoggerProvider _loggerProvider = null!;
    private PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:16.2-bullseye")
        .Build();

    public WireMockServer MockApiServer = null!;
    
    public async Task InitializeAsync()
    {
        // Warning: This is going to hold *everything* logged in a StringWriter, and can get pretty large.
        // Make sure in you appsettings.json you are configuring logging to only log things you actually care about.
        _loggerProvider = new InMemoryLoggerProvider();
        MockApiServer = WireMockServer.Start();
        await _container.StartAsync();
        Host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("ConnectionStrings:data", _container.GetConnectionString());
            config.UseSetting("loyaltyApi", MockApiServer.Url);
            config.ConfigureServices(sp =>
            {
                ConfigureServices(sp);
            });
            config.ConfigureTestServices(services =>
            {

                services.AddSingleton<ILoggerProvider>(_loggerProvider);
                ConfigureTestServices(services);
            });
        });
    }

    public bool LogContains(string message)
    {
        return _loggerProvider.Logs.Any(l => l.Message.Contains(message));
    }
    public bool LogContains(LogLevel level, string message)
    {
        return _loggerProvider.Logs.Where(l => l.LogLevel == level).Any(l => l.Message.Contains(message));
    }
    
    public async Task DisposeAsync()
    {
        await _container.StopAsync();
        await Host.DisposeAsync();
        _loggerProvider.Dispose();
        MockApiServer.Stop();
        MockApiServer.Dispose();
    }

    protected virtual void ConfigureTestServices(IServiceCollection services)
    {

    }
    protected virtual void ConfigureServices(IServiceCollection services) { }
}

