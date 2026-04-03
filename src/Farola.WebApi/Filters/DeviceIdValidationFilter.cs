using Farola.WebApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Farola.WebApi.Filters
{
    public class DeviceIdValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var attribute = context.ActionDescriptor.EndpointMetadata
                .OfType<RequireDeviceIdAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                var deviceId = context.HttpContext.Request.Headers["X-Device-Id"].FirstOrDefault();
                if (string.IsNullOrEmpty(deviceId))
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        title = "Device ID Required",
                        status = 400,
                        detail = "X-Device-Id header is required for this request"
                    });
                    return;
                }

                if (!Guid.TryParse(deviceId, out _))
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        title = "Invalid Device ID",
                        status = 400,
                        detail = "X-Device-Id must be a valid UUID"
                    });
                    return;
                }

                context.HttpContext.Items["DeviceId"] = deviceId;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
