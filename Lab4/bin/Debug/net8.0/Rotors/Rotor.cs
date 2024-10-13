namespace Lab4.Rotors
{
    public class Rotor
    {
        public Dictionary<char, char> Sequence { get; private set; } = [];
        public int Position { get; private set; } = 1;
        public int RotationNumber { get; private set; } = 1;
        public string SequenceFileText { get; private set; }

        public Rotor(string sequenceFile)
        {
            SequenceFileText = File.ReadAllText(sequenceFile).ToLower();
            SetAlphabet();
        }

        public void SetPosition(int position)
        {
            if (position < 1 || position > 26) {
                throw new Exception("Rotor positon must be in range from 1 to 26");
            }

            Position = 1;
            SetAlphabet();

            for (int i = 1; i < position; i++) {
                Rotate();
            }
        }

        public void SetRotationNumber(int rotationNumber)
        {
            if (rotationNumber < 1 || rotationNumber > 26) {
                throw new Exception("Rotor rotation number must be in range from 1 to 26");
            }

            RotationNumber = rotationNumber;
        }

        public char GetChar(char input) => Sequence[input];

        public void Rotate()
        {
            char keyChar = 'a';
            char dictionaryFirstValue = Sequence[keyChar];

            foreach (var item in Sequence)
            {
                Sequence[item.Key] = item.Key == 'z' ? dictionaryFirstValue : Sequence[++keyChar];
            }
            
            Position = Position == 26 ? 1 : Position++;
        }

        private void SetAlphabet()
        {
            char startChar = 'a';
            SequenceFileText.ToCharArray().ToList().ForEach((ch) => Sequence[startChar++] = ch);
        }
    }
}