using FlexZon.CategoryService.Application;
using FlexZon.CategoryService.Application.UseCases;
using Grpc.Core;

namespace FlexZon.CategoryService.Api.Services;

public class CategoryServiceImplementation(
    IHandler<GetCategory.Request, GetCategory.Response> getCategoryHandler,
    IHandler<GetCategories.Request, GetCategories.Response> getCategoriesHandler,
    IHandler<CreateCategory.Request, CreateCategory.Response> createCategoryHandler,
    IHandler<UpdateCategory.Request, UpdateCategory.Response> updateCategoryHandler,
    IHandler<DeleteCategory.Request, DeleteCategory.Response> deleteCategoryHandler
) : CategorySvc.CategorySvcBase
{
    public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
    {
        var response = await getCategoriesHandler.Handle(
            request: new GetCategories.Request(),
            context.CancellationToken
        );

        return new GetResponse
        {
            Categories = { response.Categories.Select(ToProto) }
        };

        static GetResponse.Types.Category ToProto(GetCategories.Category category) => new()
        {
            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
        };
    }

    public override async Task<GetByIdResponse> GetById(GetByIdRequest request, ServerCallContext context)
    {
        var response = await getCategoryHandler.Handle(
            request: new GetCategory.Request(request.Id),
            context.CancellationToken
        );

        return new GetByIdResponse
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
        };
    }

    public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
    {
        var response = await createCategoryHandler.Handle(
            request: new CreateCategory.Request(request.Title, request.Description),
            context.CancellationToken
        );

        return new CreateResponse
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
        };
    }

    public override async Task<UpdateResponse> Update(UpdateRequest request, ServerCallContext context)
    {
        var response = await updateCategoryHandler.Handle(
            request: new UpdateCategory.Request(request.Id, request.Title, request.Description),
            context.CancellationToken
        );

        return new UpdateResponse
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
        };
    }

    public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
    {
        var response = await deleteCategoryHandler.Handle(
            request: new DeleteCategory.Request(request.Id),
            context.CancellationToken
        );

        return new DeleteResponse
        {
            Id = response.Id,
            Title = response.Title,
            Description = response.Description,
        };
    }
}