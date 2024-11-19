using System.Diagnostics;
using Lab9.Hash;

var message = "hello";
var timer = Stopwatch.StartNew();
var hashedMessage = SHA1.Hash(message);
timer.Stop();
Console.WriteLine($"H(\"{message}\") = {hashedMessage} in {timer.ElapsedMilliseconds} ms");