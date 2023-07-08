﻿namespace ContainerProducts.Api.CustomResponses;

internal static class Factory
{
    public static ProductCreated ProductCreated(
        string correlationId,
        string categoryId,
        string productId
    ) => new(correlationId, categoryId, productId);
}