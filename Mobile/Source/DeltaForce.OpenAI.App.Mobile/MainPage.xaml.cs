using DeltaForce.OpenAI.App.Mobile.Libraries;
using Microsoft.Maui.Controls.Platform;
using Newtonsoft.Json;
using System.Net;

namespace DeltaForce.OpenAI.App.Mobile
{
	public partial class MainPage : ContentPage
	{
		private readonly HttpClient _httpClient;
		private FileResult fileResult;

		public MainPage(IHttpClientFactory httpClientFactory)
		{
			InitializeComponent();
			_httpClient = httpClientFactory.CreateClient("TempAPI");
		}

		/// <summary>
		/// Camera Capture
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OnTakePhotoClicked(object sender, EventArgs e)
		{
			if (MediaPicker.Default.IsCaptureSupported)
			{
				fileResult = await MediaPicker.Default.CapturePhotoAsync();

				if (fileResult != null)
				{
					await ShowImageToUI(fileResult);

				}
			}
		}

		/// <summary>
		/// File Storage
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void FromStorageClicked(object sender, EventArgs e)
		{
			fileResult = await MediaPicker.Default.PickPhotoAsync();

			if (fileResult != null)
			{
				await ShowImageToUI(fileResult);
			}
		}

		/// <summary>
		/// Upload Button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private async void BtnUploadThePhoto_Clicked(object sender, EventArgs e)
		{
			if (fileResult is null)
			{
				await DisplayAlert("Error...", "You have not choose a picture.", "OK");
				return;
			}
			UploadPhoto();
		}

		/// <summary>
		/// Generic Mehtod that shows Image to UI
		/// </summary>
		/// <param name="fileResult"></param>
		/// <returns></returns>
		private async Task ShowImageToUI(FileResult fileResult)
		{
			var stream = await fileResult.OpenReadAsync();
			var bytes = new byte[stream.Length];
			await stream.ReadAsync(bytes);

			UploadedOrSelectedImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
		}

		/// <summary>
		/// API Call
		/// </summary>
		private async void UploadPhoto()
		{

			if (_httpClient != null)
			{
				ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;
				using var content = new MultipartFormDataContent();
				using var sourceStream = await fileResult.OpenReadAsync();
				content.Add(new StreamContent(sourceStream), "file", fileResult.FileName);

				var request = new HttpRequestMessage(HttpMethod.Post, "/api/Image/ImageAnalyser") { Content = content };

				var result = await _httpClient.SendAsync(request);

				if(result.IsSuccessStatusCode)
				{
					var openAIResults = JsonConvert.DeserializeObject<IEnumerable<OpenAIResult>>(await result.Content.ReadAsStringAsync());

					var openAIResult = openAIResults.FirstOrDefault();
					openAIResult.FileResult = fileResult;

					var resultPage = new Result(openAIResults.FirstOrDefault());
					await Navigation.PushAsync(resultPage);
				}
			}
		}
	}
}