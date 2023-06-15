using OpenCvSharp;

namespace WpfOpenCvSharpStudy.Extensions;

partial class OpenCvUtils
{
    /// <summary>
    /// 色空間を変換します
    /// </summary>
    public static Mat Bgr2Hsv(this Mat imageBgr)
    {
        var mat = new Mat();
        Cv2.CvtColor(imageBgr, mat, ColorConversionCodes.BGR2HSV);
        return mat;
    }

}
