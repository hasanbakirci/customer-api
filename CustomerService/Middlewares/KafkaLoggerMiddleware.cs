using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.ServerResponse;
using CustomerService.Clients.MessageQueueClients;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Middlewares
{
    public class KafkaLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KafkaClient _kafkaClient;

        public KafkaLoggerMiddleware(RequestDelegate next, KafkaClient kafkaClient)
        {
            _next = next;
            _kafkaClient = kafkaClient;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                await _next(context);
                watch.Stop();
                
                await _kafkaClient.Publish<KafkaLogMessage>("Logs", new KafkaLogMessage
                {
                    Path = context.Request.Path,
                    HttpMethod = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    ElapsedTime = watch.Elapsed.TotalMilliseconds
                });
            }
            catch (Exception e)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var resp = new ErrorResponse(ResponseStatus.Internal,e.Message);
                var result = JsonSerializer.Serialize(resp);

                await response.WriteAsync(result);

            }
        }
    }

    public class KafkaLogMessage
    {
        public string Path { get; set; }
        public string HttpMethod { get; set; }
        public int StatusCode { get; set; }
        public double ElapsedTime { get; set; }
    }
    
}