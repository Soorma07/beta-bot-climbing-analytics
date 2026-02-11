using System.Threading;
using System.Threading.Tasks;
using BetaBotClimbingAnalytics.Application;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BetaBotClimbingAnalytics.Tests.Application;

public class MediatorTests
{
    [Fact]
    public async Task Send_Command_Resolves_Handler_And_Returns_Result()
    {
        var services = new ServiceCollection();
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<ICommandHandler<TestCommand, string>, TestCommandHandler>();

        var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        var result = await mediator.Send(new TestCommand("value"), CancellationToken.None);

        Assert.Equal("Handled: value", result);
    }

    private sealed record TestCommand(string Value) : ICommand<string>;

    private sealed class TestCommandHandler : ICommandHandler<TestCommand, string>
    {
        public Task<string> Handle(TestCommand command, CancellationToken cancellationToken = default)
            => Task.FromResult($"Handled: {command.Value}");
    }
}

