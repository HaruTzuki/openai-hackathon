using Newtonsoft.Json;

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
		[JsonIgnore]
		public FileResult FileResult { get; set; }
	}
}
