using Lab7.Encoders;
using System.Numerics;

namespace Lab7.Encryptors
{
    public class Base64Backpack : Backpack
    {
        private readonly int Base64ByteSize = 6;
        public Base64Backpack() : base(6) { }

        public override BigInteger[] Encrypt(string message)
        {
            var bits = Base64.Utf8ToBase64Binary(message);
            BigInteger[] encryptedMessage = new BigInteger[bits.Length / Base64ByteSize];

            for (int i = 0; i < bits.Length / Base64ByteSize; i++)
            {
                var base64Byte = new string(bits.Skip(i * Base64ByteSize).Take(Base64ByteSize).ToArray());
                for (int j = 0; j < Base64ByteSize; j++)
                {
                    if (base64Byte[j] == '1')
                    {
                        encryptedMessage[i] += PublicKey![j];
                    }
                }
            }

            return encryptedMessage;
        }

        public override string Decrypt(BigInteger[] message)
        {
            string decodedBits = "";
            for (int i = 0; i < message.Length; i++)
            {
                string base64Byte = "";
                BigInteger sNumber = (message[i] * InversedA) % N;

                for (int j = 5; j >= 0; j--)
                {
                    base64Byte += sNumber >= _privateKey![j] ? '1' : '0';
                    sNumber -= sNumber >= _privateKey![j] ? _privateKey![j] : 0;
                }

                decodedBits += new string(base64Byte.Reverse().ToArray());
            }

            return Base64.EncodeBase64Binary(decodedBits);
        }
    }
}
