using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using QRCoder;
using slideshow.Models;
using slideshow.ViewModels;
using slideshow.Views;

namespace slideshow;

public partial class App : Application
{
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var mainWindow = LoadDependencies();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private MainWindow LoadDependencies()
    {   
        var defaultTimerInterval = 5;
        var windowState = WindowState.Maximized;
        var topmost = false;
        var systemDecoration = SystemDecorations.Full;
        if (GlobalConfig.FullScreen)
        {
            windowState = WindowState.FullScreen;
            topmost = true;
            systemDecoration = SystemDecorations.None;
        }

        var bmp = BuildLocalQr(GlobalConfig.LocalWebUrl);
        var vm = new MainWindowViewModel()
        {
            CurrentWindowState = windowState,
            CurrentTopmost = topmost,
            CurrentSystemDecorations = systemDecoration,
            CurrentInterval = defaultTimerInterval,
            LocalIP = GlobalConfig.LocalWebUrl,
            LocalIpQrCode = bmp,
        };

        vm.Coordinator = new ImageCoordinator()
        {
            VM = vm,
            BasePath = GlobalConfig.PicturesPath
        };

        vm.Coordinator.SetInterval(TimeSpan.FromSeconds(defaultTimerInterval));
        vm.Coordinator.Start();
        
        return new MainWindow
        {
            DataContext = vm
        };
    }
    private static Bitmap BuildLocalQr(string localAddress)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(localAddress, QRCodeGenerator.ECCLevel.Q);
        BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
        var bytes = qrCode.GetGraphic(20);
        var ms = new MemoryStream(bytes);
        var bmp = new Bitmap(ms);
        return bmp;
    }
}