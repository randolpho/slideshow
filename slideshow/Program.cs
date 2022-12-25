using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;

namespace slideshow;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        if (GetRuntimeConfig())
        {
            Console.WriteLine(Usage);
            return;
        }

        BuildWebApp();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();


    public static void BuildWebApp()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        app.MapGet("/", () => "Hello World!");

        app.RunAsync(GlobalConfig.LocalWebUrl);
    }

    private static bool GetRuntimeConfig()
    {
        var arguments = Environment.GetCommandLineArgs();

        if (arguments.Length != 4 && arguments.Length != 5)
        {
            return true;
        }

        var path = arguments[1];
        var ip = arguments[2];
        var port = arguments[3];
        var di = new DirectoryInfo(path);
        if (!di.Exists)
        {
            Console.WriteLine($@"Invalid <imageDirectory> ""{path}""");
            return true;
        }

        IPAddress? address;
        if (!IPAddress.TryParse(ip, out address))
        {
            Console.WriteLine($@"Invalid <serverIpAddress> ""{ip}""");
            return true;
        }

        if (!int.TryParse(port, out var parsedPort))
        {
            Console.WriteLine($@"Invalid <serverPort> ""{port}""");
            return true;
        }

        var localAddress = $"http://{address}:{port}";

        GlobalConfig.ServerBoundAddress = address;
        GlobalConfig.ServerIpAddress = ip;
        GlobalConfig.PicturesPath = di.FullName;
        GlobalConfig.ServerBoundPort = parsedPort;
        GlobalConfig.LocalWebUrl = localAddress;

        if (arguments.Length == 5)
        {
            var fullScreen = arguments[4];
            if (fullScreen.ToLower() == "fullscreen")
            {
                GlobalConfig.FullScreen = true;
            }
            else
            {
                Console.WriteLine(
                    $@"Expected argument ""fullscreen"", but got invalid argument ""{fullScreen}"" instead. Ignoring...");
            }
        }

        return false;
    }


    private const string Usage = @"
Usage
    dotnet slideshow.dll <imageDirectory> <serverIpAddress> <serverPort> [fullscreen]

Options:
    <imageDirectory>        path to directory holding image files for the slideshow     
    <serverIpAddress>       the IP address to bind to when starting the local web server
    <serverPort>            the port to bind to when starting the local web server
    [fullscreen]            to automatically launch in full screen mode, append ""fullscreen""
";
}