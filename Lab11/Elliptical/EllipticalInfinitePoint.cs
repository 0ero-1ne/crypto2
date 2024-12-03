namespace Lab11.Elliptical
{
    public class EllipticalInfinitePoint : IPoint
    {
        public int X { get; set; } = -1;
        public int Y { get; set; } = -1;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var item = obj as EllipticalInfinitePoint;
            return item!.X == X && item!.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "Infinite(-; -)";
        }

        public static bool operator ==(EllipticalInfinitePoint point1, EllipticalInfinitePoint point2)
        {
            return true;
        }

        public static bool operator !=(EllipticalInfinitePoint point1, EllipticalInfinitePoint point2)
        {
            return false;
        }
    }
}