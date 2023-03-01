
using DeltaForce_OpenAI_DeepLearingModelCL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeltaForce.OpenAI.DeepLearingModelCL {
    public  class DLMPredictions {
        public List<DLMPredictionModel>? GetPrediction(string base64Image) {
            if (base64Image == null || string.IsNullOrEmpty(base64Image)) return null;
            List<DLMPredictionModel> returnList = new List<DLMPredictionModel>();
            var input = new DLModel.ModelInput() {
                ImageSource = Convert.FromBase64String(base64Image)
            };
            var sortedScoresWithLabel = DLModel.PredictAllLabels(input);
            if (!sortedScoresWithLabel.Any()) return null;
            foreach (var score in sortedScoresWithLabel) {
                if (score.Value < 0.25) continue;
                DLMPredictionModel dLMPrediction = new DLMPredictionModel() {
                    LeafType = score.Key.Split("___").First().Replace('_', ' ').Replace(",", string.Empty),
                    LeafStatus = score.Key.Split("___").Last().Replace('_', ' '),
                    ScorePerc = $"{Math.Round(score.Value * 100, 2)} %",
                    Score = (float)Math.Round(score.Value, 4)
                };

                returnList.Add(dLMPrediction);
            }
            return returnList;
        }

    }
}
