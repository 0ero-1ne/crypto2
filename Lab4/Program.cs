using Lab4.Enigma;
using Lab4.Reflectors;
using Lab4.Rotors;

List<Rotor> rotors = [
    new Rotor(@"./Rotors/RotorTwo.txt"),
    new Rotor(@"./Rotors/RotorThree.txt"),
    new Rotor(@"./Rotors/RotorFive.txt")
];

Reflector reflector = new(@"./Reflectors/ReflectorC.txt");

Enigma enigma = new(rotors, reflector);
enigma.SetRotorsPositions([1, 2, 1]);

string message = File.ReadAllText(@"./file.txt").ToLower();
string encodedMessage = enigma.EncodeMessage(message);

Console.WriteLine(encodedMessage);

enigma.SetRotorsPositions([1, 2, 1]);

string decodedMessage = enigma.DecodeMessage(encodedMessage);
Console.WriteLine(decodedMessage);