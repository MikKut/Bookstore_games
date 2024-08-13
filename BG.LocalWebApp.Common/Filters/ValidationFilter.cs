using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BG.LocalWebApp.Common.Filters
{
    /// <summary>
    /// An action filter that performs validation on the incoming model before the action method executes.
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        /// <summary>
        /// Called before the action method executes. Validates the model state and logs any validation errors.
        /// </summary>
        /// <param name="context">The context for the action executing.</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("ValidationFilter executing...");

            // Retrieve validation errors from the model state.
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToList();

            // Log each validation error.
            foreach (var error in errors)
            {
                Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Errors.Select(e => e.ErrorMessage))}");
            }

            // If the model state is not valid, set the result to a BadRequestObjectResult.
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        /// <summary>
        /// Called after the action method executes.
        /// This method does not perform any operations in the current implementation.
        /// </summary>
        /// <param name="context">The context for the action executed.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // This method is intentionally left empty.
        }
    }

}
