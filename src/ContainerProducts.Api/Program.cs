using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RouteHandler = ContainerProducts.Api.Features.RegisterProduct.RouteHandler;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddScoped<DomainRequestHandler>();
var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] DtoRequest request,
        [FromServices] IValidator<DtoRequest> validator,
        [FromServices] DomainRequestHandler handler
    ) => RouteHandler.Handle(correlationId, request, validator, handler)
);

app.MapGet("/", () => "Hello World!");

app.Run();
