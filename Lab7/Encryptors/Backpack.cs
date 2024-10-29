using System.Numerics;

namespace Lab7.Encryptors
{
    public abstract class Backpack
    {
        protected BigInteger[]? _privateKey;
        public BigInteger[]? PublicKey;
        public BigInteger N = new(0);
        protected BigInteger A = new(0);
        protected BigInteger InversedA = new(0);

        protected Backpack(int z)
        {
            GeneratePrivateKey(z);
            GeneratePublicKey(z);
        }

        public abstract BigInteger[] Encrypt(string message);
        public abstract string Decrypt(BigInteger[] message);

        private void GeneratePrivateKey(int z)
        {
            _privateKey = new BigInteger[z];
            _privateKey[0] = new BigInteger(new Random().NextInt64()) * 50;
            N = _privateKey[0];

            for (int i = 1; i < z; i++) {
                _privateKey[i] = _privateKey[i - 1] * 53;
                N += _privateKey[i];
            }

            N += new BigInteger(new Random().NextInt64());
        }

        private void GeneratePublicKey(int z)
        {
            PublicKey = new BigInteger[z];

            for (BigInteger i = new(100000); i < _privateKey![z - 1]; i++) {
                if (BigInteger.GreatestCommonDivisor(i, _privateKey[z - 1]) == 1) {
                    A = i;
                    InversedA = ReverseModuloNumber(A, N);
                    break;
                }
            }

            for (int i = 0; i < z; i++) {
                PublicKey[i] = (A * _privateKey[i]) % N;
            }
        }

        private BigInteger ReverseModuloNumber(BigInteger a, BigInteger N)
        {
            BigInteger left = new(0);
            BigInteger right = new(1);
            BigInteger reversed = new(0);

            while (N % a != 0)
            {
                BigInteger q = N / a;
                BigInteger remainder = N % a;
                (N, a) = (a, remainder);
                reversed = left - right * q;
                left = right;
                right = reversed;
            }

            while (reversed < 0)
            {
                reversed += this.N;
            }

            return reversed;
        }
    }
}