using System.Text;
using Lab5.Tables;

namespace Lab5.Helpers
{
    public static class Permutator
    {
        public static string Permutate(string block, int[] table)
        {
            var permutatedBlock = new StringBuilder("0".PadLeft(table.Length));

            for (int i = 0; i < table.Length; i++) {
                int index = table[i] - 1;
                permutatedBlock[i] = block[index];
            }

            return permutatedBlock.ToString();
        }

        public static string SBlockPermutate(string right)
        {
            var formatedRight = "";

            for (int i = 0; i < right.Length / 6; i++) {
                string block = right[(i * 6)..(i * 6 + 6)];
                int row = BinaryConverter.BinaryToInt32($"{block[0]}{block[5]}");
                int column = BinaryConverter.BinaryToInt32(block[1..5]);
                formatedRight += BinaryConverter.Int32ToBinary(DesTables.SConversionTable[i][row, column]);
            }

            return Permutate(formatedRight, DesTables.RPermutationTable);
        }
    }
}