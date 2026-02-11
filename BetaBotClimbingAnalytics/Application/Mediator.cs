using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BetaBotClimbingAnalytics.Application;

// Marker interfaces for commands and queries
public interface ICommand<TResult> { }

public interface IQuery<TResult> { }

// Handler contracts
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}

// Simple in-process mediator abstraction
public interface IMediator
{
    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}

// Reflection-based mediator that resolves handlers from DI
public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        if (command is null) throw new ArgumentNullException(nameof(command));

        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        var handleMethod = handlerType.GetMethod(nameof(ICommandHandler<ICommand<TResult>, TResult>.Handle));
        if (handleMethod is null)
        {
            throw new InvalidOperationException($"Handler type '{handlerType.FullName}' does not define a Handle method.");
        }

        // Invoke: Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
        var task = (Task<TResult>)handleMethod.Invoke(handler, new object[] { command, cancellationToken })!;
        return task;
    }

    public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = _serviceProvider.GetRequiredService(handlerType);

        var handleMethod = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.Handle));
        if (handleMethod is null)
        {
            throw new InvalidOperationException($"Handler type '{handlerType.FullName}' does not define a Handle method.");
        }

        // Invoke: Task<TResult> Handle(TQuery query, CancellationToken cancellationToken)
        var task = (Task<TResult>)handleMethod.Invoke(handler, new object[] { query, cancellationToken })!;
        return task;
    }
}

