using Lab12.LSB;

var message = "hello, world! I am inside of image! Привет, я внутри изображения 😉";

var timer = System.Diagnostics.Stopwatch.StartNew();
LSB.Encrypt(@"test.png", message, true);
timer.Stop();

Console.WriteLine(timer);

timer.Restart();
var decryptedMessage = LSB.Decrypt("test-encrypted.png");
timer.Stop();

Console.WriteLine("Decrypted message: " + decryptedMessage + "\n" + timer);