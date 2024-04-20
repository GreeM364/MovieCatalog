using MovieCatalog.Exceptions;

namespace MovieCatalog.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException exception)
            {
                HandleStatus400Exception(context, exception, logger);
            }
            catch (NotFoundException exception)
            {
                HandleStatus404Exception(context, exception, logger);
            }
            catch (Exception exception)
            {
                HandleStatus500Exception(context, exception, logger);
            }
        }

        private void HandleStatus400Exception(HttpContext context, Exception exception, ILogger logger)
        {
            logger.LogError(exception, "Bad request occurred: {Message}", exception.Message);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
        }

        private void HandleStatus404Exception(HttpContext context, Exception exception, ILogger logger)
        {
            logger.LogInformation(exception, "Resource not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }

        private void HandleStatus500Exception(HttpContext context, Exception exception, ILogger logger)
        {
            logger.LogError(exception, "An exception was thrown as a result of the request");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
