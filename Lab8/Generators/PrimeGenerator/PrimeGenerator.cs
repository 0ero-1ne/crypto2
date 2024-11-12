namespace Lab8.Generators.PrimeGenerator
{
    public static class PrimeGenerator
    {
        private static readonly int N = 1_000_000;
        private static readonly List<int> lp = new(new int[N + 1]);
        private static readonly List<int> pr = [];

        static PrimeGenerator()
        {
            for (int i = 2; i <= N; i++)
            {
                if (lp[i] == 0)
                {
                    lp[i] = i;
                    pr.Add(i);
                }
                for (int j = 0; i * pr[j] <= N; j++)
                {
                    lp[i * pr[j]] = pr[j];
                    if (pr[j] == lp[i])
                    {
                        break;
                    }
                }
            }
        }

        public static int GetRandomPrime() =>  pr[new Random().Next(0, pr.Count - 1)];
    }
}