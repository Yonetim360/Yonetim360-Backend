namespace Yonetim360.API.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/health"))
            {
                await _next(context);
                return;
            }

            var tenantHeader = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(tenantHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant ID header (X-Tenant-ID) is missing.");
                return;
            }

            if (!Guid.TryParse(tenantHeader, out var tenantId) || tenantId == Guid.Empty)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Tenant ID format is invalid.");
                return;
            }

            await _next(context);
        }
    }
}

