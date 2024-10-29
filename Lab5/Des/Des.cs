using Lab5.Helpers;
using Lab5.Generators;
using Lab5.Tables;

namespace Lab5.Des
{
    public class Des(string key)
    {
        private readonly string[] Keys = KeysGenerator.GenerateKeys(key);

        private string Encode(string message, bool encode)
        {
            var encodedMessage = "";

            for (int i = 0; i < message.Length; i += 64) {
                string block = Permutator.Permutate(message[i..(i + 64)], DesTables.InitialPermutationTable);
                string left = block[..32];
                string right = block[32..];

                for (int j = 0; j < 16; j++) {
                    string temp = right;
                    right = Permutator.Permutate(temp, DesTables.RExtensionTo48BitsTable);
                    right = XOR(Permutator.SBlockPermutate(XOR(right, encode ? Keys[j] : Keys[15 - j])), left);
                    left = temp;
                }

                encodedMessage += Permutator.Permutate(right + left, DesTables.FinalPermutationTable);
            }

            return encodedMessage;
        }

        public string Encode(string message) => Encode(message, true);

        public string Decode(string message) => Encode(message, false);

        private static string XOR(string first, string second)
        {
            var xorResult = "";

            for (int i = 0; i < first.Length; i++) {
                xorResult += first[i] == second[i] ? "0" : "1";
            }

            return xorResult;
        }
    }
}