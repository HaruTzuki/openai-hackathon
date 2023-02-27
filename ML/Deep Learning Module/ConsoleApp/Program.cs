using Deep_Learning_Module;
using System.IO;

// Create single instance of sample data from first line of dataset for model input
var imageBytes = File.ReadAllBytes(@"E:\tywbtsjrjv-1\Plant_leaf_diseases_dataset_with_augmentation\Plant_leave_diseases_dataset_with_augmentation\Apple___Apple_scab\00075aa8-d81a-4184-8541-b692b78d398a___FREC_Scab 3335(1).JPG");
DeepLearningModel.ModelInput sampleData = new DeepLearningModel.ModelInput() {
    ImageSource = imageBytes,
};

// Make a single prediction on the sample data and print results.
var sortedScoresWithLabel = DeepLearningModel.PredictAllLabels(sampleData);
Console.WriteLine($"{"Class",-40}{"Score",-20}");
Console.WriteLine($"{"-----",-40}{"-----",-20}");

foreach (var score in sortedScoresWithLabel) {
    Console.WriteLine($"{score.Key,-40}{score.Value,-20}");
}
Console.ReadKey();