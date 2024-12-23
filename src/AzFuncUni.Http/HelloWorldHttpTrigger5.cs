using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Web;

namespace AzFuncUni.Http
{
    public class HelloWorldHttpTrigger5
    {
        private readonly ILogger<HelloWorldHttpTrigger> _logger;

        public HelloWorldHttpTrigger5(ILogger<HelloWorldHttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(HelloWorldHttpTrigger5))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var person = await req.ReadFromJsonAsync<Person>();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            if (string.IsNullOrEmpty(person.Name))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync($"Please provide a value for the name in JSON format in the body.");
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
                await response.WriteStringAsync($"Hello, {person.Name}");
            }

            return response;
        }
    }
}
