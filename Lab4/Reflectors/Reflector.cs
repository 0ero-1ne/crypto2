namespace Lab4.Reflectors
{
    public class Reflector
    {
        public Dictionary<char, char> Pairs { get; private set; }

        public Reflector(string reflectorFile)
        {
            Pairs = [];
            SetReflector(reflectorFile);
        }

        public char GetChar(char input) => Pairs[input];

        private void SetReflector(string reflectorFile)
        {
            var reflectorPairs = File.ReadAllLines(reflectorFile);

            foreach (var pair in reflectorPairs)
            {
                Pairs.Add(pair!.ToLower()[0], pair!.ToLower()[1]);
                Pairs.Add(pair!.ToLower()[1], pair!.ToLower()[0]);
            }
            
        }
    }
}