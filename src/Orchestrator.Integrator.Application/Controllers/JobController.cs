using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Integrator.Application.BackgroundJobs;

namespace Orchestrator.Integrator.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var type = typeof(IJob);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsInterface && !p.Name.Equals(nameof(IJob)))
                .Select(x => x.Name);

            return Ok(types);
        }

        [HttpPost("enqueue/{name}")]
        public IActionResult Enqueue([FromServices] IServiceProvider serviceProvider, string name)
        {
            var type = typeof(IJob);
            var backgroundJob = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsInterface && p.Name.StartsWith($"I{name}"))
                .FirstOrDefault();

            if (backgroundJob is null)
            {
                return BadRequest($"{name} job is not registered");
            }

            IJob? backgroundJobFromEnqueue = serviceProvider.GetService(backgroundJob) as IJob;

            if (backgroundJobFromEnqueue is null)
            {
                return BadRequest($"{name} job is not registered");
            }

            BackgroundJob.Enqueue(() => backgroundJobFromEnqueue.RunAsync(null, CancellationToken.None));

            return Ok($"Enqueued background job {name}");

        }
    }
}
