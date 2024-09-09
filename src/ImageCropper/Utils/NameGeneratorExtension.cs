using OpenCvSharp;

namespace ImageCropper.Utils;

internal static class NameGeneratorExtension
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    static readonly Random random = new();

    internal static string GenerateName(this string name,
        int length = 7)
    {
        // Generate random string of the specified length
        return $"{name}_{new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray())}";
    }

    internal static string GenerateName(this Mat photo, int length = 7) 
        => GenerateName(photo.GetHashCode().ToString(), length);
}
