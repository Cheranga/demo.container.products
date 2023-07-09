using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ContainerProducts.Api.Features.UpdateProduct;
using Register = ContainerProducts.Api.Features.RegisterProduct;
using Update = ContainerProducts.Api.Features.UpdateProduct;

var builder = WebApplication.CreateBuilder(args);
builder.Services.UseRegisterProduct().UseUpdateProduct();

var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] Register.DtoRequest request,
        [FromServices] Register.DomainRequestHandler handler
    ) => Register.RouteHandler.Handle(correlationId,request,handler)
);

routeGroupBuilder.MapPut(
    "update",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] Update.DtoRequest request,
        [FromServices] IValidator<Update.DtoRequest> validator,
        [FromServices] Update.DomainRequestHandler handler
    ) => Update.RouteHandler.Handle(correlationId, request, validator, handler)
);

app.MapGet("/", () => "Hello World!");

app.Run();
