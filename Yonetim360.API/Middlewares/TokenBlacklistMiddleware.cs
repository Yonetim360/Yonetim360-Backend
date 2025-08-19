using Yonetim360.DataAccess.Data;

namespace Yonetim360.API.Middlewares
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();

                var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();

                // Access token, blacklist'te var mı?
                var exists = dbContext.TokenBlacklists.Any(b => b.Token == token);
                if (exists)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token geçersiz (blacklist).");
                    return;
                }
            }

            await _next(context);
        }
    }
}
