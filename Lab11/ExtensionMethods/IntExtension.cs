namespace Lab11.ExtensionMethods
{
    public static class IntExtension
    {
        public static int GetReversed(this int integer, int module)
        {
            if (integer.GCD(module) != 1) {
                throw new Exception("Error GetReversed method");
            }

            int left = 0;
            int right = 1;
            int reverse = 0;
            int NN = module;

            while (module % integer != 0) {
                int q = module / integer;
                int remainder = module % integer;
                module = integer;
                integer = remainder;

                reverse = left - right * q;
                left = right;
                right = reverse;
            }

            return reverse.RemoveMinusByModule(NN);
        }

        public static int GCD(this int first, int second)
        {
            if (first <= 0 || second <= 0)
            {
                return -1;
            }

            int remainder = 0;

            while (first % second != 0)
            {
                remainder = first % second;
                first = second;
                second = remainder;
            }

            return remainder;
        }

        public static int RemoveMinusByModule(this int value, int module)
        {
            while (value < 0)
            {
                value += module;
            }

            return value;
        }
    }
}