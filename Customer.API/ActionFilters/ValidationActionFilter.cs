using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Customer.API.ActionFilters;

public class ValidationActionFilter(IServiceProvider serviceProvider) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null) continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

            if (serviceProvider.GetService(validatorType) is IValidator validator)
            {
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = validator.Validate(validationContext);
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }
                }
            }
        }
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}