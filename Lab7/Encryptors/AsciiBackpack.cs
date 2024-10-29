using System.Numerics;
using System.Text;

namespace Lab7.Encryptors
{
    public class AsciiBackpack : Backpack
    {
        public AsciiBackpack() : base(8) { }

        public override BigInteger[] Encrypt(string message)
        {
            BigInteger[] encryptedMessage = new BigInteger[message.Length];
            byte[] bytes = Encoding.ASCII.GetBytes(message);

            for (int i = 0; i < bytes.Length; i++)
            {
                string bits = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    if (bits[j] == '1')
                    {
                        encryptedMessage[i] += PublicKey![j];
                    }
                }
            }

            return encryptedMessage;
        }

        public override string Decrypt(BigInteger[] message)
        {
            byte[] bytes = new byte[message.Length];

            for (int i = 0; i < message.Length; i++)
            {
                string byt = "";
                BigInteger sNumber = (message[i] * InversedA) % N;

                for (int j = 7; j >= 0; j--)
                {
                    byt += sNumber >= _privateKey![j] ? '1' : '0';
                    sNumber -= sNumber >= _privateKey![j] ? _privateKey![j] : 0;
                }

                foreach (var bit in new string(byt.Reverse().ToArray()))
                {
                    bytes[i] = (byte)((bytes[i] << 1) + (bit == '1' ? 1 : 0));
                }
            }

            return Encoding.ASCII.GetString(bytes);
        }
    }
}
