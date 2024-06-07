using FlexZon.CategoryService.Api.Interceptors;
using FlexZon.CategoryService.Api.Services;
using FlexZon.CategoryService.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLogic();

builder.Services
    .AddGrpc(options =>
    {
        options.Interceptors.Add<ExceptionInterceptor>();
    })
    .AddJsonTranscoding();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<CategoryServiceImplementation>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
