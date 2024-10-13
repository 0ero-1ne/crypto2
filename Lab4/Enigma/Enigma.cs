using Lab4.Reflectors;
using Lab4.Rotors;

namespace Lab4.Enigma
{
    public class Enigma(List<Rotor> rotors, Reflector reflector)
    {
        public readonly List<Rotor> rotors = rotors;
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

        public void SetRotorsRotationNumbers(int[] rotationNumbers)
        {
            if (rotationNumbers.Length != rotors.Count) {
                throw new Exception("Wrong numbers of rotors rotation numbers");
            }

            for (int i = 0; i < rotors.Count; i++) {
                rotors[i].SetRotationNumber(rotationNumbers[i]);
            }
        }

        public string EncodeMessage(string message)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string encodedMessage = "";
        
            foreach (var ch in message)
            {
                if (!alphabet.Contains(ch)) {
                    encodedMessage += ch;
                    continue;
                }

                char encodedChar = ch;

                rotors.ForEach((rotor) => encodedChar = rotor.GetChar(encodedChar));
                encodedChar = reflector.GetChar(encodedChar);

                for (int i = rotors.Count - 1; i >= 0; i--)
                {
                    encodedChar = rotors[i].Sequence.FirstOrDefault(item => item.Value == encodedChar).Key;
                }

                for (int i = 0; i < rotors.Count; i++) {
                    for (int j = 0; j < rotors[i].RotationNumber; j++) {
                        rotors[i].Rotate();
                    }
                }

                encodedMessage += encodedChar;
            }

            return encodedMessage;
        }
    
        public string DecodeMessage(string message) => EncodeMessage(message);
    }
}