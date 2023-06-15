using OpenCvSharp;
using System.Diagnostics.CodeAnalysis;

namespace WpfOpenCvSharpStudy.Extensions;

partial class OpenCvUtils
{
    /// <summary>
    /// 入力画像からヒストグラムのMatを求めます。 TryCreateHistogram() の方が便利です。
    /// </summary>
    public static void CreateHistogramMats(Mat imageBgr, out Mat[] hists)
    {
        if (imageBgr.Channels() != 3) throw new NotSupportedException("Only 3ch.");

        var histMats = new Mat[imageBgr.Channels()];
        for (int i = 0; i < histMats.Length; i++)
        {
            histMats[i] = new Mat();
        }

        Mat[] images = new[] { imageBgr };
        int[] histSize = new[] { 256 };
        Rangef[] ranges = new[] { new Rangef(0, 256) };

        Cv2.CalcHist(images, new[] { 0 }, null, histMats[0], 1, histSize, ranges);
        Cv2.CalcHist(images, new[] { 1 }, null, histMats[1], 1, histSize, ranges);
        Cv2.CalcHist(images, new[] { 2 }, null, histMats[2], 1, histSize, ranges);

        hists = histMats;
    }

    /// <summary>
    /// 入力画像からヒストグラムの int[] を求めます
    /// </summary>
    public static bool TryCreateHistogram(Mat imageBgr,
        [NotNullWhen(true)] out int[]? histBlue,
        [NotNullWhen(true)] out int[]? histGreen,
        [NotNullWhen(true)] out int[]? histRed)
    {
        histBlue = histGreen = histRed = null;
        if (imageBgr.Channels() != 3) throw new NotSupportedException("Only 3ch.");

        CreateHistogramMats(imageBgr, out Mat[] hists);

        static int[] ToInt32Array(float[] values)
        {
            var bs = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
                bs[i] = (int)values[i];
            return bs;
        }

        if (hists[0].GetArray(out float[] values))
            histBlue = ToInt32Array(values);

        if (hists[1].GetArray(out values))
            histGreen = ToInt32Array(values);

        if (hists[2].GetArray(out values))
            histRed = ToInt32Array(values);

        foreach (IDisposable d in hists)
            d.Dispose();

        return histBlue is not null && histGreen is not null && histRed is not null;
    }

}
