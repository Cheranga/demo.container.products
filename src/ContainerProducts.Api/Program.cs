using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped<DomainRequestHandler>();
var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    async Task<Results<ValidationProblem, Created>> (
        [FromHeader] [Required] string correlationId,
        [FromBody] DtoRequest request,
        [FromServices] IValidator<DtoRequest> validator,
            [FromServices] DomainRequestHandler handler
    ) =>
    {
        var token = new CancellationToken();
        request.CorrelationId = correlationId;

        var validationResult = await validator.ValidateAsync(request, token);
        if (!validationResult.IsValid)
            return ValidationProblem(
                validationResult.ToDictionary(),
                type: "Invalid Request",
                title: "Invalid Register Product Request"
            );

        await handler.RegisterProductAsync(request.ToDomainRequest());
        return Created($"products/{request.CategoryId}/{request.ProductId}");
    }
);

app.MapGet("/", () => "Hello World!");

app.Run();