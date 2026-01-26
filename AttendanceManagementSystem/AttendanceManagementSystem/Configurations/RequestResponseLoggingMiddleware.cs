using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementSystem.API.Configurations;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger
    )
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // logging the client request
        _logger.LogInformation(await FormatRequest(context.Request));

        //storing response data to use later if needed
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            //replaces the default response stream with a temporary in-memory stream
            //the response is written directly to the network stream, and once written,  can’t read it back to log it
            // so, temporarily replace it with a MemoryStream, so it can capture what the controller writes.
            context.Response.Body = responseBody;

            // WAIT FOR RESPONSE
            await _next(context);

            // LOG RESPONSE
            _logger.LogInformation(await FormatResponse(context.Response));
            if (context.Response.StatusCode != 204)
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    /// <summary>
    /// Returns the destination path in the Http request, the query if any, and the first 100 characters in the body if any, as a string to log.
    /// </summary>
    /// <param name="request">Http request instance</param>
    /// <returns></returns>
    private static async Task<string> FormatRequest(HttpRequest request)
    {
        try
        {
            request.EnableBuffering();
            var body = request.Body;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{request.Scheme}://{request.Host}{request.Path}"
                + (
                    !string.IsNullOrEmpty(request.QueryString.ToString())
                        ? $" - QUERY: {request.QueryString}"
                        : string.Empty
                )
                + (
                    !string.IsNullOrEmpty(bodyAsText)
                        ? (
                            bodyAsText.Length < 100
                                ? $" - BODY: {bodyAsText}"
                                : $" - BODY: {bodyAsText[..98]}.."
                        )
                        : string.Empty
                );
        }
        catch (Exception ex)
        {
            return $"ERROR: {ex.Message} - METHOD: RequestResponseLoggingMiddleware.FormatRequest()";
        }
    }

    /// <summary>
    /// Returns the status code from the http response and the first 100 characters in the body if any, as a string to log.
    /// </summary>
    /// <param name="response">Outgoing side of Http request instance</param>
    /// <returns></returns>
    private static async Task<string> FormatResponse(HttpResponse response)
    {
        try
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"Response {response.StatusCode}"
                + (
                    !string.IsNullOrEmpty(text)
                        ? (text.Length < 100 ? $" - BODY: {text}" : $" - BODY: {text[..98]}..")
                        : string.Empty
                );
        }
        catch (Exception ex)
        {
            return $"ERROR: {ex.Message} - METHOD: RequestResponseLoggingMiddleware.FormatResponse()";
        }
    }
}
