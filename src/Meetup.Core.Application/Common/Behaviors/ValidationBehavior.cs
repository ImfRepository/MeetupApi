using Meetup.Core.Domain.Errors;

namespace Meetup.Core.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(e => e.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        if (!failures.Any())
            return await next();

        var response = new TResponse();

        foreach (var failure in failures)
        {
            response.Reasons.Add(new ValidationError(failure));
        }

        return response;
    }
}