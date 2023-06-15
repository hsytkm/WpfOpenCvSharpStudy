using OpenCvSharp;
using WpfOpenCvSharpStudy.Extensions;
using System.Diagnostics;
using System.Text;

namespace WpfOpenCvSharpStudy.Models;

internal static class ImageProcUseCase
{
    internal static void Test(Mat sourceImage)
    {
        // ヒストグラムの表示
        if (OpenCvUtils.TryCreateHistogram(sourceImage, out var blue, out var green, out var red))
        {
            Debug.WriteLine($"Bch hist\t{string.Join('\t', blue)}");
            Debug.WriteLine($"Gch hist\t{string.Join('\t', green)}");
            Debug.WriteLine($"Rch hist\t{string.Join('\t', red)}");
        }

        // 円の描画
        OpenCvUtils.DrawCircle(sourceImage, true);

        // 円の描画
        OpenCvUtils.DrawText(sourceImage, "Hello World");
    }

    // 色見で何かを判定します
    internal static bool PlaqueLightDetect(Mat sourceImage, out string log)
    {
        int RgRange1Min = 130;
        int RgRange1Max = 255;
        double RgRange1Ratio = 0.36;
        int RgRange2Min = 250;
        int RgRange2Max = 255;
        double RgRange2Ratio = 0.16;
        int VRange1Min = 250;
        int VRange1Max = 255;
        double VRange1Ratio = 0.33;

        bool detected = false;

        //①RとGのヒストグラム作成(0 - 255)
        if (!OpenCvUtils.TryCreateHistogram(sourceImage, out var histBlue, out var histGreen, out var histRed))
            throw new NotSupportedException("Cannot create bgr histograms.");

        //②各ヒストグラムでR - Gを行う(負の数は０でクリップ）
        var histRgDiff = new int[histGreen.Length];
        for (int i = 0; i < histRgDiff.Length; i++)
        {
            histRgDiff[i] = (histRed[i] > histGreen[i]) ? histRed[i] - histGreen[i] : 0;
        }

        //③ヒストグラムの130※-255※のR - Gの合計を算出(sum1) 
        int sum1 = 0;
        for (int i = RgRange1Min; i <= RgRange1Max; i++)
        {
            sum1 += histRgDiff[i];
        }

        //④ヒストグラムの250※-255※のR - Gの合計を算出(sum2)
        int sum2 = 0;
        for (int i = RgRange2Min; i <= RgRange2Max; i++)
        {
            sum2 += histRgDiff[i];
        }

        //⑤H / S / VのVヒストグラム作成(0 - 255)
        //⑥ヒストグラムの250※-255※の合計を算出(sum3)
        using var imageHsv = new Mat();
        Cv2.CvtColor(sourceImage, imageHsv, ColorConversionCodes.BGR2HSV);

        if (!OpenCvUtils.TryCreateHistogram(imageHsv, out var histH, out var histS, out var histV))
            throw new NotSupportedException("Cannot create hsv histograms.");

        int sum3 = 0;
        for (int i = VRange1Min; i <= VRange1Max; i++)
        {
            sum3 += histV[i];
        }

        //⑦以下いずれかの条件を満たした場合、外光あり
        //sum1 > 110000※ or sum2 > 50000※ or sum3 > 100000※
        double imageSize = sourceImage.Cols * sourceImage.Rows;
        double sum1Ratio = sum1 / imageSize;
        if (sum1Ratio > RgRange1Ratio)
            detected |= true;

        double sum2Ratio = sum2 / imageSize;
        if (sum2Ratio > RgRange2Ratio)
            detected |= true;

        double sum3Ratio = sum3 / imageSize;
        if (sum3Ratio > VRange1Ratio)
            detected |= true;

        {
            StringBuilder sb = new();
            sb.AppendLine($"B\t" + string.Join('\t', histBlue));
            sb.AppendLine($"G\t" + string.Join('\t', histGreen));
            sb.AppendLine($"R\t" + string.Join('\t', histRed));
            sb.AppendLine();
            sb.AppendLine($"R-G\t" + string.Join('\t', histRgDiff));
            sb.AppendLine();
            sb.AppendLine($"H\t" + string.Join('\t', histH));
            sb.AppendLine($"S\t" + string.Join('\t', histS));
            sb.AppendLine($"V\t" + string.Join('\t', histV));
            sb.AppendLine();
            sb.AppendLine($"Sum1\t{sum1Ratio:f3}\tTh={RgRange1Ratio:f3}\t->\t{sum1Ratio > RgRange1Ratio}");
            sb.AppendLine($"Sum2\t{sum2Ratio:f3}\tTh={RgRange2Ratio:f3}\t->\t{sum2Ratio > RgRange2Ratio}");
            sb.AppendLine($"Sum3\t{sum3Ratio:f3}\tTh={VRange1Ratio:f3}\t->\t{sum3Ratio > VRange1Ratio}");
            sb.AppendLine($"Daylight Detect\t->\t{detected}");
            log = sb.ToString();
        }

        return detected;
    }
}
