using Lab14.Perceptron;

namespace Lab14.NeuralNetwork
{
    public class NeuralNetworkConfig : IPerceptron
    {
        public required int N { get; set; }
        public required int K { get; set; }
        public required int L { get; set; }
        public required TrainMethod Method { get; set; }

        public PerceptronConfig GetPerceptronConfig () => new()
        {
            N = N,
            L = L
        };
    }

    public enum TrainMethod
    {
        Heb,
        AntiHeb,
        RandomWalking
    }
}