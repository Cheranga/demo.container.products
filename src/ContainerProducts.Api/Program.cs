using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using ContainerProducts.Api.Features.UpdatePrice;
using FluentValidation;
using Infrastructure.Messaging.Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Register = ContainerProducts.Api.Features.RegisterProduct;
using Update = ContainerProducts.Api.Features.UpdatePrice;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    // TODO: make it feature toggled
    .UseInMemoryMessaging()
    .UseRegisterProduct()
    .UseUpdateProduct();

var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] RegisterProductRequest request,
        [FromServices] IValidator<RegisterProductRequest> validator,
        [FromServices] IMessagePublisher publisher
    ) =>
        Register.RouteHandler.Handle(
            request with
            {
                CorrelationId = correlationId
            },
            validator,
            publisher
        )
);

routeGroupBuilder.MapPut(
    "update",
    (
        [FromHeader] [Required] string correlationId,
        [FromBody] UpdateProductPriceRequest request,
        [FromServices] IValidator<UpdateProductPriceRequest> validator,
        [FromServices] IMessagePublisher publisher
    ) =>
        Update.RouteHandler.Handle(
            request with
            {
                CorrelationId = correlationId
            },
            validator,
            publisher
        )
);

app.MapGet("/", () => "Hello World!");

app.Run();
