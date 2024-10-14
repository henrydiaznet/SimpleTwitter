using SimpleTwitter.Api.Validation;

namespace SimpleTwitter.Api.Filters;

public class ValidationFilter<T>: IEndpointFilter where T: class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var obj = context.Arguments.OfType<T>().FirstOrDefault();
        if (obj == null)
        {
            return Results.BadRequest("Invalid request data type.");
        }

        var validationErrors = Annotations.Validate(obj).ToArray();

        if (validationErrors.Any())
        {
            return Results.BadRequest(validationErrors);
        }

        return await next(context);
    }
}