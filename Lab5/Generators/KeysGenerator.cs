using Lab5.Helpers;
using Lab5.Tables;

namespace Lab5.Generators
{
    public static class KeysGenerator
    {
        public static string[] GenerateKeys(string key)
        {
            string[] keys = new string[16];
            var key56Bits = Permutator.Permutate(key, DesTables.KeyPermutationTo56BitsTable);

            for (int i = 0; i < 16; i++) {
                key56Bits = ShiftKey(key56Bits, DesTables.KeyShiftTable[i]);
                keys[i] = Permutator.Permutate(key56Bits, DesTables.KeyPermutationTo48BitsTable);
            }

            return keys;
        }
   
        private static string ShiftKey(string key, int shiftsNumber)
        {
            string left = key[..28];
            string right = key[28..];

            for (int i = 0; i < shiftsNumber; i++) {
                left = left[1..] + left[..1];
                right = right[1..] + right[..1];
            }

            return left + right;
        }
    }
}