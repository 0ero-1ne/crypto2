namespace Lab14.NeuralNetwork
{
    public class NeuralNetwork
    {
        public NeuralNetworkConfig Config { get; private set; }
        public Perceptron.Perceptron[] Perceptrons { get; private set; }

        public NeuralNetwork(NeuralNetworkConfig config, List<int[]> weights)
        {
            Config = config;
            Perceptrons = Enumerable
                .Range(0, Config.K)
                .Select(p => new Perceptron.Perceptron(Config.GetPerceptronConfig())
                    { 
                        Weights = weights[p] 
                    }
                )
                .ToArray();
        }

        public int GetOutput() => Perceptrons.Select(p => p.Y).Aggregate((x, y) => x * y);

        public void Sync(int outputB)
        {
            Perceptrons
                .Where(p => p.Y == GetOutput())
                .ToList()
                .ForEach(p => p.Sync(GetOutput(), outputB, Config.Method));
        }

        public void SetPerceptronsInputs(List<int[]> inputs)
        {
            for (int i = 0; i < Perceptrons.Length; i++)
            {
                Perceptrons[i].Inputs = inputs[i];
            }
        }

        public List<int[]> GetWeights() => Perceptrons.Select(p => p.Weights).ToList();
    }
}