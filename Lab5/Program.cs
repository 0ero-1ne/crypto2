using Lab5.Helpers;
using Lab5.Des;
using System.Text;

static string AlignMessage(string message)
{
    int missingBits = 64 - message.Length % 64;
    return missingBits == 64 ? message : message.PadRight(message.Length + missingBits, '0');
}

string message = "message!";
StringBuilder binaryMessage = new(AlignMessage(BinaryConverter.UTF8ToBinary(message)));
binaryMessage[0] = binaryMessage[0] == '0' ? '1' : '0';

string key1 = "Информац";
string key2 = "зопаснос";
string key3 = "лаборато";

var des1 = new Des(BinaryConverter.UTF8ToBinary(key1));
var des2 = new Des(BinaryConverter.UTF8ToBinary(key2));
var des3 = new Des(BinaryConverter.UTF8ToBinary(key3));

Console.WriteLine($"Message: {message}");
Console.WriteLine(BinaryConverter.UTF8ToBinary(message));
Console.WriteLine("\nENCODDING");

var watch = System.Diagnostics.Stopwatch.StartNew();
//string encodedMessage1 = des1.Encode(AlignMessage(binaryMessage.ToString()));
string encodedMessage1 = des1.Encode(AlignMessage(BinaryConverter.UTF8ToBinary(message)));
string encodedMessage2 = des2.Encode(encodedMessage1);
string encodedMessage3 = des3.Encode(encodedMessage2);
watch.Stop();

Console.WriteLine($"DES1:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(encodedMessage1));
Console.WriteLine(encodedMessage1);

Console.WriteLine($"DES2:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(encodedMessage2));
Console.WriteLine(encodedMessage2);

Console.WriteLine($"DES3:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(encodedMessage3));
Console.WriteLine(encodedMessage3);

Console.WriteLine($"Amount encoding time: {watch}\n");
File.WriteAllText(@"encrypted.txt", BinaryConverter.BinaryToUTF8(encodedMessage3));

watch = System.Diagnostics.Stopwatch.StartNew();
string decodedMessage3 = des3.Decode(encodedMessage3);
string decodedMessage2 = des2.Decode(decodedMessage3);
string decodedMessage1 = des1.Decode(decodedMessage2);
watch.Stop();

Console.WriteLine($"DECODING\nDES3:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(decodedMessage3));
Console.WriteLine(decodedMessage3);

Console.WriteLine($"DES2:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(decodedMessage2));
Console.WriteLine(decodedMessage2);

Console.WriteLine($"DES1:");
Console.WriteLine(BinaryConverter.BinaryToUTF8(decodedMessage1));
Console.WriteLine(decodedMessage1);

Console.WriteLine($"\nAmount decoding time: {watch}");