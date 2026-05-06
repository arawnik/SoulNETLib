using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Application.Behaviors;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.Behaviors;

/// <summary>
/// Tests for <see cref="ValidationBehavior{TCommand, TResponse}"/> (generic Result variant).
/// </summary>
public sealed class ValidationBehaviorGenericTests
{
    private sealed record TestCommand(string Name) : ICommand<Guid>;

    private static readonly Guid ExpectedId = Guid.NewGuid();

    private bool _nextCalled;

    private PipelineStep<Guid> CreateNext()
    {
        _nextCalled = false;
        return () =>
        {
            _nextCalled = true;
            return Task.FromResult(Result<Guid>.Success(ExpectedId));
        };
    }

    [Fact]
    public async Task Handle_NoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<TestCommand, Guid>([]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("a"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(ExpectedId, result.Value);
        Assert.True(_nextCalled);
    }

    [Fact]
    public async Task Handle_ValidatorReturnsNoErrors_CallsNext()
    {
        var validator = new AlwaysValidValidator();
        var behavior = new ValidationBehavior<TestCommand, Guid>([validator]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("a"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(ExpectedId, result.Value);
        Assert.True(_nextCalled);
    }

    [Fact]
    public async Task Handle_ValidatorReturnsErrors_ShortCircuitsWithValidationResult()
    {
        var validator = new FailingValidator(
            [new Error("Field.Required", "Name is required", "name")]
        );
        var behavior = new ValidationBehavior<TestCommand, Guid>([validator]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand(""),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.False(_nextCalled);
        var validation = Assert.IsType<IValidationResult>(result, exactMatch: false);
        var error = Assert.Single(validation.Errors);
        Assert.Equal("Field.Required", error.Code);
    }

    [Fact]
    public async Task Handle_MultipleValidators_AggregatesAllErrors()
    {
        var validator1 = new FailingValidator([new Error("V1", "Error 1")]);
        var validator2 = new FailingValidator([new Error("V2", "Error 2")]);
        var behavior = new ValidationBehavior<TestCommand, Guid>([validator1, validator2]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand(""),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.False(_nextCalled);
        var validation = Assert.IsType<IValidationResult>(result, exactMatch: false);
        var errors = validation.Errors.ToArray();
        Assert.Equal(2, errors.Length);
        Assert.Contains(errors, e => e.Code == "V1");
        Assert.Contains(errors, e => e.Code == "V2");
    }

    private sealed class AlwaysValidValidator : ICommandValidator<TestCommand>
    {
        public Task<Error[]> ValidateAsync(
            TestCommand command,
            CancellationToken cancellationToken
        ) => Task.FromResult(Array.Empty<Error>());
    }

    private sealed class FailingValidator(Error[] errors) : ICommandValidator<TestCommand>
    {
        public Task<Error[]> ValidateAsync(
            TestCommand command,
            CancellationToken cancellationToken
        ) => Task.FromResult(errors);
    }
}
