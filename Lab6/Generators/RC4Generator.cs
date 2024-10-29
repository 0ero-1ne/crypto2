namespace Lab6.Generators
{
    public class RC4Generator(int n)
    {
        public int N { get; private set; } = n;

        public string Encrypt(string message, int[] key)
        {
            var kTable = GeneratePRS(key);
            var result = "";

            for (int i = 0; i < message.Length; i++)
            {
                result += (char)(message[i] ^ kTable[i] % (N * N));
            }

            return result;
        }

        public int[] GeneratePRS(int[] key)
        {
            var sTable = GenerateSTable(key);
            var kTable = new int[N * N];

            for (int k = 0; k < (N * N); k++) {
                kTable[k] = key[k % key.Length];
            }

            int i = 0;
            int j = 0;

            for (int k = 0; k < (N * N); k++) {
                i = (i + 1) % (N * N);
                j = (j + sTable[i]) % (N * N);
                (sTable[j], sTable[i]) = (sTable[i], sTable[j]);
                int a = (sTable[i] + sTable[j]) % (N * N);
                kTable[k] = sTable[a];
            }

            return kTable;
        }

        private int[] GenerateSTable(int[] key)
        {
            var sTable = new int[N * N];
            var kTable = new int[N * N];
            int i = 0;
            int j = 0;

            for (int k = 0; k < (N * N); k++) {
                sTable[k] = k;
                kTable[k] = key[k % key.Length];
            }

            while (i < (N * N)) {
                j = (j + sTable[i] + kTable[i]) % (N * N);
                (sTable[j], sTable[i]) = (sTable[i], sTable[j]);
                i++;
            }

            return sTable;
        }
    }
}