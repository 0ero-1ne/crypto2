using System.Numerics;
using Lab10.ExtensionMethods;
using Lab10.Generators.PrimeGenerator;
using Lab10.Hash;

namespace Lab10.Schnorr
{
    public class Schnorr
    {
        public long P { get; private set; }
        public long Q { get; private set; }
        public long G { get; private set; }
        public long Y { get; private set; }
        private readonly long X;

        public Schnorr()
        {
            P = PrimeGenerator.GetRandomPrime();
            Q = (P - 1).PrimeDivider();
            G = Q.GetRootOfPow(P);
            X = new Random().NextInt64(2, Q - 1);
            Y = (long)BigInteger.ModPow(G, Q - X, P);
        }

        public SignedMessage.SignedMessage Sign(string message)
        {
            var k = new Random().NextInt64(2, Q - 1);
            var a = (long)BigInteger.ModPow(G, k, P);
            var hashedMessage = SHA1.Hash(message + a, false);
            var b = (long)BigInteger.ModPow(k + X * hashedMessage, 1, Q);

            return new SignedMessage.SignedMessage
            {
                Message = message,
                Hash = new {
                    h = hashedMessage,
                    b
                }
            };
        }

        public bool Verify(SignedMessage.SignedMessage signedMessage)
        {
            var gParam = BigInteger.ModPow(G, signedMessage.Hash.b, P);
            var yParam = BigInteger.ModPow(Y, signedMessage.Hash.h, P);
            var X = (long)(BigInteger.Multiply(gParam, yParam) % P);

            return SHA1.Hash(signedMessage.Message + X, false) == signedMessage.Hash.h;
        }
    }
}