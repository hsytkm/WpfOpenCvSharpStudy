using OpenCvSharp;
using System.Diagnostics;

namespace WpfOpenCvSharpStudy.Extensions;

partial class OpenCvUtils
{
    /// <summary>
    /// 入力Matの行列を文字列に変換します
    /// </summary>
    public static string ToExcelString<TPixel>(this Mat image, int channel = 0)
        where TPixel : unmanaged
    {
        (int rows, int cols) = (image.Rows, image.Cols);
        long stride = image.Step();
        int channels = image.Channels();
        Debug.Assert(channel < channels);

        var sb = new System.Text.StringBuilder();
        unsafe
        {
            TPixel* destPtr = (TPixel*)image.DataPointer;
            TPixel* rowHead = destPtr;
            TPixel* rowTail = rowHead + ((rows * stride) / channels);

            for (TPixel* rowPtr = rowHead; rowPtr < rowTail; rowPtr += stride)
            {
                TPixel* colHead = rowPtr + channel;
                TPixel* colTail = colHead + (cols * channels);
                for (TPixel* ptr = colHead; ptr < colTail; ptr += channels)
                {
                    sb.Append($"{*ptr}\t");
                }
                sb.AppendLine("");
            }
        }

        return sb.ToString();
    }

}
