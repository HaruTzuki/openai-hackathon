using DeltaForce.OpenAI.App.Mobile.Libraries;

namespace DeltaForce.OpenAI.App.Mobile;

public partial class Result : ContentPage
{
	private readonly OpenAIResult _openAIResult;

	public Result()
	{
		
	}

    public Result(OpenAIResult openAIResult)
    {
		InitializeComponent();
		_openAIResult = openAIResult;

		LblLeafStatus.Text = $"Leaf Status: {_openAIResult.LeafStatus}";
		LblLeafType.Text = $"Leaf Type: {_openAIResult.LeafType}";
		LblScore.Text = $"Score: {_openAIResult.ScorePerc}";
		LblSolution.Text = $"Solution: \n {_openAIResult.Solution}";

		ShowImageToUI().GetAwaiter();
	}

	private async Task ShowImageToUI()
	{
		var stream = await _openAIResult.FileResult.OpenReadAsync();
		var bytes = new byte[stream.Length];
		await stream.ReadAsync(bytes);

		LeafPicture.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
	}
}