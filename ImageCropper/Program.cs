using ImageCropper.Cropping;

string inputImagePath = @"C:\Users\Alisettar\Pictures\Scans\";
string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"output");

bool deleteCurrentFiles = false;
if (deleteCurrentFiles)
{
    // Get all files in the directory
    string[] files = Directory.GetFiles(outputDirectory);

    // Delete each file in the directory
    foreach (string file in files)
    {
        File.Delete(file);
    }
}

using PhotoProcessor photoProcessor = new(inputImagePath, outputDirectory);
photoProcessor.WatcherProcessor();

// Keep the application running
while (Console.Read() != 'q') ;

//// Load the original image
//Mat originalImage = Cv2.ImRead(inputImagePath);

//var photos = PhotoExtractor.ExtractWithGrayScale(originalImage);

//int photoCounter = 0;

//foreach (var photo in photos)
//{
//    string outputPath = Path.Combine(outputDirectory, $"photo_{++photoCounter}.png");
//    Cv2.ImWrite(outputPath, photo);

//    Console.WriteLine($"Saved: {outputPath}");
//}

//if (photoCounter == 0)
//{
//    Console.WriteLine("No rectangular photos were detected.");
//}
//else
//{
//    Console.WriteLine("Photo extraction complete.");
//}
