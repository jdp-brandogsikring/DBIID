
using MediatR;

public interface IApiRequestService
    {
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
    }
