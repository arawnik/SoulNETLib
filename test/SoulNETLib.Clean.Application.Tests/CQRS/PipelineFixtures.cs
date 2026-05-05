using SoulNETLib.Clean.Application.Abstractions.CQRS;
using SoulNETLib.Clean.Domain;

namespace SoulNETLib.Clean.Application.Tests.CQRS;

internal sealed record TestCommand(string Value) : ICommand;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    public int CallCount { get; private set; }

    public Task<Result> Handle(TestCommand command, CancellationToken cancellationToken)
    {
        CallCount++;
        return Task.FromResult(Result.Success());
    }
}

internal sealed record TestGenericCommand(string Value) : ICommand<Guid>;

internal sealed class TestGenericCommandHandler : ICommandHandler<TestGenericCommand, Guid>
{
    public static readonly Guid ExpectedId = Guid.NewGuid();

    public int CallCount { get; private set; }

    public Task<Result<Guid>> Handle(
        TestGenericCommand command,
        CancellationToken cancellationToken
    )
    {
        CallCount++;
        return Task.FromResult(Result<Guid>.Success(ExpectedId));
    }
}

internal sealed record TestQuery(string Filter) : IQuery<IReadOnlyList<string>>;

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, IReadOnlyList<string>>
{
    public static readonly IReadOnlyList<string> ExpectedData = ["item1", "item2"];

    public int CallCount { get; private set; }

    public Task<Result<IReadOnlyList<string>>> Handle(
        TestQuery query,
        CancellationToken cancellationToken
    )
    {
        CallCount++;
        return Task.FromResult(Result<IReadOnlyList<string>>.Success(ExpectedData));
    }
}
