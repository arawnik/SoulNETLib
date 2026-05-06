using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.CQRS;

/// <summary>
/// Tests for <see cref="PipelineQueryHandler{TQuery, TResponse}"/>.
/// </summary>
public sealed class PipelineQueryHandlerTests
{
    [Fact]
    public async Task Handle_NoBehaviors_CallsInnerHandlerDirectly()
    {
        var handler = new TestQueryHandler();
        var pipeline = new PipelineQueryHandler<TestQuery, IReadOnlyList<string>>(handler, []);

        var result = await pipeline.Handle(
            new TestQuery("all"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestQueryHandler.ExpectedData, result.Value);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task Handle_SingleBehavior_ExecutesAroundHandler()
    {
        var handler = new TestQueryHandler();
        var log = new List<string>();
        var behavior = new PassThroughBehavior(log);
        var pipeline = new PipelineQueryHandler<TestQuery, IReadOnlyList<string>>(
            handler,
            [behavior]
        );

        var result = await pipeline.Handle(
            new TestQuery("all"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestQueryHandler.ExpectedData, result.Value);
        Assert.Equal(1, handler.CallCount);
        Assert.Equal(["before", "after"], log);
    }

    [Fact]
    public async Task Handle_BehaviorShortCircuits_DoesNotCallInnerHandler()
    {
        var handler = new TestQueryHandler();
        var behavior = new ShortCircuitBehavior();
        var pipeline = new PipelineQueryHandler<TestQuery, IReadOnlyList<string>>(
            handler,
            [behavior]
        );

        var result = await pipeline.Handle(
            new TestQuery("all"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.Equal("Blocked", result.Error!.Code);
        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task Handle_MultipleBehaviors_ExecuteInRegistrationOrder()
    {
        var handler = new TestQueryHandler();
        var log = new List<string>();

        var behaviors = new IQueryPipelineBehavior<TestQuery, IReadOnlyList<string>>[]
        {
            new OrderedBehavior(log, "A"),
            new OrderedBehavior(log, "B"),
        };

        var pipeline = new PipelineQueryHandler<TestQuery, IReadOnlyList<string>>(
            handler,
            behaviors
        );
        await pipeline.Handle(new TestQuery("all"), TestContext.Current.CancellationToken);

        Assert.Equal(["A-before", "B-before", "B-after", "A-after"], log);
    }

    private sealed class PassThroughBehavior(List<string> log)
        : IQueryPipelineBehavior<TestQuery, IReadOnlyList<string>>
    {
        public async Task<Result<IReadOnlyList<string>>> Handle(
            TestQuery query,
            PipelineStep<IReadOnlyList<string>> next,
            CancellationToken cancellationToken
        )
        {
            log.Add("before");
            var result = await next();
            log.Add("after");
            return result;
        }
    }

    private sealed class ShortCircuitBehavior
        : IQueryPipelineBehavior<TestQuery, IReadOnlyList<string>>
    {
        public Task<Result<IReadOnlyList<string>>> Handle(
            TestQuery query,
            PipelineStep<IReadOnlyList<string>> next,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                Result<IReadOnlyList<string>>.Failure(new Error("Blocked", "Short-circuited"))
            );
        }
    }

    private sealed class OrderedBehavior(List<string> log, string name)
        : IQueryPipelineBehavior<TestQuery, IReadOnlyList<string>>
    {
        public async Task<Result<IReadOnlyList<string>>> Handle(
            TestQuery query,
            PipelineStep<IReadOnlyList<string>> next,
            CancellationToken cancellationToken
        )
        {
            log.Add($"{name}-before");
            var result = await next();
            log.Add($"{name}-after");
            return result;
        }
    }
}
