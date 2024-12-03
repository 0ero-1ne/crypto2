using Lab11.ECDSA;
using Lab11.ElGamal;
using Lab11.Elliptical;

var timer = System.Diagnostics.Stopwatch.StartNew();
var curve = new EllipticalCurve(-1, 1, 751);

Console.WriteLine("Операции над точками P, Q и R:");
var P = new EllipticalPoint(61, 129);
var Q = new EllipticalPoint(59, 365);
var R = new EllipticalPoint(105, 369);
int k = 6;
int l = 10;

Console.WriteLine("P: " + P);
Console.WriteLine("Q: " + Q);
Console.WriteLine("R: " + R);

var kP = curve.MultiplyPoint(P, k);
var lQ = curve.MultiplyPoint(Q, l);

Console.WriteLine("\nkP: " + kP);
Console.WriteLine("P + Q: " + curve.SumPoints(P, Q));
Console.WriteLine("kP + lQ - R: " + curve.SumPoints(curve.SumPoints(kP, lQ), curve.GetMinusPoint(R)));
Console.WriteLine("P - Q + R: " + curve.SumPoints(curve.SumPoints(P, R), curve.GetMinusPoint(Q)));

Console.WriteLine("\nЗашифрование/Расшифрование");

var elGamal = new ElGamal(curve);
var message = "трансформеры";

Console.WriteLine("Сообщение: " + message);

timer.Restart();
var encryptedMessage = elGamal.Encrypt(message);
timer.Stop();

Console.WriteLine("Зашифровано за " + timer);

foreach (var item in encryptedMessage)
{
    Console.WriteLine($"C1: {item.C1}, C2: {item.C2}");
}

timer.Restart();
var decryptedMessage = elGamal.Decrypt(encryptedMessage);
timer.Stop();

Console.WriteLine("Расшифровано за " + timer);
Console.WriteLine("Расшифрованное сообщение: " + decryptedMessage);

Console.WriteLine("\nПодпись и верификация");
var ecdsa = new ECDSA(curve);

timer.Restart();
var signedMessage = ecdsa.Sign(message);
timer.Stop();

Console.WriteLine("Сообщение подписано за " + timer);
Console.WriteLine($"ЭЦП: ({signedMessage.R}, {signedMessage.S})");
Console.WriteLine("Сообщение: " + signedMessage.Message);

timer.Restart();
var verifyMessage = ecdsa.Verify(signedMessage);
timer.Stop();

Console.WriteLine("Сообщение проверено за " + timer);
Console.WriteLine($"Результат проверки ЭЦП: {verifyMessage}");