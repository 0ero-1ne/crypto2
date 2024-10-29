using Lab7.Encoders;
using Lab7.Encryptors;

const string message = "Volikov Dmitry Anatolevich";

var asciiEncryptor = new AsciiBackpack();

Console.WriteLine($"Message: {message}\n");
Console.WriteLine("ASCII\n");

var watch = System.Diagnostics.Stopwatch.StartNew();
var asciiEncryptedMessage = asciiEncryptor.Encrypt(message);
watch.Stop();

Console.WriteLine($"Encrypting time: {watch.Elapsed}");

for (int i = 0; i < asciiEncryptedMessage.Length; i++)
{
    Console.WriteLine($"{i} char: {asciiEncryptedMessage[i]}");
}

watch.Start();
var asciiDecryptedMessage = asciiEncryptor.Decrypt(asciiEncryptedMessage);
watch.Stop();
Console.WriteLine($"\nDecrypting time: {watch.Elapsed}");
Console.WriteLine(asciiDecryptedMessage);

Console.WriteLine("\n--------------------------------------------------------------\n");

Console.WriteLine("Base64\n");
var base64Encryptor = new Base64Backpack();
watch.Start();
var base64EncryptedMessage = base64Encryptor.Encrypt(message);
watch.Stop();

Console.WriteLine($"Encrypting time: {watch.Elapsed}");

for (int i = 0; i < base64EncryptedMessage.Length; i++)
{
    Console.WriteLine($"{i} char: {base64EncryptedMessage[i]}");
}

watch.Start();
var base64DecryptedMessage = base64Encryptor.Decrypt(base64EncryptedMessage);
watch.Stop();

Console.WriteLine($"\nDecrypting time: {watch.Elapsed}");
Console.WriteLine(base64DecryptedMessage);
Console.WriteLine(Base64.Decode(base64DecryptedMessage));