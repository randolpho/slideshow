using Avalonia.Media.Imaging;
using System.Net;

namespace slideshow;

public static class GlobalConfig
{
    public static bool FullScreen { get; set; } = false;

    public static string? PicturesPath { get; set; } = null;
    public static string? ServerIpAddress { get; set; }
    public static IPAddress? ServerBoundAddress { get; set; }
    public static string? ServerPort { get; set; }
    public static int? ServerBoundPort { get; set; }

    public static string? LocalWebUrl { get; set; }
}