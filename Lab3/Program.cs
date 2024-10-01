using System.Text;

static void CalculateCharsFrequency(ref Dictionary<char, int> alphabet, string message)
{
    foreach (var ch in message) {
        if (!alphabet.TryAdd(ch, 1)) {
            alphabet[ch] += 1;
        }
    }
}

static void CharsFrequencyToCSV(ref readonly Dictionary<char, int> alphabet, string fileName)
{
    int length = alphabet.Sum(item => item.Value);

    File.WriteAllText(fileName, "Char\tFrequency\n");

    foreach (var item in alphabet) {
        string ch = item.Key == '\n' ? "\\n" :
            item.Key == '\r' ? "\\r" :
            item.Key == '\"' ? "\\\"" :
            item.Key == '\'' ? "\\'" :
            item.Key.ToString();

        File.AppendAllText(fileName, ch + "\t" + (double)item.Value / length + '\n');
    }
}

static int CalculateMatrixRows(int columns, int messageLength) {
    return (int)Math.Ceiling((double)messageLength / columns);
}

static string SpiralPermutationEncode(string message, int columns)
{   
    if (columns < 5) {
        throw new Exception("Spiral permutation encoding: Number of columns must be >= 5");
    }

    int rows = CalculateMatrixRows(columns, message.Length);
    char[,] matrix = new char[rows, columns];

    string formatMessage = message + new string('*', columns * rows - message.Length);

    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < columns; j++) {
            matrix[i, j] = formatMessage[i * columns + j];
        }
    }

    string encodedMessage = "";
    
    // Вообще не понимаю как читать по спирали, пытался вывести закономерность, но не получилось
    // Поэтому взял готовый алгоритм с какого-то задрыпанного сайта по С
    // https://www.haikson.com/programming/zapolnenie-dvumernoj-matritsyi-po-spirali/

    int Ibeg = 0;
    int Ifin = 0;
    int Jbeg = 0;
    int Jfin = 0;

    int counter = 0;
    int I = 0;
    int J = 0;

    while (counter < rows * columns){
        encodedMessage += formatMessage[I * columns + J];

        if (I == Ibeg && J < columns - Jfin - 1)
            ++J;
        else if (J == columns - Jfin - 1 && I < rows - Ifin - 1)
            ++I;
        else if (I == rows - Ifin - 1 && J > Jbeg)
            --J;
        else
            --I;

        if ((I == Ibeg + 1) && (J == Jbeg) && (Jbeg != columns - Jfin - 1)){
            ++Ibeg;
            ++Ifin;
            ++Jbeg;
            ++Jfin;
        }

        counter++;
    }

    // -------------------------------------------

    return encodedMessage;
}

static string SpiralPermutationDecode(string message, int columns)
{
    if (columns < 5) {
        throw new Exception("Spiral permutation encoding: Number of columns must be >= 5");
    }

    int rows = CalculateMatrixRows(columns, message.Length);
    char[,] matrix = new char[rows, columns];

    int Ibeg = 0;
    int Ifin = 0;
    int Jbeg = 0;
    int Jfin = 0;

    int counter = 0;
    int I = 0;
    int J = 0;

    while (counter < rows * columns){
        matrix[I, J] = message[counter];

        if (I == Ibeg && J < columns - Jfin - 1)
            ++J;
        else if (J == columns - Jfin - 1 && I < rows - Ifin - 1)
            ++I;
        else if (I == rows - Ifin - 1 && J > Jbeg)
            --J;
        else
            --I;

        if ((I == Ibeg + 1) && (J == Jbeg) && (Jbeg != columns - Jfin - 1)){
            ++Ibeg;
            ++Ifin;
            ++Jbeg;
            ++Jfin;
        }

        counter++;
    }

    string decodedMessage = "";

    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < columns; j++) {
            decodedMessage += matrix[i, j].ToString();
        }
    }

    return decodedMessage[..decodedMessage.IndexOf('*')];
}
    
static string GenerateRandomMessage(int length)
{
    string characters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
    string message = "";

    for (int i = 0; i < length; i++) {
        int rand = new Random().Next() % characters.Length;
        message += characters[rand];
    }

    return message;
}

static string MultiPermutationEncode(string message, string key1, string key2)
{
    string encodedMessage = "";

    var key1copy = new StringBuilder(key1);
    var key2copy = new StringBuilder(key2);

    char[][] matrix = new char[key1.Length][];

    string sortedKey1 = string.Concat(key1.OrderBy(c => c));
    string sortedKey2 = string.Concat(key2.OrderBy(c => c));

    for (int i = 0; i < key1.Length; i++) {
        matrix[i] = new char[key2.Length];
        for (int j = 0; j < key2.Length; j++) {
            matrix[i][j] = message[i * key2.Length + j];
        }
    }

    char[][] matrixBuf = (char[][])matrix.Clone();

    for (int i = 0; i < sortedKey1.Length; i++) {
        int rowIndex = key1copy.ToString().IndexOf(sortedKey1[i]);
        key1copy[rowIndex] = '_';
        matrix[i] = (char[])matrixBuf[rowIndex].Clone();
    }

    for (int i = 0; i < key1.Length; i++) {
        for (int j = 0; j < key2.Length; j++) {
            matrixBuf[i][j] = matrix[i][j];
        }
    }

    for (int i = 0; i < key1.Length; i++) {
        for (int j = 0; j < key2.Length; j++) {
            int columnIndex = key2copy.ToString().IndexOf(sortedKey2[j]);
            key2copy[columnIndex] = '_';
            matrix[i][j] = matrixBuf[i][columnIndex];
        }
        key2copy = new StringBuilder(key2);
    }

    for (int i = 0; i < key2.Length; i++) {
        for (int j = 0; j < key1.Length; j++) {
            encodedMessage += matrix[j][i];
        }
    }

    return encodedMessage;
}

static string MultiPermutationDecode(string message, string key1, string key2)
{
    string decodedMessage = "";

    char[][] matrix = new char[key1.Length][];
    char[][] matrixBuf = new char[key1.Length][];

    var sortedKey1 = new StringBuilder(string.Concat(key1.OrderBy(c => c)));
    var sortedKey2 = new StringBuilder(string.Concat(key2.OrderBy(c => c)));
    
    for (int i = 0; i < key1.Length; i++) {
        matrix[i] = new char[key2.Length];
        matrixBuf[i] = new char[key2.Length];
        for (int j = 0; j < key2.Length; j++) {
            matrix[i][j] = message[j * key1.Length + i];
            matrixBuf[i][j] = message[j * key1.Length + i];
        }
    }

    for (int i = 0; i < key1.Length; i++) {
        for (int j = 0; j < key2.Length; j++) {
            int columnIndex = sortedKey2.ToString().IndexOf(key2[j]);
            sortedKey2[columnIndex] = '_';
            matrix[i][j] = matrixBuf[i][columnIndex];
        }
        sortedKey2 = new StringBuilder(string.Concat(key2.OrderBy(c => c)));
    }

    for (int i = 0; i < key1.Length; i++) {
        for (int j = 0; j < key2.Length; j++) {
            matrixBuf[i][j] = matrix[i][j];
        }
    }

    for (int i = 0; i < sortedKey1.Length; i++) {
        int rowIndex = sortedKey1.ToString().IndexOf(key1[i]);
        sortedKey1[rowIndex] = '_';
        matrix[i] = (char[])matrixBuf[rowIndex].Clone();
    }

    for (int i = 0; i < key1.Length; i++) {
        for (int j = 0; j < key2.Length; j++) {
            decodedMessage += matrix[i][j];
        }
    }

    return decodedMessage;
}

var fileContent = File.ReadAllText("file.txt").ToLower();

var alphabet = new Dictionary<char, int>();
var spiralAlphabet = new Dictionary<char, int>();
var multiAlphabetBefore = new Dictionary<char, int>();
var multiAlphabetAfter = new Dictionary<char, int>();

string csvFileContent = @"./file.csv";
string csvSpiral = @"./spiral.csv";
string csvMultiBefore = @"./multiBefore.csv";
string csvMultiAfter = @"./multiAfter.csv";

CalculateCharsFrequency(ref alphabet, fileContent);
CharsFrequencyToCSV(ref alphabet, csvFileContent);

int spiralColumns = 20;

var watch = System.Diagnostics.Stopwatch.StartNew();
var spiralEncodedMessage = SpiralPermutationEncode(fileContent, spiralColumns);
watch.Stop();

Console.WriteLine("Spiral encoding time, ms: " + watch.Elapsed);

watch = System.Diagnostics.Stopwatch.StartNew();
var spiralDecodedMessage = SpiralPermutationDecode(spiralEncodedMessage, spiralColumns);
watch.Stop();

Console.WriteLine("Spiral decoding time, ms: " + watch.Elapsed);

CalculateCharsFrequency(ref spiralAlphabet, spiralEncodedMessage);
CharsFrequencyToCSV(ref spiralAlphabet, csvSpiral);

Console.WriteLine($"Spiral decoded == file: {spiralDecodedMessage == fileContent}");

const string key1 = "воликов";
const string key2 = "дмитрийй";

string randomMessage = GenerateRandomMessage(key1.Length * key2.Length);

CalculateCharsFrequency(ref multiAlphabetBefore, randomMessage);
CharsFrequencyToCSV(ref multiAlphabetBefore, csvMultiBefore);

Console.WriteLine();
Console.WriteLine("Random message for multi permutation: " + randomMessage);

watch = System.Diagnostics.Stopwatch.StartNew();
var multiEncodedMessage = MultiPermutationEncode(randomMessage, key1, key2);
watch.Stop();

Console.WriteLine("Multi encoding time, ms: " + watch.Elapsed);
Console.WriteLine("Encoded message: " + multiEncodedMessage);

CalculateCharsFrequency(ref multiAlphabetAfter, multiEncodedMessage);
CharsFrequencyToCSV(ref multiAlphabetAfter, csvMultiAfter);

watch = System.Diagnostics.Stopwatch.StartNew();
var multiDecodedMessage = MultiPermutationDecode(multiEncodedMessage, key1, key2);
watch.Stop();

Console.WriteLine("Multi decoding time, ms: " + watch.Elapsed);
Console.WriteLine("Decoded message: " + multiDecodedMessage);

Console.WriteLine($"Multi permutation decoded == random message: {multiDecodedMessage == randomMessage}");