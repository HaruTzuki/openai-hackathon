using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using Microsoft.OpenApi.Models;
using Microsoft.ML.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using DeepLearningModel_WebApi;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using DeepLearningModel_WebApi.Models;
using System.Net;
using Swashbuckle.AspNetCore.Annotations;

namespace DeepLearningModel_WebApi.Controllers {
    [ApiController]
    [Route("api/[controller]")]

    public class GetPredictionFromDLMController : ControllerBase {
        private readonly ILogger<GetPredictionFromDLMController> _logger;
        public GetPredictionFromDLMController(ILogger<GetPredictionFromDLMController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// bla bla bla
        /// </summary>
        /// <param name="base64Image"></param>
        /// <returns>List<DLMPrediction></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Ordered Predictions", typeof(List<DLMPrediction>))]
        public async Task< IActionResult> Get([FromBody] Base64Image base64Image) {
            if (base64Image == null || string.IsNullOrEmpty(base64Image.Base64StringImage))  return BadRequest();
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var api = new OpenAI_API.OpenAIAPI(new OpenAI_API.APIAuthentication(apiKey));
            var input = new DeepLearningModel.ModelInput() {
                ImageSource = Convert.FromBase64String(base64Image.Base64StringImage)
            };
            var sortedScoresWithLabel = DeepLearningModel.PredictAllLabels(input);
            //DeepLearningModel.Predict(input);
            if (!sortedScoresWithLabel.Any()) return BadRequest(); 
            List<DLMPrediction> returnList = new List<DLMPrediction>(); 
            foreach (var score in sortedScoresWithLabel) {
                if (score.Value < 0.01) continue;
                DLMPrediction dLMPrediction = new DLMPrediction() {
                    LeafType = score.Key.Split("___").First().Replace('_', ' ').Replace(",", string.Empty),
                    LeafStatus = score.Key.Split("___").Last().Replace('_', ' '),
                    ScorePerc = $"{Math.Round(score.Value * 100, 2)} %",
                    Score = (float)Math.Round(score.Value, 4)
                };
                if (score.Value > 0.25 && !dLMPrediction.LeafStatus.Equals("healthy")) {
                    var result = await api.Completions.CreateCompletionAsync($"Write possible treatments for {dLMPrediction.LeafStatus} on {dLMPrediction.LeafType} plant", temperature: 0.1);
                    dLMPrediction.GPT3ProposedTreatment = result.ToString();
                }else if(dLMPrediction.LeafStatus.Equals("healthy")) dLMPrediction.GPT3ProposedTreatment = "No treatment needed.";
                returnList.Add(dLMPrediction);
            }
            //return Content(JsonConvert.SerializeObject(sortedScoresWithLabel));
            return Content(JsonConvert.SerializeObject(returnList));
        }
    }
}
