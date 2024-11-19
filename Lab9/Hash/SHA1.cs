using System.Text;

namespace Lab9.Hash
{
    public class SHA1
    {
        private static uint H0 = 0x67452301;
        private static uint H1 = 0xEFCDAB89;
        private static uint H2 = 0x98BADCFE;
        private static uint H3 = 0x10325476;
        private static uint H4 = 0xC3D2E1F0;

        private static readonly uint[] K = [
            0x5A827999,
            0x6ED9EBA1,
            0x8F1BBCDC,
            0xCA62C1D6
        ];

        public static string Hash(string message)
        {
            ResetHash();
            
            byte[] paddedMessage = GetPaddedMessage(message);
            for (int i = 0; i < paddedMessage.Length; i += 64)
            {
                HashBlock([.. paddedMessage.Skip(i).Take(64)]);
            }
            
            string hash = $"{H0:x8}{H1:x8}{H2:x8}{H3:x8}{H4:x8}";
            ResetHash();

            return hash;
        }

        private static byte[] GetPaddedMessage(string message)
        {
            List<byte> result = [.. Encoding.UTF8.GetBytes(message)];
            uint messageLength = (uint)(result.Count * 8);
            int lastBits = result.Count * 8 % 512;

            if (lastBits != 0 || message.Length == 0)
            {
                result.Add(0x80);

                int paddingLength = (448 - result.Count * 8 % 512) / 8;
                if (paddingLength < 0)
                {
                    paddingLength += 64;
                }
                result.AddRange(Enumerable.Repeat((byte)0x0, paddingLength));

                byte[] lengthBytes = BitConverter.GetBytes(messageLength);
                Array.Reverse(lengthBytes);
                result.AddRange(new byte[8 - lengthBytes.Length]);
                result.AddRange(lengthBytes);
            }

            return [.. result];
        }

        private static void HashBlock(byte[] block)
        {
            uint A = H0;
            uint B = H1;
            uint C = H2;
            uint D = H3;
            uint E = H4;

            uint[] W = new uint[80];

            for (int t = 0; t < 16; t++)
            {
                W[t] =  (uint)block[t * 4] << 24;
                W[t] |= (uint)block[t * 4 + 1] << 16;
                W[t] |= (uint)block[t * 4 + 2] << 8;
                W[t] |= block[t * 4 + 3];
            }

            for (int t = 16; t < 80; t++)
            {
                W[t] = SHA1CircularShift(1, W[t - 3] ^ W[t - 8] ^ W[t - 14] ^ W[t - 16]);
            }

            for(int t = 0; t < 20; t++)
            {
                var temp = SHA1CircularShift(5, A) + ((B & C) | ((~B) & D)) + E + W[t] + K[0];
                E = D;
                D = C;
                C = SHA1CircularShift(30, B);
                B = A;
                A = temp;
            }

            for(int t = 20; t < 40; t++)
            {
                var temp = SHA1CircularShift(5, A) + (B ^ C ^ D) + E + W[t] + K[1];
                E = D;
                D = C;
                C = SHA1CircularShift(30, B);
                B = A;
                A = temp;
            }

            for(int t = 40; t < 60; t++)
            {
                var temp = SHA1CircularShift(5, A) + ((B & C) | (B & D) | (C & D)) + E + W[t] + K[2];
                E = D;
                D = C;
                C = SHA1CircularShift(30, B);
                B = A;
                A = temp;
            }

            for(int t = 60; t < 80; t++)
            {
                var temp = SHA1CircularShift(5, A) + (B ^ C ^ D) + E + W[t] + K[3];
                E = D;
                D = C;
                C = SHA1CircularShift(30, B);
                B = A;
                A = temp;
            }

            H0 += A;
            H1 += B;
            H2 += C;
            H3 += D;
            H4 += E;
        }

        private static uint SHA1CircularShift(int bits, uint word) => (word << bits) | (word >> (32 - bits));
        
        private static void ResetHash()
        {
            H0 = 0x67452301;
            H1 = 0xEFCDAB89;
            H2 = 0x98BADCFE;
            H3 = 0x10325476;
            H4 = 0xC3D2E1F0;
        }
    }
}