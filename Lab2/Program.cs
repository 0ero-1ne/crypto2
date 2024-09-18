static int GCD(int first, int second) {
    if (first <= 0 || second <= 0)
    {
        return -1;
    }

    int remainder = 0;

    while (first % second != 0)
    {
        remainder = first % second;
        first = second;
        second = remainder;
    }

    return remainder;
}

static int ReverseModuloNumber(int a, int N) {
    int left = 0;
    int right = 1;
    int reverse = 0;
    int NN = N;

    while (N % a != 0) {
        int q = N / a;
        int remainder = N % a;
        N = a;
        a = remainder;

        reverse = left - right * q;
        left = right;
        right = reverse;
    }

    if (reverse < 0) {
        reverse += NN;
    }

    return reverse;
}

static string AphineEncode(int a, int b, int N, string message, string characters) {
    if (a < 0 || a > N)
    {
        throw new Exception("Invalid param a. Must be 0 <= a < N");
    }

    if (b < 0 || b > N)
    {
        throw new Exception("Invalid param b. Must be 0 <= b < N");
    }

    if (GCD(a, N) != 1) {
        throw new Exception("Invalid param a\nGCD(a, N) must be 1");
    }

    string encodedMessage = "";

    foreach (var ch in message) {
        int index = (a * characters.IndexOf(ch) + b) % N;
        encodedMessage += characters[index];
    }

    return encodedMessage;
}

static string AphineDecode(int a, int b, int N, string message, string characters) {
    if (a < 0 || a > N)
    {
        throw new Exception("Invalid param a. Must be 0 <= a < N");
    }

    if (b < 0 || b > N)
    {
        throw new Exception("Invalid param b. Must be 0 <= b < N");
    }

    if (GCD(a, N) != 1) {
        throw new Exception("Invalid param a\nGCD(a, N) must be 1");
    }

    var reversedModulo = ReverseModuloNumber(a, N);
    string decodedMessage = "";

    foreach (var ch in message) {
        int index = reversedModulo * (characters.IndexOf(ch) + N - b) % N;
        decodedMessage += characters[index];
    }

    return decodedMessage;
}

static string VigenereEncode(string message, string key, string characters) {
    string encodedMessage = "";

    for (int i = 0; i < message.Length; i++) {
        int keyIndex = i % key.Length;
        int messageCharIndex = characters.IndexOf(message[i]);
        int keyCharIndex = characters.IndexOf(key[keyIndex]);

        int index = (messageCharIndex + keyCharIndex) % characters.Length;
        encodedMessage += characters[index];
    }

    return encodedMessage;
}

static string VigenereDecode(string encodedMessage, string key, string characters) {
    string decodedMessage = "";

    for (int i = 0; i < encodedMessage.Length; i++) {
        int keyIndex = i % key.Length;
        int messageCharIndex = characters.IndexOf(encodedMessage[i]);
        int keyCharIndex = characters.IndexOf(key[keyIndex]);

        int index = (messageCharIndex - keyCharIndex + characters.Length) % characters.Length;

        decodedMessage += characters[index];
    }

    return decodedMessage;
}

string characters = "0123456789абвгдеёжзийклмнопрстуфхцчшщъыьэюя `~!@№#$;%:^&?*()-_=+.,/|{[]}\n\r\"\'";

var alphabet = new Dictionary<char, int>();
var aphineAlphabet = new Dictionary<char, int>();
var vigenereAlphabet = new Dictionary<char, int>();

var fileContent = File.ReadAllText("file.txt").ToLower();

string csvFileContent = @"./file.csv";
string csvAphineFilePath = @"./aphine.csv";
string csvVigenereFilePath = @"./vigenere.csv";

foreach (var ch in fileContent) {
    if (!alphabet.TryAdd(ch, 1)) {
        alphabet[ch] += 1;
    }
}

File.WriteAllText(csvFileContent, "Char\tFrequency\n");

foreach (var item in alphabet) {
    string ch = item.Key == '\n' ? "\\n" :
        item.Key == '\r' ? "\\r" :
        item.Key == '\"' ? "\\\"" :
        item.Key == '\'' ? "\\'" :
        item.Key.ToString();

    File.AppendAllText(csvFileContent, ch + "\t" + item.Value + '\n');
}

var a = 7;
var b = 10;

var watch = System.Diagnostics.Stopwatch.StartNew();
var aphineEncodedMessage = AphineEncode(a, b, characters.Length, fileContent, characters);
watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;

Console.WriteLine("Aphine encoding: " + elapsedMs);

var aphineDecodedMessage = AphineDecode(a, b, characters.Length, aphineEncodedMessage, characters);

foreach (var ch in aphineEncodedMessage) {
    if (!aphineAlphabet.TryAdd(ch, 1)) {
        aphineAlphabet[ch] += 1;
    }
}

File.WriteAllText(csvAphineFilePath, "Char\tFrequency\n");

foreach (var item in aphineAlphabet) {
    string ch = item.Key == '\n' ? "\\n" :
        item.Key == '\r' ? "\\r" :
        item.Key == '\"' ? "\\\"" :
        item.Key == '\'' ? "\\'" :
        item.Key.ToString();

    File.AppendAllText(csvAphineFilePath, ch + "\t" + item.Value + '\n');
}

Console.WriteLine(fileContent == aphineDecodedMessage);

watch = System.Diagnostics.Stopwatch.StartNew();
var vigenereEncodedMessage = VigenereEncode(fileContent, "воликов", characters);
watch.Stop();
elapsedMs = watch.ElapsedMilliseconds;

Console.WriteLine("Vigenere encoding: " + elapsedMs);

var vigenereDecodedMessage = VigenereDecode(vigenereEncodedMessage, "воликов", characters);

Console.WriteLine(vigenereDecodedMessage == fileContent);

foreach (var ch in vigenereEncodedMessage) {
    if (!vigenereAlphabet.TryAdd(ch, 1)) {
        vigenereAlphabet[ch] += 1;
    }
}

File.WriteAllText(csvVigenereFilePath, "Char\tFrequency\n");

foreach (var item in vigenereAlphabet) {
    string ch = item.Key == '\n' ? "\\n" :
        item.Key == '\r' ? "\\r" :
        item.Key == '\"' ? "\\\"" :
        item.Key == '\'' ? "\\'" :
        item.Key.ToString();

    File.AppendAllText(csvVigenereFilePath, ch + "\t" + item.Value + '\n');
}