using Backend.Features.Emails.SendNegativeBalanceEmail;
using Backend.Features.Expenses.GetBalance;
using Backend.Features.Jobs.CheckNegativeBalance;
using Backend.Shared.Models.Balance;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Test_Backend.Features.Jobs.CheckNegativeBalance;

public class TestableCheckNegativeBalanceJob(
    IServiceProvider serviceProvider,
    ILogger<CheckNegativeBalanceJob> logger,
    IConfiguration configuration)
    : CheckNegativeBalanceJob(serviceProvider, logger, configuration)
{
    public new Task ExecuteAsync(CancellationToken cancellationToken)
        => base.ExecuteAsync(cancellationToken);

    public static TimeSpan CheckInterval => TimeSpan.FromSeconds(1);
}

public class CheckNegativeBalanceJobTests
{
    private readonly ILogger<CheckNegativeBalanceJob> _logger;
    private readonly IMediator _mediator;
    private readonly TestableCheckNegativeBalanceJob _job;

    public CheckNegativeBalanceJobTests()
    {
        _logger = Substitute.For<ILogger<CheckNegativeBalanceJob>>();
        _mediator = Substitute.For<IMediator>();
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "CheckNegativeBalance:CheckTimeHour", "18" },
                { "CheckNegativeBalance:CheckTimeMinute", "0" }
            })
            .Build();

        var serviceScope = Substitute.For<IServiceScope>();
        var serviceProvider = Substitute.For<IServiceProvider>();
        
        var serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        serviceScopeFactory.CreateScope().Returns(serviceScope);
        serviceProvider.GetService(typeof(IServiceScopeFactory)).Returns(serviceScopeFactory);
        
        serviceScope.ServiceProvider.GetService(typeof(IMediator)).Returns(_mediator);
        
        _job = new TestableCheckNegativeBalanceJob(serviceProvider, _logger, configuration);
    }

    [Fact]
    public async Task CheckBalanceAsync_WhenBalanceIsNegative_SendsEmail()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var balances = new List<BalanceDto>
        {
            new() { Date = startOfMonth, AccumulatedBalance = 100 },
            new() { Date = today, AccumulatedBalance = -50 }
        };

        _mediator.Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IEnumerable<BalanceDto>>(balances));

        // Act
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await TestExecuteAsync(_job, cts.Token);

        // Assert
        Assert.Equal(-50, balances.Last().AccumulatedBalance);
        await _mediator.Received(1).Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>());
        await _mediator.Received(1).Send(Arg.Is<SendNegativeBalanceEmailCommand>(cmd => 
            cmd.Balance == -50 && cmd.Date == today), Arg.Any<CancellationToken>());
        _logger.Received(1).Log(
            LogLevel.Warning,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Negative balance detected")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task CheckBalanceAsync_WhenBalanceIsPositive_DoesNotSendEmail()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var balances = new List<BalanceDto>
        {
            new() { Date = startOfMonth, AccumulatedBalance = 100 },
            new() { Date = today, AccumulatedBalance = 150 }
        };

        _mediator.Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IEnumerable<BalanceDto>>(balances));

        // Act
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await TestExecuteAsync(_job, cts.Token);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>());
        await _mediator.DidNotReceive().Send(Arg.Any<SendNegativeBalanceEmailCommand>(), Arg.Any<CancellationToken>());
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Current balance")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public async Task CheckBalanceAsync_WhenNoBalances_DoesNotSendEmail()
    {
        // Arrange
        var balances = new List<BalanceDto>();

        _mediator.Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IEnumerable<BalanceDto>>(balances));

        // Act
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        await TestExecuteAsync(_job, cts.Token);

        // Assert
        await _mediator.Received(1).Send(Arg.Any<GetBalanceQuery>(), Arg.Any<CancellationToken>());
        await _mediator.DidNotReceive().Send(Arg.Any<SendNegativeBalanceEmailCommand>(), Arg.Any<CancellationToken>());
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Is<object>(msg => msg.ToString()!.Contains("Current balance")),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    private static Task TestExecuteAsync(TestableCheckNegativeBalanceJob job, CancellationToken cancellationToken)
        => job.ExecuteAsync(cancellationToken);
} 