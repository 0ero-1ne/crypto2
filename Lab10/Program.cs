using Lab10.ElGamal;
using Lab10.RSA;
using Lab10.Schnorr;

string message = "Hello, World!";

var rsa = new RSA();
var elGamal = new ElGamal();
var schnorr = new Schnorr();

Console.WriteLine("RSA\n");

var timer = System.Diagnostics.Stopwatch.StartNew();
var rsaSignedMessage = rsa.Sign(message);
timer.Stop();

Console.WriteLine($"RSA signed for {timer}");

timer.Start();
var rsaCheckSignedMessage = rsa.Verify(rsaSignedMessage);
timer.Stop();

Console.WriteLine($"RSA verification result {rsaCheckSignedMessage} in {timer}\n");

Console.WriteLine("ElGamal\n");

var elGamalSignedMessage = elGamal.Sign(message);
timer.Stop();

Console.WriteLine($"ElGamal signed for {timer}");

timer.Start();
var elGamalCheckSignedMessage = elGamal.Verify(elGamalSignedMessage);
timer.Stop();

Console.WriteLine($"ElGamal verification result {elGamalCheckSignedMessage} in {timer}\n");

Console.WriteLine("Schnorr\n");

var schnorrSignedMessage = schnorr.Sign(message);
timer.Stop();

Console.WriteLine($"Schnorr signed for {timer}");

timer.Start();
var schnorrCheckSignedMessage = schnorr.Verify(schnorrSignedMessage);
timer.Stop();

Console.WriteLine($"Schnorr verification result {schnorrCheckSignedMessage} in {timer}\n");
