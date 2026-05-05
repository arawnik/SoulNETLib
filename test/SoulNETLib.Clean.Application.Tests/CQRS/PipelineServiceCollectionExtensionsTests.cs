using Microsoft.Extensions.DependencyInjection;
using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.DependencyInjection;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.CQRS;

/// <summary>
/// Integration tests verifying that <see cref="PipelineServiceCollectionExtensions"/>
/// correctly registers handlers with pipeline wrappers.
/// </summary>
public sealed class PipelineServiceCollectionExtensionsTests
{
    [Fact]
    public async Task AddCommandHandler_ResolvesHandlerWithPipelineBehaviors()
    {
        var log = new List<string>();
        var services = new ServiceCollection();
        services.AddSingleton(log);
        services.AddScoped<IPipelineBehavior<TestCommand>, LoggingBehavior>();
        services.AddCommandHandler<TestCommand, TestCommandHandler>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TestCommand>>();
        var result = await handler.Handle(
            new TestCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(["behavior"], log);
    }

    [Fact]
    public async Task AddCommandHandler_Generic_ResolvesHandlerWithoutBehaviors()
    {
        var services = new ServiceCollection();
        services.AddCommandHandler<TestGenericCommand, Guid, TestGenericCommandHandler>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<
            ICommandHandler<TestGenericCommand, Guid>
        >();
        var result = await handler.Handle(
            new TestGenericCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestGenericCommandHandler.ExpectedId, result.Value);
    }

    [Fact]
    public async Task AddQueryHandler_ResolvesHandlerThroughPipeline()
    {
        var services = new ServiceCollection();
        services.AddQueryHandler<TestQuery, IReadOnlyList<string>, TestQueryHandler>();

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService<
            IQueryHandler<TestQuery, IReadOnlyList<string>>
        >();
        var result = await handler.Handle(
            new TestQuery("filter"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestQueryHandler.ExpectedData, result.Value);
    }

    private sealed class LoggingBehavior(List<string> log) : IPipelineBehavior<TestCommand>
    {
        public async Task<Result> Handle(
            TestCommand command,
            PipelineStep next,
            CancellationToken cancellationToken
        )
        {
            log.Add("behavior");
            return await next();
        }
    }
}
