using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text;

namespace WebApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase {

        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger) {
            _logger = logger;
        }

        [HttpPost(Name = "Post")]
        public async Task<IActionResult> OnPostDoAsync([FromForm]IFormFile file) {
            if (file.Length > 0) {
                var filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath)) {
                    await file.CopyToAsync(stream);
                }
            }

            // john job

            var gpt3 = new OpenAIService(new OpenAiOptions() {
                ApiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ChatGPTApiKey"]
            });

            var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest() {
                Prompt = "John Response",
                Model = Models.TextDavinciV2,
                Temperature = 0.5F,
                MaxTokens = 100
            });

            StringBuilder sb = new StringBuilder();
            if (completionResult.Successful) {
                foreach (var choice in completionResult.Choices) {
                    sb.Append(choice.Text + Environment.NewLine);
                }
            } else {
                if (completionResult.Error == null) {
                    sb.Append("Unknown Error");

                } else {
                    sb.Append($"{completionResult.Error.Code}: {completionResult.Error.Message}");
                }
            }

            return Ok(sb.ToString());
        }


        //[HttpPost(Name = "ChatGPTTest")]
        //public async Task<IActionResult> ChatGPTTest() {
        //    var gpt3 = new OpenAIService(new OpenAiOptions() {
        //        ApiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ChatGPTApiKey"]
        //    });

        //    var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest() {
        //        Prompt = "What is the meaning of life?",
        //        Model = Models.TextDavinciV2,
        //        Temperature = 0.5F,
        //        MaxTokens = 100
        //    });

        //    if (completionResult.Successful) {
        //        foreach (var choice in completionResult.Choices) {
        //            Console.WriteLine(choice.Text);
        //        }
        //    } else {
        //        if (completionResult.Error == null) {
        //            throw new Exception("Unknown Error");
        //        }
        //        Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
        //    }
        //    return Ok();
        //}
    }
}