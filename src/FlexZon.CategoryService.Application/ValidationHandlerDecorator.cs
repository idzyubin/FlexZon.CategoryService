using System.ComponentModel.DataAnnotations;

namespace FlexZon.CategoryService.Application;

public sealed class BaseHandler<TRequest, TResponse>(IHandler<TRequest, TResponse> inner)
    : IHandler<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var requestErrors = Validate(request);
        if (requestErrors.Count != 0)
        {
            throw new ArgumentException(
                $"Запрос содержит невалидные данные. Ошибки: {string.Join(", ", requestErrors.Select(ToMessage))}",
                nameof(request)
            );
        }

        var response = await inner.Handle(request, cancellationToken);

        var responseErrors = Validate(response);
        if (responseErrors.Count != 0)
        {
            throw new ArgumentException(
                $"Ответ содержит невалидные данные. Ошибки: {string.Join(", ", requestErrors.Select(ToMessage))}",
                nameof(response)
            );
        }

        return response;
    }

    private static IReadOnlyCollection<ValidationResult> Validate<T>(T model) where T : notnull
    {
        var validationContext = new ValidationContext(model);
        var validationErrors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(model, validationContext, validationErrors, true);
        return isValid
            ? Array.Empty<ValidationResult>()
            : validationErrors;
    }

    private static string ToMessage(ValidationResult validationResult) =>
        $"'{validationResult.MemberNames.First()}':'{validationResult.ErrorMessage}'";
}