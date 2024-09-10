using CommandLine;
using ImageCropper.Cropping;
using ImageCropper.Utils;

var options = Parser.Default.ParseArguments<Options>(args).Value;

if (options.DeleteCurrentFiles)
{
    // Get all files in the directory
    string[] files = Directory.GetFiles(options.OutputFolder);

    // Delete each file in the directory
    foreach (string file in files)
    {
        File.Delete(file);
    }

    Console.WriteLine($"Deleted all files in \"{options.OutputFolder}\"");
}

using PhotoProcessor photoProcessor = new(options.InputFolder, options.OutputFolder);

if (options.WatchModeActive)
{
    Console.WriteLine($"Watcher mode activated." +
        $"\nLooking for scanned files in \"{options.InputFolder}\"" +
        $"\nOutputting cropped photos to \"{options.OutputFolder}\"" +
        $"\nPress 'q' to quit. ");

    photoProcessor.WatcherProcessor();
}
else
{
    photoProcessor.ProcessPhotos();
}

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
