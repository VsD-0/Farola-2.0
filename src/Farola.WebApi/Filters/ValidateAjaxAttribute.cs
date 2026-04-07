using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Farola.WebApi.Filters
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            if (!request.Headers.ContainsKey("X-Requested-With") ||
                request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                context.Result = new BadRequestObjectResult(new { error = "Invalid request" });
            }
        }
    }
}
