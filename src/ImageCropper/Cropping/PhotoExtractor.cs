using OpenCvSharp;

namespace ImageCropper.Cropping;

internal class PhotoExtractor
{
    internal static Mat ExtractWithCanny(Mat image)
    {
        // Convert the image to grayscale
        Mat gray = new();
        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

        // Detect edges using Canny
        Mat edges = new();
        Cv2.Canny(gray, edges, 50, 150);

        // Detect contours
        Cv2.FindContours(edges, out Point[][] contours, out HierarchyIndex[] _, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

        // Find the largest contour
        double maxArea = 0;
        int maxAreaIndex = -1;
        for (int i = 0; i < contours.Length; i++)
        {
            double area = Cv2.ContourArea(contours[i]);
            if (area > maxArea)
            {
                maxArea = area;
                maxAreaIndex = i;
            }
        }

        // Get the bounding rectangle of the largest contour
        Rect boundingRect = Cv2.BoundingRect(contours[maxAreaIndex]);

        // Extract the photo from the original image
        Mat photo = new(image, boundingRect);

        return photo;
    }

    internal static List<Mat> ExtractWithGrayScale(
        Mat image,
        int grayThreshold = 240,
        int minWidth = 1000,
        int maxWidth = 1000)
    {
        // Convert the image to grayscale
        using Mat grayImage = new();
        Cv2.CvtColor(image, grayImage, ColorConversionCodes.BGR2GRAY);

        Cv2.GaussianBlur(grayImage, grayImage, new Size(5, 5), 1.5);

        // Apply a binary threshold to highlight potential frames
        using Mat thresholdImage = new();
        Cv2.Threshold(grayImage, thresholdImage, grayThreshold, 255, ThresholdTypes.BinaryInv);

        // Find contours (edges that represent shapes)
        Cv2.FindContours(
            thresholdImage,
            out Point[][] contours,
            out HierarchyIndex[] _,
            RetrievalModes.External,
            ContourApproximationModes.ApproxSimple);

        List<Mat> croppedRegions = [];

        // Process each contour to find rectangles
        foreach (Point[] contour in contours)
        {
            // Approximate the contour to a polygon and then check if it's a rectangle
            Point[] approxContour = Cv2.ApproxPolyDP(contour, 0.02 * Cv2.ArcLength(contour, true), true);

            //// A rectangle has 4 sides, so we only keep contours with 4 vertices
            //if (approxContour.Length == 4)
            //{
            // Get the bounding rectangle around the contour
            Rect boundingRect = Cv2.BoundingRect(approxContour);

            // Define a minimum size for rectangles to filter out noise
            if (boundingRect.Width > minWidth && boundingRect.Height > maxWidth)
            {
                // Crop the region corresponding to the photo
                croppedRegions.Add(new Mat(image, boundingRect));
            }
            //}
        }

        return croppedRegions;
    }
}
