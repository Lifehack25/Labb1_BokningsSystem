namespace Labb1_BokningsSystem.Services.UseCases;

public interface IUseCase<TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request);
}