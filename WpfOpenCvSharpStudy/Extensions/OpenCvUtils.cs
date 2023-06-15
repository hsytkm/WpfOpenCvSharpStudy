using OpenCvSharp;

namespace WpfOpenCvSharpStudy.Extensions;

public static partial class OpenCvUtils
{
    /// <summary>
    /// ファイルPATHからMatを作成します
    /// </summary>
    public static Mat LoadFromFile(string filename)
    {
        return new Mat(filename);
    }

}
