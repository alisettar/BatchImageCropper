using OpenCvSharp;

namespace ImageCropper.Research;

internal class HSVandBrightnest
{
    void Test()
    {
        // Load the original image
        Mat originalImage = new();

        //// Set the brightness and contrast values
        //double alpha = 0.1; // Contrast (1.0-3.0)
        //int beta = 0;      // Brightness (-100 to 100)

        //// Create a new matrix to store the adjusted image
        //Mat adjustedImage = new();

        //// Apply brightness and contrast adjustments
        //originalImage.ConvertTo(adjustedImage, MatType.CV_8UC3, alpha, beta);
        //Cv2.ImWrite(outputDirectory + "/adjustedImage.png", adjustedImage);

        // Convert the image to grayscale
        Mat grayImage = new();
        Cv2.CvtColor(originalImage, grayImage, ColorConversionCodes.BGR2GRAY);
        //Cv2.ImWrite(outputDirectory + "/grayImage.png", grayImage);

        Cv2.GaussianBlur(grayImage, grayImage, new Size(5, 5), 1.5);

        //// Convert the image from BGR to HSV
        //Mat hsvImage = new();
        //Cv2.CvtColor(originalImage, hsvImage, ColorConversionCodes.BGR2HSV);

        //// Split the HSV channels (Hue, Saturation, Value)
        //Mat[] hsvChannels = Cv2.Split(hsvImage);
        //Mat hue = hsvChannels[0];
        //Mat saturation = hsvChannels[1];
        //Mat value = hsvChannels[2];

        //// Set the saturation adjustment value (1.0 = no change, <1.0 decreases, >1.0 increases saturation)
        //double saturationScale = 0;

        //hue.ConvertTo(hue, hue.Type(), 360, 0);

        //// Multiply the saturation channel by the scale factor
        //saturation.ConvertTo(saturation, saturation.Type(), saturationScale);

        //// Merge the channels back together
        //Mat adjustedHsvImage = new();
        //Cv2.Merge([hue, saturation, value], adjustedHsvImage);

        //// Convert back to BGR color space
        //Mat resultImage = new();
        //Cv2.CvtColor(adjustedHsvImage, resultImage, ColorConversionCodes.HSV2BGR);
        //Cv2.ImWrite(outputDirectory + "/resultImage.png", resultImage);

        //Mat grayImag1e = new();
        //Cv2.CvtColor(resultImage, grayImag1e, ColorConversionCodes.BGR2GRAY);
    }
}
