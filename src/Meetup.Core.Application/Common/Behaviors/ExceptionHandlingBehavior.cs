namespace Meetup.Core.Application.Common.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var response = await next();

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            var response = new TResponse()
            {
                Errors = { new ExceptionalError(ex) }
            };

            return response;
        }
    }
}
