using Backend.Features.Emails.SendNegativeBalanceEmail;
using Backend.Features.Expenses.GetBalance;
using MediatR;

namespace Backend.Features.Jobs.CheckNegativeBalance;

public class CheckNegativeBalanceJob(
    IServiceProvider serviceProvider,
    ILogger<CheckNegativeBalanceJob> logger,
    IConfiguration configuration)
    : BackgroundService
{
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);
    private readonly int _checkTimeHour = configuration.GetValue("CheckNegativeBalance:CheckTimeHour", 18);
    private readonly int _checkTimeMinute = configuration.GetValue("CheckNegativeBalance:CheckTimeMinute", 0);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var currentTime = DateTime.UtcNow;
                if (currentTime.Hour >= _checkTimeHour && currentTime.Minute >= _checkTimeMinute)
                {
                    await CheckBalanceAsync(stoppingToken);
                    // After running the check, wait until next day at configured time
                    var nextRun = currentTime.Date.AddDays(1).AddHours(_checkTimeHour).AddMinutes(_checkTimeMinute);
                    var delay = nextRun - currentTime;
                    await Task.Delay(delay, stoppingToken);
                }
                else
                {
                    // If it's not time yet, wait for an hour before checking again
                    await Task.Delay(_checkInterval, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while checking balance");
            }
        }
    }

    private async Task CheckBalanceAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var today = DateTime.UtcNow.Date;
        var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        
        var query = new GetBalanceQuery(startOfMonth, today);
        var balances = await mediator.Send(query, cancellationToken);
        
        var currentBalance = balances.LastOrDefault();
        if (currentBalance is { AccumulatedBalance: < 0 })
        {
            logger.LogWarning("Negative balance detected: {Balance:C2}", currentBalance.AccumulatedBalance);
            await mediator.Send(new SendNegativeBalanceEmailCommand(currentBalance.AccumulatedBalance, today), cancellationToken);
        }
        else
        {
            logger.LogInformation("Current balance: {Balance:C2}", currentBalance?.AccumulatedBalance ?? 0);
        }
    }
} 