using OpenCvSharp;

namespace WpfOpenCvSharpStudy.Extensions;

partial class OpenCvUtils
{
    /// <summary>
    /// 入力画像に文字列を重畳します
    /// </summary>
    public static void DrawText(Mat image, string message, int x = 10, int y = 180)
    {
        Cv2.PutText(image,
            message,
            new Point(x, y),
            HersheyFonts.HersheyComplexSmall,
            1,
            new Scalar(255, 0, 255),
            1,
            LineTypes.AntiAlias);
    }

    /// <summary>
    /// 入力画像に複数行の文字列を重畳します
    /// </summary>
    public static void DrawTextLines(Mat image, string message)
    {
        const int offsetY = 20;
        int y = 100;

        foreach (var line in message.Split(Environment.NewLine))
        {
            if (!string.IsNullOrEmpty(line))
                DrawText(image, line, 10, y);

            y += offsetY;
        }
    }


}
