namespace Lab8.ExtensionMethods
{
    public static class Int64Extension
    {
        public static bool IsPrime(this long p)
        {
            if (p <= 1) return false;
            if (p == 2) return true;
            if (p % 2 == 0) return false;

            for (long i = 3; i <= (long)Math.Floor(Math.Sqrt(p)); i++)
            {
                if (p % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static long GetReversed(this long value, long module)
        {
            long left = 0;
            long right = 1;
            long reverse = 0;
            long NN = module;

            while (module % value != 0) {
                long q = module / value;
                long remainder = module % value;
                module = value;
                value = remainder;

                reverse = left - right * q;
                left = right;
                right = reverse;
            }

            while (reverse < 0) {
                reverse += NN;
            }

            return reverse;
        }

        public static long GCD(this long first, long second)
        {
            if (first <= 0 || second <= 0)
            {
                return -1;
            }

            long remainder = 0;

            while (first % second != 0)
            {
                remainder = first % second;
                first = second;
                second = remainder;
            }

            return remainder;
        }

        public static long EilersFunction(this long value)
        {
            var divides = new Dictionary<long, int>();
            long div = 2;

            while (value > 1)
            {
                if (value % div == 0)
                {
                    if (!divides.TryAdd(div, 1)) {
                        divides[div] += 1;
                    }
                    value /= div;
                }
                else div++;
            }

            long result = 1;

            foreach (var item in divides)
            {
                result *= (item.Key - 1) * (long)Math.Pow(item.Key, item.Value - 1);
            }

            return result;
        }

        public static long GetCoprimeBelow(this long value)
        {
            if (value <= 0)
            {
                throw new Exception("Value is <= 0");
            }

            long result = 1;

            for (int i = 2; i <= (long)Math.Floor(Math.Sqrt(value)); i++)
            {
                if (GCD(value, i) == 1)
                {
                    result = i;
                }
            }

            return result;
        }

        public static long PowMod(this long a, long b, long m)
        {
            long result = 1;

            while (b != 0)
            {
                if ((b & 1) == 1)
                {
                    result = result * a % m;
                    b--;
                } else {
                    a = a * a % m;
                    b >>= 1;
                }
            }

            return result;
        }

        public static long GetPrimitiveRoot(this long value)
        {
            if (!value.IsPrime()) {
                throw new Exception("Value is not prime");
            }

            List<long> fact = [];
            long phi = value - 1;
            long n = phi;

            for (long i = 2; i * i <= n; i++)
            {
                if (n % i == 0)
                {
                    fact.Add(i);
                    while (n % i == 0)
                        n /= i;
                }
            }

            if (n > 1) fact.Add(n);

            for (int res = 2; res <= value; res++)
            {
                bool isPrimitiveRoot = true;
                for (int i = 0; i < fact.Count; i++)
                {
                    isPrimitiveRoot &= PowMod(res, phi / fact[i], value) != 1;
                }
                if (isPrimitiveRoot) return res;
            }

            return -1;
        }
    }
}