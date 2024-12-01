using System.Numerics;
using System.Text;
using Lab10.Encoders;
using Lab10.ExtensionMethods;
using Lab10.Generators.PrimeGenerator;
using Lab10.Hash;

namespace Lab10.ElGamal
{
    public class ElGamal
    {
        public long P { get; private set; } // part of public key (random prime number)
        public long Q { get; private set; } // part of public key (primitive root of P)
        public long Y { get; private set; } // part of public key
        private readonly long X; // private key

        public ElGamal()
        {
            P = PrimeGenerator.GetRandomPrime();
            Q = P.GetPrimitiveRoot();
            X = new Random().NextInt64(2, P - 1);
            Y = (long)BigInteger.ModPow(Q, X, P);
        }

        public ElGamalEncryptedByte[] AsciiEncrypt(string message)
        {
            return Encoding.ASCII.GetBytes(message)
                .Select(b => {
                    long K = PrimeGenerator.GetRandomPrime();
                    return new ElGamalEncryptedByte
                    {
                        A = (long)BigInteger.ModPow(Q, K, P),
                        B = (long)(BigInteger.ModPow(Y, K, P) * b % P)
                    };
                })
                .ToArray();
        }

        public string AsciiDecrypt(ElGamalEncryptedByte[] message)
        {
            return Encoding.ASCII.GetString(
                message
                    .Select(b => (byte)(BigInteger.ModPow(b.A, P - 1 - X, P) * b.B % P))
                    .ToArray()
            );
        }

        public ElGamalEncryptedByte[] Base64Encrypt(string message)
        {
            return Encoding.UTF8.GetBytes(Base64.Encode(message))
                .Select(b => {
                    long K = PrimeGenerator.GetRandomPrime();
                    return new ElGamalEncryptedByte
                    {
                        A = (long)BigInteger.ModPow(Q, K, P),
                        B = (long)(BigInteger.ModPow(Y, K, P) * b % P)
                    };
                })
                .ToArray();
        }

        public string Base64Decrypt(ElGamalEncryptedByte[] message)
        {
            return Base64.Decode(
                Encoding.UTF8.GetString(
                    message
                        .Select(b => (byte)(BigInteger.ModPow(b.A, P - 1 - X, P) * b.B % P))
                        .ToArray()
                )
            ); 
        }

        public SignedMessage.SignedMessage Sign(string message)
        {
            return new SignedMessage.SignedMessage
            {
                Message = message,
                Hash = Base64Encrypt(SHA1.Hash(message))
            };
        }

        public bool Verify(SignedMessage.SignedMessage signedMessage)
        {
            var messageHash = SHA1.Hash(signedMessage.Message!);
            var decryptedHash = Base64Decrypt(signedMessage.Hash);

            return messageHash == decryptedHash;
        }

        public record ElGamalEncryptedByte
        {
            public long A { get; set; }
            public long B { get; set; }
        }
    }
}