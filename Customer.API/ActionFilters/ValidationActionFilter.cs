using Customer.API.ResponseModels;
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

            var errors = new BadRequestObjectResult(context.ModelState).Value;
            var errorResponse = new ErrorResponse(
                statusCode: 400,
                title: "Validation Failed",
                detail: errors,
                type: "ValidationException",
                instance: context.HttpContext.Request.Path.ToString()
            );

            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}