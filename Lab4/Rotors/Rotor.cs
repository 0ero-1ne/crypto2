namespace Lab4.Rotors
{
    public class Rotor
    {
        public Dictionary<char, char> Alphabet { get; private set; }
        public int RotorPosition { get; private set; } = 1;
        public int RotationNumber { get; private set; } = 1;
        private readonly string AlphabetFile;

        public Rotor(string alphabetFile)
        {
            SetAlphabet(alphabetFile);
            Alphabet = [];
            AlphabetFile = alphabetFile;
        }

        public void SetPosition(int position)
        {
            if (position < 1 || position > 26) {
                throw new Exception("Rotor positon must be in range from 1 to 26");
            }

            SetAlphabet(AlphabetFile);

            RotorPosition = 1;

            for (int i = 1; i < position; i++) {
                Rotate();
            }
        }

        public void SetRotationNumber(int rotationNumber)
        {
            if (rotationNumber < 0 || rotationNumber > 25) {
                throw new Exception("Rotation number must be in range from 1 to 26");
            }

            RotationNumber = rotationNumber;
        }

        public char GetChar(char input) => Alphabet[input];

        public void Rotate()
        {
            Dictionary<char, char> newAlphabet = [];
            char startChar = 'a';
            char counterChar = 'b';

            foreach (var _ in Alphabet)
            {
                if (startChar == 'z') {
                    newAlphabet.Add(startChar, Alphabet['a']);
                } else {
                    newAlphabet.Add(startChar, Alphabet[counterChar]);
                }

                startChar++;
                counterChar++;
            }

            Alphabet = newAlphabet;

            RotorPosition++;

            if (RotorPosition == 27) {
                RotorPosition = 1;
            }
        }

        private void SetAlphabet(string alphabetFile)
        {
            Alphabet = [];
            char startChar = 'a';
            string alphabetRow = File.ReadAllText(alphabetFile).ToLower();

            foreach (var ch in alphabetRow)
            {   
                Alphabet.Add(startChar, ch);
                startChar++;
            }
        }
    }
}