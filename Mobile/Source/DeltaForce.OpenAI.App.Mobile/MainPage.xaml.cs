using Plugin.Media.Abstractions;
using Plugin.Media;
using System.IO.Compression;
using System.Net;

namespace DeltaForce.OpenAI.App.Mobile
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private string localFilePath;
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
                    localFilePath = Path.Combine(FileSystem.CacheDirectory, fileResult.FileName);
                    UploadedOrSelectedImage.Source = localFilePath;
                    UploadedOrSelectedImage.
                }
            }
        }

        private async void UploadPhoto()
        {
            if(_httpClient != null)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;
                using var content = new MultipartFormDataContent();
                using var sourceStream = await fileResult.OpenReadAsync();
                content.Add(new StreamContent(sourceStream), "file", fileResult.FileName);

                var request = new HttpRequestMessage(HttpMethod.Post, "api/Images/ImageAnalyser") { Content = content };

                var result = await _httpClient.SendAsync(request);

            }
        }

        private void BtnUploadThePhoto_Clicked(object sender, EventArgs e)
        {
            UploadPhoto();
        }
    }
}