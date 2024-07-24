using Microsoft.AspNetCore.Mvc;
using TracingGrpcService;

namespace TracingGrpcServiceClientApi.Controllers;

[ApiController]
[Route("tracing")]
public class TracingController : ControllerBase
{
    private readonly ILogger<TracingController> _logger;
    private readonly Greeter.GreeterClient _greeterClient;

    public TracingController(ILogger<TracingController> logger, Greeter.GreeterClient greeterClient)
    {
        _logger = logger;
        _greeterClient = greeterClient;
    }

    [HttpPost]
    public async Task<IActionResult> Make(string message)
    {
        var reply = await _greeterClient.SayHelloAsync(new HelloRequest()
        {
            Name = message
        });

        return Ok(reply.Message); // This will return HTTP 200 OK
    }
}