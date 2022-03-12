namespace chart2csv.Parser;

public readonly struct Pixel
{
    public int X { get; }
    public int Y { get; }

    public Pixel(int x, int y)
    {
        X = x;
        Y = y;
    }
}
