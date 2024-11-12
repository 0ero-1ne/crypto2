using System.Text;

namespace Lab8.Encoders
{
    public static class Base64
    {
        private static readonly int Base64ByteSize = 6;
        private static readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

        public static string Encode(string input)
        {
            string encodedMessage = "";

            string bits = Utf8ToBase64Binary(input);
            int size = 0;
            byte index = 0;

            foreach (char bit in bits) {
                index = (byte)((index << 1) + (bit == '1' ? 1 : 0));

                if (++size == Base64ByteSize) {
                    encodedMessage += _chars[index];
                    size = index = 0;
                }
            }

            return encodedMessage + "".PadLeft(input.Length * 8 % 3, '=');
        }
        
        public static string Decode(string input)
        {
            string bits = Base64ToBinary(input.Replace("=", ""));

            byte[] bytes = new byte[bits.Length / 8];
            byte byt = 0;
            int size = 0;
            int i = 0;


            foreach (var bit in bits) {
                byt = (byte)((byt << 1) + (bit == '1' ? 1 : 0));

                if (++size == 8) {
                    bytes[i++] = byt;
                    size = byt = 0;
                }
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public static string EncodeBase64Binary(string bits)
        {
            string encodedMessage = "";
            int size = 0;
            byte index = 0;

            foreach (char bit in bits)
            {
                index = (byte)((index << 1) + (bit == '1' ? 1 : 0));

                if (++size == Base64ByteSize)
                {
                    encodedMessage += _chars[index];
                    size = index = 0;
                }
            }

            if (bits.Length % 8 == 2)
            {
                encodedMessage += "=";
            }
            else if (bits.Length % 8 == 4)
            {
                encodedMessage += "==";
            }
            
            return encodedMessage;
        }

        public static string Utf8ToBase64Binary(string input) => Utf8ToBinary(input) + "".PadRight(Base64ByteSize - input.Length * 8 % Base64ByteSize, '0');

        private static string Utf8ToBinary(string input)
        {
            return string.Join(
                "",
                Encoding.UTF8.GetBytes(input).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
            );
        }

        private static string Base64ToBinary(string input)
        {
            return string.Join(
                "",
                input.Select(c => Convert.ToString(_chars.IndexOf(c), 2).PadLeft(Base64ByteSize, '0'))
            );
        }
    }
}
