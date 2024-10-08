using Lab4.Reflectors;
using Lab4.Rotors;

namespace Lab4.Enigma
{
    public class Enigma(List<Rotor> rotors, Reflector reflector)
    {
        public List<Rotor> rotors = rotors;
        public Reflector reflector = reflector;

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
                throw new Exception("Wrong numbers of rotors positions");
            }   

            for (int i = 0; i < rotors.Count; i++) {
                rotors[i].SetRotationNumber(rotationNumbers[i]);
            }
        }
    
        public string EncodeMessage(string message)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string encodedMessage = "";
            var toRotate = new bool[rotors.Count];
        
            foreach (var ch in message)
            {
                char encodedChar = ch;
                toRotate[0] = true;
                
                if (!alphabet.Contains(encodedChar)) {
                    encodedMessage += encodedChar;
                    continue;
                }

                var prevFirstRotorPosition = rotors[0].RotorPosition;
                
                rotors[0].Rotate();

                if (prevFirstRotorPosition > rotors[0].RotorPosition) {
                    toRotate[1] = true;
                }

                foreach (var rotor in rotors)
                {
                    encodedChar = rotor.GetChar(encodedChar);
                }

                encodedChar = reflector.GetChar(encodedChar);

                for (int i = rotors.Count - 1; i >= 0; i--)
                {
                    encodedChar = rotors[i].Alphabet.FirstOrDefault(item => item.Value == encodedChar).Key;
                }

                for (int i = 1; i < rotors.Count; i++) {
                    var prevPositionOfRoter = rotors[i].RotorPosition;

                    if (toRotate[i] == true) {
                        rotors[i].Rotate();
                        toRotate[i] = false;
                    }

                    toRotate[(i + 1) % rotors.Count] = prevPositionOfRoter > rotors[i].RotorPosition;
                }

                encodedMessage += encodedChar;
            }

            return encodedMessage;
        }
    
        public string DecodeMessage(string message)
        {
            return EncodeMessage(message);
        }
    }
}