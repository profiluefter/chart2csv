using System;

namespace chart2csv
{
    public class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
            
        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        protected bool Equals(Pixel other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pixel)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}