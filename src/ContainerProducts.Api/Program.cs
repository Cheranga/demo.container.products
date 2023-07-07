using System.ComponentModel.DataAnnotations;
using ContainerProducts.Api.Features.RegisterProduct;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.TypedResults;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var routeGroupBuilder = app.MapGroup("products");
routeGroupBuilder.MapPost(
    "register",
    ([FromHeader] [Required] string correlationId, [FromBody] DtoRequest request) =>
        Created($"products/{request.CategoryId}/{request.ProductId}")
);

app.MapGet("/", () => "Hello World!");

app.Run();
