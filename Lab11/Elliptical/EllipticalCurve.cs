using Lab11.ExtensionMethods;

namespace Lab11.Elliptical
{
    public class EllipticalCurve
    {
        public int A { get; private set; }
        public int B { get; private set; }
        public int P { get; private set; }
        public EllipticalPoint[] Points { get; private set; } // w/o Infinite point

        public EllipticalCurve(int a, int b, int p)
        {
            if (p <= 0) throw new Exception("P param of curve cannot be negative or 0");
            A = a;
            B = b;
            P = p;
            Points = GetEllipticalPoints();
        }

        public EllipticalPoint[] GetPointsRange(int x1, int x2)
        {
            if (x1 < 0 || x2 < 0) throw new Exception("params cannot be below 0");
            x1 = x1 >= P ? 0 : x1;
            x2 = x2 >= P ? P : x2;
            return [.. Points.Where(point => point.X >= x1 && point.X <= x2)];
        }

        public IPoint SumPoints(IPoint point1, IPoint point2)
        {
            var check = CheckPointsForInfinity(point1, point2);
            if (check != null) return check;

            int lambda = GetLambda(point1, point2);
            if (lambda == -1) return new EllipticalInfinitePoint();

            int x = (lambda * lambda - point2.X - point1.X) % P;
            int y = (-point1.Y - lambda * (x - point1.X)) % P;

            return new EllipticalPoint(x.RemoveMinusByModule(P), y.RemoveMinusByModule(P));
        }

        public IPoint MultiplyPoint(IPoint point, int k)
        {
            if (k == 0 || point is EllipticalInfinitePoint) return new EllipticalInfinitePoint();
            IPoint result = new EllipticalInfinitePoint();
            var addend = point;

            while (k > 0)
            {
                if (k % 2 == 1)
                {
                    result = SumPoints(result, addend);
                }

                addend = SumPoints(addend, addend);
                k /= 2;
            }

            return result;
        }

        public int GetPointOrder(IPoint point)
        {
            int order = 1;
            IPoint buf = point;
            while (buf is not EllipticalInfinitePoint)
            {
                buf = SumPoints(buf, point);
                order++;
            }
            return order;
        }

        public IPoint GetMinusPoint(IPoint point)
        {
            return point is EllipticalInfinitePoint ?
                point :
                new EllipticalPoint(point.X, (-point.Y).RemoveMinusByModule(P));
        }

        private int GetLambda(IPoint point1, IPoint point2)
        {
            if (point1.X == point2.X && point1.Y != point2.Y) return -1;

            int up = point2.Y - point1.Y;
            int down = point2.X - point1.X;

            if (point1.X == point2.X)
            {
                up = 3 * point1.X * point1.X + A;
                down = 2 * point1.Y;
                if (down == 0) return -1;
            }

            up = up.RemoveMinusByModule(P) % P;
            down = down.RemoveMinusByModule(P) % P;

            return up % down == 0 ? up / down % P : up * down.GetReversed(P) % P;
        }

        private EllipticalPoint[] GetEllipticalPoints()
        {
            var xRange = Enumerable.Range(0, P).Select(x => KeyValuePair.Create(x, CurveFunction(x)));
            var yRange = Enumerable.Range(0, P).Select(y => KeyValuePair.Create(y, y * y % P));

            return [.. xRange.SelectMany(
                x => yRange.Where(y => y.Value == x.Value).Select(y => new EllipticalPoint(x.Key, y.Key))
            )];
        }

        private static IPoint? CheckPointsForInfinity(IPoint point1, IPoint point2)
        {
            return (point1, point2) switch {
                (EllipticalPoint point, EllipticalInfinitePoint) => point,
                (EllipticalInfinitePoint, EllipticalPoint point) => point,
                (EllipticalInfinitePoint, EllipticalInfinitePoint) => new EllipticalInfinitePoint(),
                _ => null
            };
        }
        
        private int CurveFunction(int x) => (x * x * x + A * x + B) % P;
    }
}