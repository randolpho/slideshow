using System;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using ReactiveUI;
using slideshow.Models;

namespace slideshow.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        ScreenTapped = ReactiveCommand.Create(OnScreenTap);
        XButtonTapped = ReactiveCommand.Create(OnXButtonTap);
        AddButtonTapped = ReactiveCommand.Create(OnAddButtonTap);
    }
    #region Properties
    #region CurrentSystemDecorations Property

    /// <summary>
    /// Reactive VM Property CurrentSystemDecorations 
    /// </summary>
    public SystemDecorations CurrentSystemDecorations
    {
        get => _currentSystemDecorations;
        set => this.RaiseAndSetIfChanged(ref _currentSystemDecorations, value);
    }

    /// <summary>
    /// Backing field for CurrentSystemDecorations
    /// </summary>
    private SystemDecorations _currentSystemDecorations;

    #endregion

    #region CurrentWindowState Property

    /// <summary>
    /// Reactive VM Property CurrentWindowState 
    /// </summary>
    public WindowState CurrentWindowState
    {
        get => _currentWindowState;
        set => this.RaiseAndSetIfChanged(ref _currentWindowState, value);
    }

    /// <summary>
    /// Backing field for CurrentWindowState
    /// </summary>
    private WindowState _currentWindowState = WindowState.Normal;

    #endregion

    #region CurrentTopmost Property

    /// <summary>
    /// Reactive VM Property CurrentTopmost 
    /// </summary>
    public bool CurrentTopmost
    {
        get => _currentTopmost;
        set => this.RaiseAndSetIfChanged(ref _currentTopmost, value);
    }

    /// <summary>
    /// Backing field for CurrentTopmost
    /// </summary>
    private bool _currentTopmost;

    #endregion

    #region CurrentImage Property

    /// <summary>
    /// Reactive VM Property CurrentImage 
    /// </summary>
    public Bitmap? CurrentImage
    {
        get => _currentImage;
        set => this.RaiseAndSetIfChanged(ref _currentImage, value);
    }

    /// <summary>
    /// Backing field for CurrentImage
    /// </summary>
    private Bitmap? _currentImage;

    #endregion

    #region CurrentInterval Property

    /// <summary>
    /// Reactive VM Property CurrentInterval 
    /// </summary>
    public int CurrentInterval
    {
        get => _currentInterval;
        set
        {
            Coordinator?.SetInterval(TimeSpan.FromSeconds(value));
            this.RaiseAndSetIfChanged(ref _currentInterval, value);
        }
    }

    /// <summary>
    /// Backing field for CurrentInterval
    /// </summary>
    private int _currentInterval;

    #endregion

    #region ShowTools Property

    /// <summary>
    /// Reactive VM Property ShowTools 
    /// </summary>
    public bool ShowTools
    {
        get => _showTools;
        set => this.RaiseAndSetIfChanged(ref _showTools, value);
    }

    /// <summary>
    /// Backing field for ShowTools
    /// </summary>
    private bool _showTools = false;
    #endregion

    #region ShowQr Property

    /// <summary>
    /// Reactive VM Property ShowQr 
    /// </summary>
    public bool ShowQr
    {
        get => _showQr;
        set => this.RaiseAndSetIfChanged(ref _showQr, value);
    }

    /// <summary>
    /// Backing field for ShowQr
    /// </summary>
    private bool _showQr = false;

    #endregion

    #region ShowImages Property

    /// <summary>
    /// Reactive VM Property ShowImages 
    /// </summary>
    public bool ShowImages
    {
        get => _showImages;
        set => this.RaiseAndSetIfChanged(ref _showImages, value);
    }

    /// <summary>
    /// Backing field for ShowImages
    /// </summary>
    private bool _showImages = true;

    #endregion
    
    public ImageCoordinator? Coordinator { get; set; }
    public string? LocalIP { get; set; }
    
    public Bitmap? LocalIpQrCode { get; set; }
    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> ScreenTapped { get; }
    public ReactiveCommand<Unit, Unit> XButtonTapped { get; }
    public ReactiveCommand<Unit, Unit> QrXButtonTapped { get; }
    public ReactiveCommand<Unit, Unit> AddButtonTapped { get; }
    #endregion

 
    #region Command Handlers
    private void OnScreenTap()
    {
        ShowImages = false;
        ShowQr = false;
        ShowTools = true;
    }

    private void OnXButtonTap()
    {
        ShowTools = false;
        ShowQr = false;
        ShowImages = true;
    }

    private void OnAddButtonTap()
    {
        ShowTools = false;
        ShowImages = false;
        ShowQr = true;
    }
    #endregion
}