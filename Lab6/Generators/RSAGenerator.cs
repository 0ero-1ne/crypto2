using System.Numerics;

namespace Lab6.Generators
{
    public class RSAGenerator
    {
        private readonly BigInteger P;
        private readonly BigInteger Q;
        private readonly BigInteger N;
        private readonly BigInteger E;

        public RSAGenerator()
        {
            P = BigInteger.Parse("92575699611712814098324868827691115100832465461551981488684012097885956824763"); // Я не хочу писать генератор простых чисел
            Q = BigInteger.Parse("112067608275005981840867909737467414898838159295533174762192122642924053837623");
            E = BigInteger.Parse("102575699611712814098324868827691115100832465461551981488684012097885956824761");
            N = P * Q;
        }

        public BigInteger[] Generate(int length)
        {
            BigInteger x0 = new(new Random().Next());
            BigInteger[] sequence = new BigInteger[length];

            for (int i = 0; i < length; i++) {
                sequence[i] = BigInteger.ModPow(i == 0 ? x0 : sequence[i - 1], E, N);
            }

            return sequence;
        }
    }
}