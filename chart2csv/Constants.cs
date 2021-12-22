using SixLabors.ImageSharp;

namespace chart2csv;

public static class Constants
{
    public static readonly Color PointColor = Color.ParseHex("3366CC");
    public static readonly Color BackgroundColor = Color.ParseHex("FFFFFF");

    public static readonly Color LineCornerColor = Color.ParseHex("B2B2B2");
    public static readonly Color LineColor = Color.ParseHex("C0C0C0");
    public static readonly Color LineXLabelColor = Color.ParseHex("A8A8A8");
    public static readonly Color LineXEndColor = Color.ParseHex("D0D0D0");
    public static readonly Color LineYMarkerColor = Color.ParseHex("B1B1B1");
    public static readonly Color LineYLabelColor = Color.ParseHex("B2B2B2");
    public static readonly Color LineYEndColor = Color.ParseHex("CFCFCF");

    public static readonly Color NumberZero = Color.ParseHex("7A7A7A");
}