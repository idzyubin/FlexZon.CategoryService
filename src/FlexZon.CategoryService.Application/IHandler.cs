namespace FlexZon.CategoryService.Application;

public interface IHandler<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}