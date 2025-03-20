namespace Backend.Shared.Interfaces;

public interface IEndpoint
{
    public static abstract void MapEndpoints(IEndpointRouteBuilder app);
}