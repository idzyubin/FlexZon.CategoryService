using System.ComponentModel.DataAnnotations;

namespace FlexZon.CategoryService.Application.UseCases;

public static class GetCategories
{
    public sealed record Request;

    public sealed record Response(IReadOnlyCollection<Category> Categories);

    public sealed record Category(
        [property: Required(ErrorMessage = "Идентификатор является обязательным полем")]
        [property: Range(1, long.MaxValue, ErrorMessage = "Минимальная длина названия - 3 символа")]
        long Id,
        [property: Required(ErrorMessage = "Название категории не может быть пустой строкой")]
        [property: MinLength(3, ErrorMessage = "Минимальная длина названия - 3 символа")]
        string Title,
        string Description
    );

    internal sealed class Handler : IHandler<Request, Response>
    {
        public ValueTask<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}