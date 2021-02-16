using System;
using System.Collections.Generic;
using System.Text;
using LineDC.Messaging.Webhooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mr.LINEbot
{
    class WebhookEndpoint
    {
        private readonly IWebhookApplication _app;

        public WebhookEndpoint(IWebhookApplication app)
        {
            _app = app;
        }

        [FunctionName(nameof(WebhookEndpoint))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                var xLineSignature = req.Headers["x-line-signature"];

                log.LogTrace($"RequestBody: {body}");
                await _app.RunAsync(xLineSignature, body);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                log.LogError(ex.StackTrace);
            }

            return new OkResult();
        }
    }
}
