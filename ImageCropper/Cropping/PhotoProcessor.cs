using ImageCropper.Utils;
using OpenCvSharp;

namespace ImageCropper.Cropping;

internal class PhotoProcessor(
    string ImgDirectory,
    string OutputFolder) : IDisposable
{
    private readonly FileSystemWatcher watcher = new(ImgDirectory, "*.*");
    private bool disposedValue;

    public void WatcherProcessor()
    {
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }

        watcher.NotifyFilter = NotifyFilters.FileName;

        watcher.Renamed += Handler;

        watcher.EnableRaisingEvents = true;
    }

    private void Handler(object sender, RenamedEventArgs e)
    {
        if (IsImage(e.FullPath))
        {
            Mat image = Cv2.ImRead(e.FullPath);
            var photos = PhotoExtractor.ExtractWithGrayScale(image, 240, 500, 500);

            foreach (var photo in photos)
            {
                //var rotated = ImageRotator.RotateImage(photo);

                string outputPath = Path.Combine(OutputFolder, $"{photo.GenerateName()}.png");
                Cv2.ImWrite(outputPath, photo);

                Console.WriteLine($"Saved: {outputPath}");
            }
        }
    }

    public void ProcessPhotos()
    {
        //int photoCounter = 0;

        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }

#if !DEBUG

        Parallel.ForEach(Directory.EnumerateFiles(ImgDirectory), filePath =>
        {
            if (IsImage(filePath))
            {
                // File is an image

                Mat image = Cv2.ImRead(filePath);
                var photos = PhotoExtractor.ExtractWithGrayScale(image, 240, 500, 500);

                foreach (var photo in photos)
                {
                    //var rotated = ImageRotator.RotateImage(photo);

                    string outputPath = Path.Combine(OutputFolder, $"{photo.GenerateName()}.png");// $"Photo_{++photoCounter}.png");
                    Cv2.ImWrite(outputPath, photo);

                    Console.WriteLine($"Saved: {outputPath}");
                }
            }
        });
#else
        foreach (string filePath in Directory.EnumerateFiles(ImgDirectory))
        {
            if (IsImage(filePath))
            {
                // File is an image

                Mat image = Cv2.ImRead(filePath);
                var photos = PhotoExtractor.ExtractWithGrayScale(image);

                foreach (var photo in photos)
                {
                    var rotated = ImageRotator.RotateImage(photo);

                    string outputPath = Path.Combine(OutputFolder, $"{photo.GenerateName()}.png");
                    Cv2.ImWrite(outputPath, photo);

                    Console.WriteLine($"Saved: {outputPath}");
                }
            }
        }
#endif
    }

    private static bool IsImage(string filePath)
    {
        try
        {
            using var stream = File.OpenRead(filePath);
            // Check the file header to determine if it's an image
            byte[] header = new byte[8];
            stream.Read(header, 0, header.Length);

            // Check for common image file signatures
            if (header.Length >= 2 &&
                (header[0] == 0xFF && header[1] == 0xD8) || // JPEG
                (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47 && header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A) || // PNG
                (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46 && header[3] == 0x38 && (header[4] == 0x37 || header[4] == 0x39) && header[5] == 0x61)) // GIF
            {
                return true;
            }
        }
        catch (Exception)
        {
            // Error occurred while reading the file, assume it's not an image
        }

        return false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                watcher.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
