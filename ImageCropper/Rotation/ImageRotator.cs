using OpenCvSharp;

namespace ImageCropper.Rotation;

internal class ImageRotator
{
    static double DetectImageOrientation(Mat image)
    {
        // Convert the image to grayscale
        using Mat gray = new();
        Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

        // Detect edges using Canny
        using Mat edges = new();
        Cv2.Canny(gray, edges, 50, 70);
        //Cv2.ImWrite(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"output/edges.png"), edges);

        // Detect lines using Hough Line Transform
        LineSegmentPoint[] lines = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 100, 30, 10);

        if (lines.Length == 0)
            return 0;

        // Calculate the average angle of the detected lines
        double angleSum = 0;
        int count = 0;
        foreach (var line in lines)
        {
            double angle = Math.Atan2(line.P2.Y - line.P1.Y, line.P2.X - line.P1.X) * 180 / Math.PI;
            angleSum += angle;
            count++;
        }

        double averageAngle = angleSum / count;

        return averageAngle;
    }

    static double RoundAngleToNearest90Or180(double angle)
    {
        // Normalize the angle to the nearest multiple of 90 or 180 degrees
        if (angle < -135 || angle > 135)
            return 180;  // Rotate by ±180 degrees
        else if (angle >= -135 && angle <= -45)
            return -90;  // Rotate by -90 degrees
        else if (angle > -45 && angle < 45)
            return 0;    // No rotation needed
        else
            return 90;   // Rotate by 90 degrees
    }

    internal static Mat RotateImage(Mat image)
    {
        // Detect orientation
        double angle = DetectImageOrientation(image);

        // Adjust angle to ±90° or ±180°
        angle = RoundAngleToNearest90Or180(angle);

        // Get the center of the image
        Point2f center = new(image.Cols / 2.0f, image.Rows / 2.0f);

        // Get the rotation matrix
        Mat rotationMatrix = Cv2.GetRotationMatrix2D(center, angle, 1.0);

        // Rotate the image
        Mat rotatedImage = new Mat();
        Cv2.WarpAffine(image, rotatedImage, rotationMatrix, image.Size(), InterpolationFlags.Linear, BorderTypes.Constant, Scalar.All(255));

        return rotatedImage;
    }
}
