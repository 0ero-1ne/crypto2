using Lab11.ExtensionMethods;

namespace Lab11.Elliptical
{
    public class EllipticalCurve(int a, int b, int p)
    {
        public int A { get; set; } = a;
        public int B { get; set; } = b;
        public int P { get; set; } = p;

        public EllipticalPoint[] GetEllipticalPoints(int x1, int x2)
        {
            if (x1 < 0 || x2 < 0) throw new Exception("params cannot be below 0");
            x1 = x1 >= P ? 0 : x1;
            x2 = x2 >= P ? P - 1 : x2;
            var xRange = Enumerable.Range(x1, x2 - x1 + 1).Select(x => KeyValuePair.Create(x % P, CurveFunction(x)));
            var yRange = Enumerable.Range(0, P).Select(y => KeyValuePair.Create(y, y * y % P));

            return xRange.SelectMany(
                x => yRange.Where(y => y.Value == x.Value).Select(y => new EllipticalPoint(x.Key, y.Key))
            ).ToArray();
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
            var result = point;
            while (k > 1)
            {
                result = k % 2 == 0 ? SumPoints(result, result) : SumPoints(result, point);
                k = k % 2 == 0 ? k / 2 : k - 1;
            }
            return result;
        }

        public IPoint PMinusQPlusR(IPoint pPoint, IPoint qPoint)
        {
            var rPoint = SumPoints(pPoint, qPoint);
            return SumPoints(SumPoints(pPoint, GetMinusPoint(qPoint)), rPoint);
        }

        public IPoint KPPlusLQPMinusR(IPoint pPoint, IPoint qPoint, int k, int l)
        {
            var kP = MultiplyPoint(pPoint, k);
            var lQ = MultiplyPoint(qPoint, l);
            return SumPoints(SumPoints(kP, lQ), SumPoints(pPoint, qPoint));
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
            }

            up = up.RemoveMinusByModule(P) % P;
            down = down.RemoveMinusByModule(P) % P;

            return up % down == 0 ? up / down % P : up * down.GetReversed(P) % P;
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

        private IPoint GetMinusPoint(IPoint point)
        {
            return point is EllipticalInfinitePoint ?
                point :
                new EllipticalPoint(point.X, (-point.Y).RemoveMinusByModule(P));
        }
        
        private int CurveFunction(int x) => (x * x * x + A * x + B) % P;
    }
}