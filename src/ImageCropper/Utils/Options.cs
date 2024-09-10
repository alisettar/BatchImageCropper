using CommandLine;

namespace ImageCropper.Utils;

internal record Options
{
    [Option('i', "input", Required = true, HelpText = "Input folder path")]
    public required string InputFolder { get; set; }
    [Option('o', "output", Required = true, HelpText = "Output folder path")]
    public required string OutputFolder { get; set; }
    [Option('d', "delete", Required = false, HelpText = "Delete current files in the output folder")]
    public bool DeleteCurrentFiles { get; set; }
    [Option('w', "watch", Required = false, HelpText = "Activate/Deactivate watcher mode")]
    public bool WatchModeActive { get; set; }
}
