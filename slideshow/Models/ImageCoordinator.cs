using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol.Designer;
using Avalonia.Threading;
using slideshow.ViewModels;

namespace slideshow.Models;

public class ImageCoordinator
{
    public string? BasePath { get; set; }
    public MainWindowViewModel VM { get; set; }
    private DispatcherTimer _timer = new DispatcherTimer();
    private IEnumerator<string> _playlist;
    

    public ImageCoordinator()
    {
        _timer.Tick += TimerOnTick;
    }

    public void Start()
    {
        LoadPlaylist();
        _timer.Start();
    }

    public void LoadPlaylist()
    {
        _playlist = Directory.EnumerateFiles(BasePath).GetEnumerator();
    }

    public void SetInterval(TimeSpan interval)
    {
        _timer.Interval = interval;
    }

    private async void TimerOnTick(object? sender, EventArgs e)
    {
        if (!_playlist.MoveNext())
        {
            LoadPlaylist();
            return;
        }

        var next = _playlist.Current;
        Console.WriteLine($"Loading BMP '{next}'");
        var img = await LoadBitmap(next);
        Console.WriteLine("BMP Generated");
        var previous = VM.CurrentImage;
        VM.CurrentImage = img;
        Console.WriteLine("IMG assigned");
        previous?.Dispose();
    }

    private async Task<Bitmap> LoadBitmap(string path)
    {
        return new Bitmap(path);
    }
}