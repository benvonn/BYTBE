
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CornHoleRevamp.Data;

namespace CornHoleRevamp.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public HealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHealth()
        {
            var healthInfo = new
            {
                status = "checking",
                timestamp = DateTime.UtcNow,
                checks = new Dictionary<string, object>()
            };

            bool isHealthy = true;

            healthInfo.checks["web_server"] = new { status = "up" };


            try
            {

                bool canConnect = await _context.Database.CanConnectAsync();

                if (canConnect)
                {
                    healthInfo.checks["database"] = new { status = "connected" };
                }
                else
                {
                    healthInfo.checks["database"] = new { status = "connection_failed" };
                    isHealthy = false;
                }
            }
            catch (Exception ex)
            {

                healthInfo.checks["database"] = new { status = "error", message = ex.Message };
                isHealthy = false;
            }



            var finalStatus = isHealthy ? "healthy" : "unhealthy";

            // Return 200 OK if healthy, 503 Service Unavailable if unhealthy
            return isHealthy
                ? Ok(new { status = finalStatus, timestamp = healthInfo.timestamp, checks = healthInfo.checks })
                : StatusCode(503, new { status = finalStatus, timestamp = healthInfo.timestamp, checks = healthInfo.checks });
        }
    }
}