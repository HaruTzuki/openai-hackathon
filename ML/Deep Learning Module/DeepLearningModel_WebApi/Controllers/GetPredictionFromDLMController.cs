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
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "Ordered Predictions", typeof(List<DLMPrediction>))]
        public IActionResult Get([FromBody] Base64Image base64Image) {
            if (base64Image == null || string.IsNullOrEmpty(base64Image.Base64StringImage))  return BadRequest(); 
            var input = new DeepLearningModel.ModelInput() {
                ImageSource = Convert.FromBase64String(base64Image.Base64StringImage)
            };
            var sortedScoresWithLabel = DeepLearningModel.PredictAllLabels(input);
            //DeepLearningModel.Predict(input);
            if (!sortedScoresWithLabel.Any()) return NotFound(); 
            List<DLMPrediction> returnList = new List<DLMPrediction>(); 
            foreach (var score in sortedScoresWithLabel) {
                if (score.Value < 0.01) continue;
                returnList.Add(new DLMPrediction() {
                    LeafType = score.Key.Split("___").First().Replace('_', ' ').Replace(",", string.Empty),
                    LeafStatus = score.Key.Split("___").Last().Replace('_', ' '),
                    ScorePerc = $"{Math.Round(score.Value * 100, 2)} %",
                    Score = (float)Math.Round(score.Value, 4)
                });
            }
            //return Content(JsonConvert.SerializeObject(sortedScoresWithLabel));
            return Content(JsonConvert.SerializeObject(returnList));
        }
    }
}
