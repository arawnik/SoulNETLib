using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Application.Abstractions.Validation;
using SoulNETLib.Clean.Application.Behaviors;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.Behaviors;

/// <summary>
/// Tests for <see cref="ValidationBehavior{TCommand}"/> (non-generic Result variant).
/// </summary>
public sealed class ValidationBehaviorTests
{
    private sealed record TestCommand(string Name, string Email) : ICommand;

    private bool _nextCalled;

    private PipelineStep CreateNext()
    {
        _nextCalled = false;
        return () =>
        {
            _nextCalled = true;
            return Task.FromResult(Result.Success());
        };
    }

    [Fact]
    public async Task Handle_NoValidators_CallsNext()
    {
        var behavior = new ValidationBehavior<TestCommand>([]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("a", "b"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.True(_nextCalled);
    }

    [Fact]
    public async Task Handle_ValidatorReturnsNoErrors_CallsNext()
    {
        var validator = new AlwaysValidValidator();
        var behavior = new ValidationBehavior<TestCommand>([validator]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("a", "b"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsSuccess);
        Assert.True(_nextCalled);
    }

    [Fact]
    public async Task Handle_ValidatorReturnsErrors_ShortCircuitsWithValidationResult()
    {
        var validator = new FailingValidator(
            [new Error("Field.Required", "Name is required", "name")]
        );
        var behavior = new ValidationBehavior<TestCommand>([validator]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("", "b"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.False(_nextCalled);
        var validation = Assert.IsType<IValidationResult>(result, exactMatch: false);
        var error = Assert.Single(validation.Errors);
        Assert.Equal("Field.Required", error.Code);
        Assert.Equal("name", error.Field);
    }

    [Fact]
    public async Task Handle_MultipleValidators_AggregatesAllErrors()
    {
        var validator1 = new FailingValidator([new Error("V1", "Error from validator 1")]);
        var validator2 = new FailingValidator(
            [
                new Error("V2a", "First error from validator 2"),
                new Error("V2b", "Second error from validator 2"),
            ]
        );
        var behavior = new ValidationBehavior<TestCommand>([validator1, validator2]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("", ""),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.False(_nextCalled);
        var validation = Assert.IsType<IValidationResult>(result, exactMatch: false);
        var errors = validation.Errors.ToArray();
        Assert.Equal(3, errors.Length);
        Assert.Contains(errors, e => e.Code == "V1");
        Assert.Contains(errors, e => e.Code == "V2a");
        Assert.Contains(errors, e => e.Code == "V2b");
    }

    [Fact]
    public async Task Handle_MultipleValidators_OneValid_OneInvalid_ReturnsOnlyErrors()
    {
        var validValidator = new AlwaysValidValidator();
        var failingValidator = new FailingValidator([new Error("Invalid", "Fails")]);
        var behavior = new ValidationBehavior<TestCommand>([validValidator, failingValidator]);
        var next = CreateNext();

        var result = await behavior.Handle(
            new TestCommand("a", "b"),
            next,
            TestContext.Current.CancellationToken
        );

        Assert.True(result.IsFailure);
        Assert.False(_nextCalled);
        var validation = Assert.IsType<IValidationResult>(result, exactMatch: false);
        Assert.Single(validation.Errors);
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
