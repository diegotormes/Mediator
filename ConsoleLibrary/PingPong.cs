using Mediator;

namespace ConsoleLibrary;

public sealed record PingLibrary(Guid Id) : IRequest<PongLibrary>;

public sealed record PongLibrary(Guid Id);

public sealed class GenericLoggerLibraryHandler<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken
    )
    {
        Console.WriteLine("1) Running logger handler");
        try
        {
            var response = await next(message, cancellationToken);
            Console.WriteLine("5) No error!");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}

public sealed class PingHandler : IRequestHandler<PingLibrary, PongLibrary>
{
    public ValueTask<PongLibrary> Handle(PingLibrary request, CancellationToken cancellationToken)
    {
        Console.WriteLine("4) Returning pong!");
        return new ValueTask<PongLibrary>(new PongLibrary(request.Id));
    }
}
