using Lab11.Elliptical;
using Lab11.Generators.PrimeGenerator;

namespace Lab11.ElGamal
{
    public class ElGamal
    {
        public EllipticalCurve Curve;
        public IPoint G { get; } = new EllipticalPoint(0, 1); // point generator encryption
        public IPoint Q { get; private set; } // public key
        private readonly int D; // private key
        private readonly string RussianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        public ElGamal(EllipticalCurve curve)
        {
            if (curve.Points.Length <= 33)
            {
                throw new Exception("Curve has not much points for russian alphabet (at least 34 points)");
            }
            Curve = curve;
            D = new Random().Next(2, Curve.P - 1);
            Q = curve.MultiplyPoint(G, D);
        }

        public EncryptedChar[] Encrypt(string message)
        {
            return message
                .Select(c => {
                    var k = PrimeGenerator.GetRandomPrime();
                    var p = Curve.Points[RussianAlphabet.IndexOf(c)];

                    return new EncryptedChar
                    {
                        C1 = Curve.MultiplyPoint(G, k),
                        C2 = Curve.SumPoints(p, Curve.MultiplyPoint(Q, k))
                    };
                }).ToArray();
        }

        public string Decrypt(EncryptedChar[] message)
        {
            return new string(
                message.Select(c => {
                    var point = Curve.SumPoints(c.C2!, Curve.GetMinusPoint(Curve.MultiplyPoint(c.C1!, D)));
                    return RussianAlphabet[Array.IndexOf(Curve.Points, point)];
                }).ToArray()
            );
        }

        public record class EncryptedChar()
        {
            public IPoint? C1 { get; init; }
            public IPoint? C2 { get; init; }
        }
    }
}