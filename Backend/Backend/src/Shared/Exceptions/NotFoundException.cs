namespace Backend.Shared.Exceptions;

public class NotFoundError : Exception
{
    public NotFoundError(string message) : base(message)
    {
    }
} 