using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ContainerProducts.Api.Features.UpdateProduct;
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
    ) => Register.RouteHandler.Handle(correlationId, request, handler)
);

routeGroupBuilder.MapPut(
    "update",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] UpdateProductRequest request,
        [FromServices] IValidator<UpdateProductRequest> validator,
        [FromServices] UpdateProductRequest.Handler handler
    ) => Update.RouteHandler.Handle(correlationId, request, validator, handler)
);

app.MapGet("/", () => "Hello World!");

app.Run();
