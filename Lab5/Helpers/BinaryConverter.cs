using System.Text;

namespace Lab5.Helpers
{
    public static class BinaryConverter
    {
        public static string UTF8ToBinary(string text) => string.Join(
            string.Empty,
            Encoding.UTF8.GetBytes(text).Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))
        );

        public static string BinaryToUTF8(string binaryText)
        {
            List<byte> bytes = [];

            for (int i = 0; i < binaryText.Length; i += 8) {
                bytes.Add(Convert.ToByte(binaryText[i..(i + 8)], 2));
            }
                
            return Encoding.UTF8.GetString([.. bytes]);
        }
    
        public static int BinaryToInt32(string binaryText) => Convert.ToInt32(binaryText, 2);
    
        public static string Int32ToBinary(int value) => Convert.ToString(value, 2).PadLeft(4, '0'); // value = {0..15}
    }
}