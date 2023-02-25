using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using DALL_E_Conectivity;
//Initialize PlantDiseases custom objects
List<string> plantDisease = new List<string>() { "Powdery mildew", "Botrytis blight", "Black spot", "Rust", "Verticillium wilt", "Phytophthora root rot", "Fusarium wilt", "Bacterial leaf spot", "Anthracnose", "Downy mildew" };
List<string> plantNames = new List<string>() { "rose" };
List<PlantDiseases> plantDiseases = new List<PlantDiseases>();
foreach (string name in plantDisease)
    foreach (string p in plantNames)
        plantDiseases.Add(new PlantDiseases(p, name));

// Set your OpenAI API key
string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
// Set the base URL for the DALL-E API
const string url = "https://api.openai.com/v1/images/generations";
// Set the model to use for image generation
const string model = "image-alpha-001";
// Set the number of images to generate
const int count = 10;
// Set the size of the images to generate
const int width = 512;
const int height = 512;
// Set the prompt for image generation
foreach(PlantDiseases pd in plantDiseases){
    string prompt = $"{pd.DiseaseName} on {pd.PlantName} plant";
    Console.WriteLine(prompt);
    // Create an HTTP client
    using (var client = new HttpClient()){
        // Set the API key for authentication
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        // Set the content type for the HTTP request
        client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
        // Create an HTTP request object with the image generation parameters
        var request = new HttpRequestMessage(HttpMethod.Post, url){
            Content = new StringContent(JsonConvert.SerializeObject( new {
                model = model,
                prompt = prompt,
                n = count,
                size = $"{width}x{height}",
                response_format = "b64_json" }))};
        if (request?.Content?.Headers?.ContentType == null) continue;
        request.Content.Headers.ContentType.MediaType = "application/json";
        // Send the HTTP request to the DALL-E API
        var response = await client.SendAsync(request);
        Console.WriteLine($"Request response: {response.StatusCode}" );
        // Deserialize the API response into a JSON object
        var jsonResponse = JsonConvert.DeserializeObject<dynamic>( await response.Content.ReadAsStringAsync());
        if (jsonResponse == null) continue;
        // Get Images from b64_json
        for (int i = 0; i < jsonResponse.data.Count; i++){
            string? base64 = jsonResponse.data[i].b64_json;
            Bitmap? img = Base64StringToBitmap(base64);
            if(img != null) {
                long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                string path = @"c:\Plant_Diseases_Images\" + pd.DiseaseName.Replace(' ', '-');
                DirectoryInfo di = Directory.CreateDirectory(path);
                string filename = $"{pd.PlantName.Replace(' ', '-')}_{pd.DiseaseName.Replace(' ', '-')}_{unixTime}_{i}.png";
                filename = di.FullName + "\\" + filename;
                img.Save(filename, ImageFormat.Png);
                Console.WriteLine("Saving image to " + filename);
            }
        }
    }
    //stop console from closing 
}
Console.ReadKey();

Bitmap? Base64StringToBitmap(string base64String){
    Bitmap? bmpReturn = null;
    byte[]? byteBuffer = Convert.FromBase64String(base64String);
    if (byteBuffer == null) return bmpReturn;
    using MemoryStream? memoryStream = new MemoryStream(byteBuffer) { Position = 0 };
    bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
    memoryStream.Close(); 
    byteBuffer = null;
    return bmpReturn;
}