using Lab4.Reflectors;
using Lab4.Rotors;

namespace Lab4.Enigma
{
    public class Enigma(List<Rotor> rotors, Reflector reflector)
    {
        readonly List<Rotor> rotors = rotors;
        readonly Reflector reflector = reflector;

        public void SetRotorsPositions(int[] positions)
        {
            if (positions.Length != rotors.Count) {
                throw new Exception("Wrong numbers of rotors positions");
            }   

            for (int i = 0; i < rotors.Count; i++) {
                rotors[i].SetPosition(positions[i]);
            }
        }

        public string EncodeMessage(string message)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string encodedMessage = "";

            var toRotate = new bool[rotors.Count];
        
            foreach (var ch in message)
            {
                if (!alphabet.Contains(ch)) {
                    encodedMessage += ch;
                    continue;
                }

                char encodedChar = ch;
                toRotate[0] = true;

                for (int i = 0; i < rotors.Count; i++) {
                    var prevPosition = rotors[i].Position;

                    if (toRotate[i]) {
                        rotors[i].Rotate();
                        toRotate[i] = false;
                    }

                    toRotate[(i + 1) % rotors.Count] = prevPosition > rotors[i].Position;
                }

                rotors.ForEach((rotor) => encodedChar = rotor.GetChar(encodedChar));
                encodedChar = reflector.GetChar(encodedChar);

                for (int i = rotors.Count - 1; i >= 0; i--)
                {
                    encodedChar = rotors[i].Sequence.FirstOrDefault(item => item.Value == encodedChar).Key;
                }

                encodedMessage += encodedChar;
            }

            return encodedMessage;
        }
    
        public string DecodeMessage(string message) => EncodeMessage(message);
    }
}