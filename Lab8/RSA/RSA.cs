using System.Numerics;
using System.Text;
using Lab8.Encoders;
using Lab8.ExtensionMethods;
using Lab8.Generators.PrimeGenerator;

namespace Lab8.RSA
{
    public class RSA
    {
        public long N { get; private set; } // module
        public long E { get; private set; } // public key
        private readonly long D = 0; // private key

        public RSA()
        {
            N = (long)PrimeGenerator.GetRandomPrime() * PrimeGenerator.GetRandomPrime();
            E = N.EilersFunction().GetCoprimeBelow();
            D = E.GetReversed(N.EilersFunction());
        }

        public long[] AsciiEncrypt(string message)
        {
            return Encoding.ASCII.GetBytes(message)
                .Select(b => (long)BigInteger.ModPow(b, E, N))
                .ToArray();
        }

        public string AsciiDecrypt(long[] message)
        {
            return Encoding.ASCII.GetString(
                message.Select(m => (byte)BigInteger.ModPow(m, D, N))
                    .ToArray()
            );
        }

        public long[] Base64Encrypt(string message)
        {
            return Encoding.UTF8.GetBytes(Base64.Encode(message))
                .Select(b => (long)BigInteger.ModPow(b, E, N))
                .ToArray();
        }

        public string Base64Decrypt(long[] message)
        {
            return Base64.Decode(
                Encoding.UTF8.GetString(
                    message.Select(m => (byte)BigInteger.ModPow(m, D, N)).ToArray()
                )
            );
        }
    }
}