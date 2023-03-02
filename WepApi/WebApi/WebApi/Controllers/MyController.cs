using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using DeltaForce.OpenAI.DeepLearingModelCL;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class Image : ControllerBase {

        private readonly ILogger<Image> _logger;
        private readonly OpenAIService _openAIService;

        public Image(ILogger<Image> logger, OpenAIService openAIService) {
            _logger = logger;
            this._openAIService = openAIService;
        }

        [HttpPost("ImageAnalyser")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Ordered Predictions", typeof(List<DLMPredictionModel>))]
        public async Task<IActionResult> ImageAnalyser([FromForm] IFormFile file) {
            if (file == null) return BadRequest();


            if (file.Length > 0) {
                //var filePath = Path.GetTempFileName();

                //using (var stream = System.IO.File.Create(filePath)) {
                //    await file.CopyToAsync(stream);
                //}

                string base64Str = string.Empty;
                using (var ms = new MemoryStream()) {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    base64Str = Convert.ToBase64String(fileBytes);
                }

                DLMPredictions dLMPredictions = new DLMPredictions();
                var predictions = dLMPredictions.GetPrediction(base64Str);
                foreach (var prediction in predictions!) {
                    if (prediction.Score > 0.25 && (!prediction.LeafStatus.Equals("healthy") || !prediction.LeafStatus.Equals("Background without leaves"))) {
                        var completionResult = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest() {
                            Prompt = $"Write possible treatments for {prediction.LeafStatus} on {prediction.LeafType} plant",
                            Model = Models.TextDavinciV3,
                            Temperature = 0.1F,
                            MaxTokens = 100
                        });

                        StringBuilder sb = new StringBuilder();
                        if (completionResult.Successful) {
                            foreach (var choice in completionResult.Choices) {
                                prediction.GPT3ProposedTreatment = choice.Text;
                            }
                        } else {
                            if (completionResult.Error == null) {
                                prediction.GPT3ProposedTreatment = "Unknown Error";

                            } else {
                                prediction.GPT3ProposedTreatment = $"{completionResult.Error.Code}: {completionResult.Error.Message}";
                            }
                        }
                    } else if (prediction.LeafStatus.Equals("healthy")) prediction.GPT3ProposedTreatment = "No treatment needed.";
                }
                return Content(JsonConvert.SerializeObject(predictions));
            }

            return BadRequest();
        }

        //[HttpGet]
        //[SwaggerResponse((int)HttpStatusCode.OK, "Ordered Predictions Test", typeof(List<DLMPredictionModel>))]
        //public async Task<IActionResult> GetTest([FromBody] string base64Image) {
        //    if (string.IsNullOrEmpty(base64Image)) return BadRequest();

        //    if (base64Image.Length > 0) {
        //        //var filePath = Path.GetTempFileName();

        //        //using (var stream = System.IO.File.Create(filePath)) {
        //        //    await file.CopyToAsync(stream);
        //        //}

        //        //string base64Str = string.Empty;
        //        //using (var ms = new MemoryStream()) {
        //        //    file.CopyTo(ms);
        //        //    var fileBytes = ms.ToArray();
        //        //    base64Str = Convert.ToBase64String(fileBytes);
        //        //}

        //        var gpt3 = new OpenAIService(new OpenAiOptions() {
        //            ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")!
        //        });

        //        DLMPredictions dLMPredictions = new DLMPredictions();
        //        var predictions = dLMPredictions.GetPrediction(base64Image);
        //        foreach (var prediction in predictions!) {
        //            if (prediction.Score > 0.25 && !prediction.LeafStatus.Equals("healthy")) {
        //                var completionResult = await gpt3.Completions.CreateCompletion(new CompletionCreateRequest() {
        //                    Prompt = $"Write possible treatments for {prediction.LeafStatus} on {prediction.LeafType} plant",
        //                    Model = Models.TextDavinciV3,
        //                    Temperature = 0.1F,
        //                    MaxTokens = 100
        //                });

        //                StringBuilder sb = new StringBuilder();
        //                if (completionResult.Successful) {
        //                    foreach (var choice in completionResult.Choices) {
        //                        prediction.GPT3ProposedTreatment = choice.Text;
        //                    }
        //                } else {
        //                    if (completionResult.Error == null) {
        //                        prediction.GPT3ProposedTreatment = "Unknown Error";

        //                    } else {
        //                        prediction.GPT3ProposedTreatment = $"{completionResult.Error.Code}: {completionResult.Error.Message}";
        //                    }
        //                }
        //            } else if (prediction.LeafStatus.Equals("healthy")) prediction.GPT3ProposedTreatment = "No treatment needed.";
        //        }
        //        return Content(JsonConvert.SerializeObject(predictions));
        //    }

        //    return BadRequest();
        //}
    }
}