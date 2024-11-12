using System.Numerics;
using System.Security.Cryptography;
using Lab8.ElGamal;
using Lab8.Generators.PrimeGenerator;

BigInteger a = new(7);
byte[] bytes = new byte[128];

using var rng = RandomNumberGenerator.Create();
rng.GetBytes(bytes);

BigInteger n = new(bytes);
if (n.Sign == -1) n = BigInteger.Multiply(n, -1); 

BigInteger[] xArray = [
    PrimeGenerator.GetRandomPrime(),
    PrimeGenerator.GetRandomPrime(),
    PrimeGenerator.GetRandomPrime(),
    PrimeGenerator.GetRandomPrime(),
    PrimeGenerator.GetRandomPrime(),
];

Console.WriteLine($"A = {a}\nN = {n}");

foreach (var item in xArray)
{
    var timer = System.Diagnostics.Stopwatch.StartNew();
    var y = BigInteger.ModPow(a, item, n);
    timer.Stop();
    Console.WriteLine($"X = {item}\t|\tTime = {timer}");
}

var message = "Dmitry";
var rsaEncryptor = new Lab8.RSA.RSA();
var elGamalEncryptor = new ElGamal();

Console.WriteLine("\nRSA\n");

var watch = System.Diagnostics.Stopwatch.StartNew();
var rsaAsciiEncryptedMessage = rsaEncryptor.AsciiEncrypt(message);
watch.Stop();
Console.WriteLine($"RSA ASCII ENCRYTPTION - {watch}");
Console.WriteLine($"RSA ASCII ENCRYPTED MESSAGE - {string.Join(" ", rsaAsciiEncryptedMessage.Select(c => c.ToString()))}");

watch.Start();
var rsaAsciiDecryptedMessage = rsaEncryptor.AsciiDecrypt(rsaAsciiEncryptedMessage);
watch.Stop();
Console.WriteLine($"RSA ASCII DECRYTPTION - {watch}");
Console.WriteLine($"RSA ASCII DECRYPTED MESSAGE - {rsaAsciiDecryptedMessage}\n");

watch.Start();
var rsaBase64EncryptedMessage = rsaEncryptor.Base64Encrypt(message);
watch.Stop();
Console.WriteLine($"RSA BASE64 ENCRYTPTION - {watch}");
Console.WriteLine($"RSA BASE64 ENCRYTPTED MESSAGE - {string.Join(" ", rsaBase64EncryptedMessage.Select(c => c.ToString()))}");

watch.Start();
var rsaBase64DecryptedMessage = rsaEncryptor.Base64Decrypt(rsaBase64EncryptedMessage);
watch.Stop();
Console.WriteLine($"RSA BASE64 DECRYTPTION - {watch}");
Console.WriteLine($"RSA BASE64 DECRYTPTED MESSAGE - {rsaBase64DecryptedMessage}");

Console.WriteLine("\nEL GAMAL\n");

watch.Start();
var elGamalAsciiEncryptedMessage = elGamalEncryptor.AsciiEncrypt(message);
watch.Stop();
Console.WriteLine($"EL GAMAL ASCII ENCRYPTION - {watch}");
Console.WriteLine($"EL GAMAL ASCII ENCRYPTED MESSAGE\n{string.Join('\n', elGamalAsciiEncryptedMessage.Select(b => b.A + " " + b.B).ToArray())}");


watch.Start();
var elGamalAsciiDecryptedMessage = elGamalEncryptor.AsciiDecrypt(elGamalAsciiEncryptedMessage);
watch.Stop();
Console.WriteLine($"EL GAMAL ASCII DECRYPTION - {watch}");
Console.WriteLine($"EL GAMAL ASCII DECRYPTED MESSAGE - {elGamalAsciiDecryptedMessage}\n");

watch.Start();
var elGamalBase64EncryptedMessage = elGamalEncryptor.Base64Encrypt(message);
watch.Stop();
Console.WriteLine($"EL GAMAL BASE64 ENCRYPTION - {watch}");
Console.WriteLine($"EL GAMAL BASE64 ENCRYPTED MESSAGE\n{string.Join('\n', elGamalBase64EncryptedMessage.Select(b => b.A + " " + b.B).ToArray())}");


watch.Start();
var elGamalBase64DecryptedMessage = elGamalEncryptor.Base64Decrypt(elGamalBase64EncryptedMessage);
watch.Stop();
Console.WriteLine($"EL GAMAL BASE64 DECRYPTION - {watch}");
Console.WriteLine($"EL GAMAL BASE64 DECRYPTED MESSAGE - {elGamalBase64DecryptedMessage}");