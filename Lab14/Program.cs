using Lab14.NeuralNetwork;

static List<int[]> GetRandomInputs(int N, int K)
{
    return Enumerable.Range(0, N * K)
        .Select(_ => new Random().Next(2) == 0 ? 1 : - 1)
        .Select((value, index) => new { value, index })
        .GroupBy(p => p.index / N)
        .Select(p => p.Select(v => v.value).ToArray())
        .ToList();
}

static List<int[]> GetRandomWeights(int N, int K, int L)
{
    return Enumerable.Range(0, N * K)
        .Select(_ => new Random().Next(-L, L + 1))
        .Select((value, index) => new { value, index })
        .GroupBy(p => p.index / N)
        .Select(p => p.Select(v => v.value).ToArray())
        .ToList();
}

static bool ComparePerceptrons(NeuralNetwork nn1, NeuralNetwork nn2)
{
    for (int i = 0; i < nn1.Perceptrons.Length; i++)
    {
        for (int j = 0; j < nn1.Perceptrons[i].Weights.Length; j++)
        {
            if ( nn1.Perceptrons[i].Weights[j] != nn2.Perceptrons[i].Weights[j])
                return false;
        }
    }

    return true;
}

static void SaveToCvsFile(Dictionary<int, int> rounds, int count)
{
    string filename = "rounds.csv";

    File.WriteAllText(filename, "Step;Count\n");
    foreach (var item in rounds)
    {
        File.AppendAllText(filename, $"{item.Key};{item.Value}\n");
    }

    Console.WriteLine($"Data about {count} rounds saved to rounds.cvs file");
}

var N = 12;
var K = 4;
var L = 2;

var config = new NeuralNetworkConfig()
{
    N = N,
    K = K,
    L = L,
    Method = TrainMethod.Heb
};

var weightsA = GetRandomWeights(N, K, L);
Console.WriteLine("NN A weights before sync:");
foreach (var item in weightsA)
{
    foreach (var weight in item)
    {
        Console.Write(weight + " ");
    }
    Console.WriteLine();
}

var weightsB = GetRandomWeights(N, K, L);
Console.WriteLine("\nNN B weights before sync:");
foreach (var item in weightsB)
{
    foreach (var weight in item)
    {
        Console.Write(weight + " ");
    }
    Console.WriteLine();
}

var networkA = new NeuralNetwork(config, weightsA);
var networkB = new NeuralNetwork(config, weightsB);

var steps = 0;
var found = false;

var timer = System.Diagnostics.Stopwatch.StartNew();
do
{
    var inputs = GetRandomInputs(N, K);

    networkA.SetPerceptronsInputs(inputs);
    networkB.SetPerceptronsInputs(inputs);

    networkA.Perceptrons.ToList().ForEach(p => p.CalculateOutputNetworkValue());
    networkB.Perceptrons.ToList().ForEach(p => p.CalculateOutputNetworkValue());

    var outputA = networkA.GetOutput();
    var outputB = networkB.GetOutput();

    if (outputA == outputB)
    {
        networkA.Sync(outputB);
        networkB.Sync(outputA);
    }

    if (ComparePerceptrons(networkA, networkB))
    {
        found = true;
    }
    
    steps++;
} while (!found && steps < 10000);
timer.Stop();

Console.WriteLine($"\nNN were synchronize in {steps} steps ({timer.ElapsedMilliseconds} ms)");
Console.WriteLine("NN A weights after sync:");
foreach (var item in networkA.GetWeights())
{
    foreach (var weight in item)
    {
        Console.Write(weight + " ");
    }
    Console.WriteLine();
}

Console.WriteLine("NN B weights after sync:");
foreach (var item in networkB.GetWeights())
{
    foreach (var weight in item)
    {
        Console.Write(weight + " ");
    }
    Console.WriteLine();
}

Dictionary<int, int> rounds = [];
int count = 10000;

for (int i = 0; i < count; i++)
{
    steps = 0;
    found = false;

    var weightsAstep = GetRandomWeights(N, K, L);
    var weightsBstep = GetRandomWeights(N, K, L);

    var networkAstep = new NeuralNetwork(config, weightsAstep);
    var networkBstep = new NeuralNetwork(config, weightsBstep);

    do
    {
        var inputs = GetRandomInputs(N, K);

        networkAstep.SetPerceptronsInputs(inputs);
        networkBstep.SetPerceptronsInputs(inputs);

        networkAstep.Perceptrons.ToList().ForEach(p => p.CalculateOutputNetworkValue());
        networkBstep.Perceptrons.ToList().ForEach(p => p.CalculateOutputNetworkValue());

        var outputA = networkAstep.GetOutput();
        var outputB = networkBstep.GetOutput();

        if (outputA == outputB)
        {
            networkAstep.Sync(outputB);
            networkBstep.Sync(outputA);
        }

        if (ComparePerceptrons(networkAstep, networkBstep))
        {
            if (!rounds.TryAdd(steps, 1)) {
                rounds[steps] += 1;
            }
            found = true;
        }
        
        steps++;
    } while (!found && steps < 10000);
}

Console.WriteLine();
SaveToCvsFile(rounds, count);