using Lab11.Elliptical;
using Lab11.ExtensionMethods;
using Lab11.Hash;

namespace Lab11.ECDSA
{
    public class ECDSA
    {
        public EllipticalCurve Curve;
        public IPoint G { get; } = new EllipticalPoint(416, 55); // point generator signature
        public IPoint Q { get; private set; } // public key
        private readonly int D; // private key
        private readonly int Order;

        public ECDSA(EllipticalCurve curve)
        {
            if (curve.Points.Length <= 33)
            {
                throw new Exception("Curve has not much points for russian alphabet (at least 34 points)");
            }
            Curve = curve;
            Order = curve.GetPointOrder(G);
            D = new Random().Next(2, Order - 1);
            Q = curve.MultiplyPoint(G, D);
        }

        public SignedMessage Sign(string message)
        {
            var hM = (int)(SHA1.Hash(message, false) % Order);
            var s = 0;
            var r = 0;

            while (s == 0)
            {
                var k = new Random().Next(2, Order - 1);
                var kG = Curve.MultiplyPoint(G, k);
                r = kG.X % Order;

                if (r == 0) continue;

                s = k.GetReversed(Order) * (hM + D * r) % Order;
            }
            
            return new SignedMessage
            {
                Message = message,
                R = r,
                S = s
            };
        }

        public bool Verify(SignedMessage signedMessage)
        {
            if (!Enumerable.Range(2, Order - 2).Contains(signedMessage.R)) {
                return false;
            }
            if (!Enumerable.Range(2, Order - 2).Contains(signedMessage.S)) {
                return false;
            }
            
            var hM = (int)(SHA1.Hash(signedMessage.Message!, false) % Order);
            var w = signedMessage.S.GetReversed(Order);

            var u1 = w * hM % Order;
            var u2 = w * signedMessage.R % Order;

            var u1G = Curve.MultiplyPoint(G, u1);
            var u2Q = Curve.MultiplyPoint(Q, u2);

            var resultPoint = Curve.SumPoints(u1G, u2Q);
            if (resultPoint is EllipticalInfinitePoint) return false;

            return resultPoint.X % Order == signedMessage.R;
        }

        public record class SignedMessage()
        {
            public string? Message { get; init; }
            public int R { get; init; }
            public int S { get; init; }
        }
    }
}