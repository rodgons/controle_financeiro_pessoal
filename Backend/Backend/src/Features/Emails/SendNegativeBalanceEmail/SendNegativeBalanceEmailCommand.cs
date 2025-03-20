using MediatR;

namespace Backend.Features.Emails.SendNegativeBalanceEmail;

public record SendNegativeBalanceEmailCommand(decimal Balance, DateTime Date) : IRequest<Unit>; 