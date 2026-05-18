using DrinkStore.API.Exceptions;

namespace DrinkStore.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var status = ex switch
                {
                    CustomValidationException => 400,
                    _ => 500
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = status;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = status == 500 ? "Internal server error" : ex.Message,
                });
            }
        }
    }
}
