using System.Text.Json;

namespace ContainerProducts.Api.Extensions;

public static class RequestExtensions
{
    public static Func<string> GetMessageContent<T>(this T data) =>
        () =>
            JsonSerializer.Serialize(
                data,
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
            );
}
