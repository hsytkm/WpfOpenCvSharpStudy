using CommunityToolkit.Mvvm.ComponentModel;
using WpfOpenCvSharpStudy.Extensions;
using WpfOpenCvSharpStudy.Models;
using System.IO;
using System.Windows.Media.Imaging;
using OpenCvSharp;

namespace WpfWpfOpenCvSharpStudy.ViewModels;

internal sealed partial class MainWindowViewModel : ObservableObject
{
    private const string SourceFilePath = "Resources/image01.png";

    [ObservableProperty]
    string _imageFilePath = "";

    [ObservableProperty]
    BitmapSource? _image;

    [ObservableProperty]
    string? _message;

    public MainWindowViewModel()
    {
        PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ImageFilePath))
                Image = CreateImage(ImageFilePath);
        };

        ImageFilePath = Path.Combine(Environment.CurrentDirectory, SourceFilePath);
    }

    private BitmapSource? CreateImage(string imagePath)
    {
        // "" で括られてたら除去る
        if (imagePath[0] == '\"' && imagePath[^1] == '\"')
            imagePath = imagePath[1..^1];

        if (!File.Exists(imagePath))
            return null;

        using var imageBgr = OpenCvUtils.LoadFromFile(imagePath);
        Message = ImageProcPlayground(imageBgr);
        return OpenCvSharp.WpfExtensions.BitmapSourceConverter.ToBitmapSource(imageBgr);
    }

    // 画像処理の遊び場
    private static string? ImageProcPlayground(Mat imageBgr)
    {
        string? log = null;
#if false
        ImageProcUseCase.Test(imageBgr);
        log = "Finish Test().";
#else
        ImageProcUseCase.PlaqueLightDetect(imageBgr, out log);
        Debug.WriteLine(log);
        OpenCvUtils.DrawTextLines(imageBgr, log.Replace('\t', ' '));
#endif
        return log;
    }
}
