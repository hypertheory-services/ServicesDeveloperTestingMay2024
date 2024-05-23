

using Microsoft.Extensions.Logging;
using NSubstitute;
using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
[Trait("Stage", "Unit")]
public class LoggingNotifierTests
{
    /// <summary>
    /// See how sort of dumb this kind of thing is? How much of the code in this test
    /// is actually testing our stuff? Very little.
    /// </summary>
    [Fact]
    public void SithNotifierLogs()
    {

        var _stringWriter = new StringWriter();
        var org = Console.Out;
        Console.SetOut(_stringWriter);
        using (var loggerFactory = LoggerFactory.Create(c => c.AddConsole()))
        {

            var logger = loggerFactory.CreateLogger<LoggingNotifier>();
            INotifyOfPossibleSithLords sut = new LoggingNotifier(logger);

            sut.Notify("Vlad", "Impaler");
        }

        var logged = _stringWriter.ToString();
        Assert.Equal("info: ReferenceApi.Employees.LoggingNotifier[0]\r\n      We Have a Possible Sith Lord Vlad Impaler\r\n", logged);

        Console.SetOut(org);
    }

    [Fact]
    public void Substituting()
    {
        // This isn't much better - maybe worse.
        var mockLogger = Substitute.For<MockLogger<LoggingNotifier>>();

        var sut = new LoggingNotifier(mockLogger);

        sut.Notify("Daisey", "Ridley");

        mockLogger.Received().Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(o => o.ToString() == "We Have a Possible Sith Lord Daisey Ridley"),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
            );
    }


}

public abstract class MockLogger<T> : ILogger<T>
{
    public abstract IDisposable? BeginScope<TState>(TState state) where TState : notnull;


    public abstract bool IsEnabled(LogLevel logLevel);

    public abstract void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter);

}