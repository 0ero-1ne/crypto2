using Lab6.Generators;

var rsaGenerator = new RSAGenerator();
var rc4Generator = new RC4Generator(8);

Console.WriteLine("RSA PRS:");
var watch = System.Diagnostics.Stopwatch.StartNew();
var rsaSequence = rsaGenerator.Generate(8);
watch.Stop();
for (int i = 0; i < rsaSequence.Length; i++) {
    Console.WriteLine($"{i} number = {rsaSequence[i]}");
}
Console.WriteLine($"PRS RSA generating time: {watch}\n");

Console.WriteLine("RC4 PRS (10 numbers):");
watch = System.Diagnostics.Stopwatch.StartNew();
var rc4Sequence = rc4Generator.GeneratePRS([76, 111, 85, 54, 211]);
watch.Stop();
for (int i = 0; i < 10; i++) {
    Console.WriteLine($"{i} number = {rc4Sequence[i]}");
}
Console.WriteLine($"PRS RC4 generating time: {watch}");

var rc4EncryptedText = rc4Generator.Encrypt("Hello", [76, 111, 85, 54, 211]);
Console.WriteLine($"RC4 encrypted text: {rc4EncryptedText}");