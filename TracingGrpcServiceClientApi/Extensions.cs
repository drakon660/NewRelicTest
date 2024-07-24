using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TracingGrpcServiceClientApi;

public static class Extensions
{
    public static IHttpClientBuilder AddGrpcCorrelationIdForwarding(this IHttpClientBuilder builder)
    {
        builder.Services.TryAddTransient<TestInterceptor>();
        builder.AddInterceptor<TestInterceptor>();
        return builder;
    }
}