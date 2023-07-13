using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using ContainerProducts.Api.Features.UpdateProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Register = ContainerProducts.Api.Features.RegisterProduct;
using Update = ContainerProducts.Api.Features.UpdateProduct;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .UseRegisterProduct()
    .UseUpdateProduct();

var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] RegisterProductRequest request,
        [FromServices] RegisterProductRequest.Handler handler
    ) => Register.RouteHandler.Handle(request with { CorrelationId = correlationId }, handler)
);

routeGroupBuilder.MapPut(
    "update",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] UpdateProductRequest request,
        [FromServices] IValidator<UpdateProductRequest> validator,
        [FromServices] UpdateProductRequest.Handler handler
    ) =>
        Update.RouteHandler.Handle(
            request with
            {
                CorrelationId = correlationId
            },
            validator,
            handler
        )
);

app.MapGet("/", () => "Hello World!");

app.Run();
