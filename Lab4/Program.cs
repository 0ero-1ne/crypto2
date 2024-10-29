using Lab4.Enigma;
using Lab4.Reflectors;
using Lab4.Rotors;

List<Rotor> rotors = [
    new Rotor(@"./Rotors/RotorBeta.txt"),
    new Rotor(@"./Rotors/RotorThree.txt"),
    new Rotor(@"./Rotors/RotorGamma.txt"),
]; // First - right rotor, Last - left rotor

Reflector reflector = new(@"./Reflectors/ReflectorC.txt");

Enigma enigma = new(rotors, reflector);

var rotorsPositions = new int[] {1, 1, 1};

enigma.SetRotorsPositions(rotorsPositions);
enigma.SetRotorsRotationNumbers([1, 2, 2]);

string message = File.ReadAllText(@"./file.txt").ToLower();

if (message == "") {
    throw new Exception("Empty message");
}

Console.WriteLine($"MESSAGE\n{message}\n");

var watch = System.Diagnostics.Stopwatch.StartNew();
string encodedMessage = enigma.EncodeMessage(message);
watch.Stop();

Console.WriteLine($"ENCODED MESSAGE\n{encodedMessage}\n\nEncoding time: {watch.Elapsed}\n");

enigma.SetRotorsPositions(rotorsPositions);

watch = System.Diagnostics.Stopwatch.StartNew();
string decodedMessage = enigma.DecodeMessage(encodedMessage);
watch.Stop();

Console.WriteLine($"DECODED MESSAGE\n{decodedMessage}\n\nDecoding time: {watch.Elapsed}\n");

Console.WriteLine($"Are input text and encoded text same? {message == decodedMessage}");