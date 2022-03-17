using Serilog.Context;

namespace Orchestrator.Integrator.Application.Middlewares
{
    public class RequestSerilLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestSerilLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("UserName", context?.User?.Identity?.Name ?? "anonymous"))
            {
                return _next.Invoke(context);
            }
        }
    }
}
