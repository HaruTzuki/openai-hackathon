using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaForce.OpenAI.App.Mobile.Libraries
{
    public class OpenAIResult
    {
        public string LeafType { get; set; }
        public string LeafStatus { get; set; }
        public string ScorePerc { get; set; }
        public double Score { get; set; }
        [JsonProperty("GPT3ProposedTreatment")]
        public string Solution { get; set; }
    }
}
