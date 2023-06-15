using OpenCvSharp;

namespace WpfOpenCvSharpStudy.Extensions;

partial class OpenCvUtils
{
    /// <summary>
    /// 入力画像に円を描画します
    /// </summary>
    public static void DrawCircle(Mat image, bool fill = false)
    {
        int centerX = image.Cols / 20;
        int centerY = image.Rows / 20;
        int radius = Math.Min(image.Cols, image.Rows) / 20;
        Scalar color = new(255, 0, 0);
        int thickness = fill ? -1 : 1;  // 負数だと塗り潰し

        Cv2.Circle(image, centerX, centerY, radius, color, thickness);
    }

}
