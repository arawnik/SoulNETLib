using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.CQRS;

/// <summary>
/// Tests for <see cref="PipelineCommandHandler{TCommand, TResponse}"/> (generic Result variant).
/// </summary>
public sealed class PipelineCommandHandlerGenericTests
{
    [Fact]
    public async Task Handle_NoBehaviors_CallsInnerHandlerDirectly()
    {
        var handler = new TestGenericCommandHandler();
        var pipeline = new PipelineCommandHandler<TestGenericCommand, Guid>(handler, []);

        var result = await pipeline.Handle(
            new TestGenericCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestGenericCommandHandler.ExpectedId, result.Value);
        Assert.Equal(1, handler.CallCount);
    }

    [Fact]
    public async Task Handle_SingleBehavior_ExecutesAroundHandler()
    {
        var handler = new TestGenericCommandHandler();
        var log = new List<string>();
        var behavior = new PassThroughBehavior(log);
        var pipeline = new PipelineCommandHandler<TestGenericCommand, Guid>(handler, [behavior]);

        var result = await pipeline.Handle(
            new TestGenericCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(TestGenericCommandHandler.ExpectedId, result.Value);
        Assert.Equal(1, handler.CallCount);
        Assert.Equal(["before", "after"], log);
    }

    [Fact]
    public async Task Handle_BehaviorShortCircuits_DoesNotCallInnerHandler()
    {
        var handler = new TestGenericCommandHandler();
        var behavior = new ShortCircuitBehavior();
        var pipeline = new PipelineCommandHandler<TestGenericCommand, Guid>(handler, [behavior]);

        var result = await pipeline.Handle(
            new TestGenericCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.Equal("Blocked", result.Error!.Code);
        Assert.Equal(0, handler.CallCount);
    }

    [Fact]
    public async Task Handle_MultipleBehaviors_ExecuteInRegistrationOrder()
    {
        var handler = new TestGenericCommandHandler();
        var log = new List<string>();

        var behaviors = new IPipelineBehavior<TestGenericCommand, Guid>[]
        {
            new OrderedBehavior(log, "A"),
            new OrderedBehavior(log, "B"),
        };

        var pipeline = new PipelineCommandHandler<TestGenericCommand, Guid>(handler, behaviors);
        var result = await pipeline.Handle(
            new TestGenericCommand("test"),
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(["A-before", "B-before", "B-after", "A-after"], log);
    }

    private sealed class PassThroughBehavior(List<string> log)
        : IPipelineBehavior<TestGenericCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(
            TestGenericCommand command,
            PipelineStep<Guid> next,
            CancellationToken cancellationToken
        )
        {
            log.Add("before");
            var result = await next();
            log.Add("after");
            return result;
        }
    }

    private sealed class ShortCircuitBehavior : IPipelineBehavior<TestGenericCommand, Guid>
    {
        public Task<Result<Guid>> Handle(
            TestGenericCommand command,
            PipelineStep<Guid> next,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(Result<Guid>.Failure(new Error("Blocked", "Short-circuited")));
        }
    }

    private sealed class OrderedBehavior(List<string> log, string name)
        : IPipelineBehavior<TestGenericCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(
            TestGenericCommand command,
            PipelineStep<Guid> next,
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
