using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
using DALL_E_Conectivity;
using System.Xml.Serialization;
//Initialize PlantDiseases custom objects
List<string> plantDisease = new List<string>() { "Healthy", "Phytophthora root rot", "Fusarium wilt", "Bacterial leaf spot", "Anthracnose", "Downy mildew", "Powdery mildew", "Botrytis blight", "Black spot", "Rust", "Verticillium wilt" };
List<string> plantNames = new List<string>() { "rose" };
List<PlantDiseases> plantDiseases = new List<PlantDiseases>();
foreach (string name in plantDisease)
    foreach (string p in plantNames)
        plantDiseases.Add(new PlantDiseases(p, name));
// Set how many times will execute the procedure
int loopsCount = 5;
// Set your OpenAI API key
string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
// Set the base URL for the DALL-E API
const string url = "https://api.openai.com/v1/images/generations";
// Set the model to use for image generation
const string model = "image-alpha-001";
// Set the number of images to generate
const int imgCount = 10;
// Set the size of the images to generate
const int width = 512;
const int height = 512;
// Set the prompt for image generation
for (int i = 0; i < loopsCount; i++)
    await GetImagesFromDalle(i);
//stop console from closing 
Console.ReadKey();

async Task GetImagesFromDalle(int loopCounter) {
    foreach (PlantDiseases pd in plantDiseases) {
        Console.WriteLine($"Loop {loopCounter}");
        string prompt = $"{pd.DiseaseName} on {pd.PlantName} plant";
        if (string.IsNullOrEmpty(pd.DiseaseName)) continue;
        if (pd.DiseaseName.Equals("Healthy"))
            prompt = $"{pd.DiseaseName} {pd.PlantName} plant";
        Console.WriteLine($"Request DALL-E for: {prompt}");
        // Create an HTTP client
        using (var client = new HttpClient()) {
            // Set the API key for authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            // Set the content type for the HTTP request
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Create an HTTP request object with the image generation parameters
            var request = new HttpRequestMessage(HttpMethod.Post, url) {
                Content = new StringContent(JsonConvert.SerializeObject(new {
                    model = model,
                    prompt = prompt,
                    n = imgCount,
                    size = $"{width}x{height}",
                    response_format = "b64_json"
                }))
            };
            if (request?.Content?.Headers?.ContentType == null) continue;
            request.Content.Headers.ContentType.MediaType = "application/json";
            // Send the HTTP request to the DALL-E API
            var response = await client.SendAsync(request);
            Console.WriteLine($"Request response: {response.StatusCode}");
            if (response?.StatusCode != HttpStatusCode.OK) {
                Console.WriteLine("Skip to next request!!");
                continue;
            }
            // Deserialize the API response into a JSON object
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
            if (jsonResponse == null) continue;
            // Get Images from b64_json
            for (int i = 0; i < jsonResponse.data.Count; i++) {
                string? base64 = jsonResponse.data[i].b64_json;
                Bitmap? img = Base64StringToBitmap(base64);
                if (img != null) {
                    long unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                    string path = @"c:\Plant_Leaf_Diseases_Dataset\" + pd.DiseaseName.Replace(' ', '-');
                    DirectoryInfo di = Directory.CreateDirectory(path);
                    string filename = $"{pd.PlantName.Replace(' ', '_')}___{pd.DiseaseName.Replace(' ', '_')}_{unixTime}_{i}.png";
                    filename = di.FullName + "\\" + filename;
                    img.Save(filename, ImageFormat.Png);
                    Console.WriteLine("Saving image to " + filename);
                }
            }
            if (plantDiseases.FindIndex(x => x == pd) != plantDiseases.Count - 1) {
                Console.WriteLine("Waiting for 5 seconds before next request");
                Thread.Sleep(5000);
            } else
                Console.WriteLine("The End!");
        }
    }
}

Bitmap? Base64StringToBitmap(string base64String) {
    Bitmap? bmpReturn = null;
    byte[]? byteBuffer = Convert.FromBase64String(base64String);
    if (byteBuffer == null) return bmpReturn;
    using MemoryStream? memoryStream = new MemoryStream(byteBuffer) { Position = 0 };
    bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
    memoryStream.Close();
    byteBuffer = null;
    return bmpReturn;
}