using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;

string inputImagePath = @"C:\Users\Alisettar\Pictures\Scans\Scan_20240908 (7).png";
string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"output");

// Get all files in the directory
string[] files = Directory.GetFiles(outputDirectory);

// Delete each file in the directory
foreach (string file in files)
{
    File.Delete(file);
}

// Load the input image as grayscale
using Mat image = CvInvoke.Imread(inputImagePath, ImreadModes.Color);
using Mat grayImage = new();
// Convert the image to grayscale
CvInvoke.CvtColor(image, grayImage, ColorConversion.Bgr2Gray);

string outputPath = Path.Combine(outputDirectory, $"grayImage.png");
//CvInvoke.Imwrite(outputPath, grayImage);

// Apply Gaussian blur to reduce noise and improve contour detection
CvInvoke.GaussianBlur(grayImage, grayImage, new Size(5, 5), 1.3);

//outputPath = Path.Combine(outputDirectory, $"GaussianBlur.png");
//CvInvoke.Imwrite(outputPath, grayImage);

// Apply Canny edge detection
using Mat edges = new();
CvInvoke.Canny(grayImage, edges, 200, 400);

// Find contours (potential photo frames) in the edge image
using VectorOfVectorOfPoint contours = new();
CvInvoke.FindContours(edges, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

// Loop over the contours and extract each identified frame
for (int i = 0; i < contours.Size; i++)
{
    // Get the bounding box of the contour
    Rectangle boundingRect = CvInvoke.BoundingRectangle(contours[i]);

    // Extract the sub-image (photo) using the bounding box
    Mat extractedPhoto = new(image, boundingRect);

    // Save the extracted photo as a new PNG file
    outputPath = Path.Combine(outputDirectory, $"photo_{i + 1}.png");
    CvInvoke.Imwrite(outputPath, extractedPhoto);
    Console.WriteLine($"Saved: {outputPath}");
}
