using Lab12.ImageProcess;
using Lab12.LSB;

var message = File.ReadAllText(@"file.txt");

var timer = System.Diagnostics.Stopwatch.StartNew();
LSB.Encrypt(@"test.png", message, true);
timer.Stop();

Console.WriteLine("Put the message in image in " + timer);

timer.Restart();
ImageProcess.GetLSBImage(@"test.png");
timer.Stop();

Console.WriteLine("Full lsb image in " + timer);

timer.Restart();
ImageProcess.GetLSBImage(@"test-encrypted.png");
timer.Stop();

Console.WriteLine("Full lsb encrypted image in " + timer);

timer.Restart();
ImageProcess.GetLSBImageChannel(@"test-encrypted.png", 0);
timer.Stop();

Console.WriteLine("LSB encrypted image red channel in " + timer);

timer.Restart();
ImageProcess.GetLSBImageChannel(@"test-encrypted.png", 1);
timer.Stop();

Console.WriteLine("LSB encrypted image green channel in " + timer);

timer.Restart();
ImageProcess.GetLSBImageChannel(@"test-encrypted.png", 2);
timer.Stop();

Console.WriteLine("LSB encrypted image blue channel in " + timer);

timer.Restart();
var decryptedMessage = LSB.Decrypt("test-encrypted.png");
timer.Stop();

File.WriteAllText(@"file-decrypt.txt", decryptedMessage);
Console.WriteLine($"All decrypted text in file-decrypt.txt ({timer})");