using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaForce.OpenAI.DeepLearingModelCL {
    public class DLMPredictionModel {
        public string LeafType { get; set; }
        public string LeafStatus { get; set; }
        public string ScorePerc { get; set; }
        public float Score { get; set; }
        public string GPT3ProposedTreatment { get; set; }
    }
}
