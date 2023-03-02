using Plugin.Media.Abstractions;
using Plugin.Media;
using System.IO.Compression;
using System.Net;
using Microsoft.Maui.Controls;
using System;
using Newtonsoft.Json;
using DeltaForce.OpenAI.App.Mobile.Libraries;

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
        private async void OnTakePhotoClicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                fileResult = await MediaPicker.Default.CapturePhotoAsync();

                if (fileResult != null)
                {
                    // save the file into local storage

                    var stream = await fileResult.OpenReadAsync();
                    var bytes = new byte[stream.Length];
                    await stream.ReadAsync(bytes);

                    UploadedOrSelectedImage.Source = ImageSource.FromStream( () =>  new MemoryStream(bytes));
                    
                }
            }
        }

        private async void FromStorageClicked(object sender, EventArgs e)
        {

        }

        private async void UploadPhoto()
        {
            if(_httpClient != null)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;
                using var content = new MultipartFormDataContent();
                using var sourceStream = await fileResult.OpenReadAsync();
                content.Add(new StreamContent(sourceStream), "file", fileResult.FileName);

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/Image/ImageAnalyser") { Content = content };

                var result = await _httpClient.SendAsync(request);

                var openAIResult = JsonConvert.DeserializeObject<IEnumerable<OpenAIResult>>(await result.Content.ReadAsStringAsync());
            }
        }

        private void BtnUploadThePhoto_Clicked(object sender, EventArgs e)
        {
            UploadPhoto();
        }
    }
}