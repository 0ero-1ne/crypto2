namespace Lab11.Elliptical
{
    public class EllipticalPoint(int x, int y) : IPoint
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var item = obj as EllipticalPoint;
            return item!.X == X && item!.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"EllipticalPoint({X};{Y})";
        }

        public static bool operator ==(EllipticalPoint point1, EllipticalPoint point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        public static bool operator !=(EllipticalPoint point1, EllipticalPoint point2)
        {
            return point1.X != point2.X || point1.Y != point2.Y;
        }
    }
}