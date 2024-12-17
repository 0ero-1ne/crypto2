using Lab14.NeuralNetwork;

namespace Lab14.Perceptron
{
    public class Perceptron(PerceptronConfig config)
    {
        public PerceptronConfig Config { get; } = config;
        public required int[] Weights { get; set; }
        public int[]? Inputs { get; set; } = null;
        public int Y { get; private set; }

        public void CalculateOutputNetworkValue()
        {
            Y = Sign(Enumerable.Range(0, Weights.Length - 1).Sum(i => Weights[i] * Inputs![i]));
        }

        public void Sync(int outputA, int outputB, TrainMethod method)
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = LimitWeight(method switch
                {
                    TrainMethod.Heb => Weights[i] += Y * Inputs![i],
                    TrainMethod.AntiHeb => Weights[i] -= Y * Inputs![i],
                    TrainMethod.RandomWalking => Weights[i] += Inputs![i],
                    _ => 0
                });
                //Weights[i] *= Theta(Y * outputB) * Theta(-outputA * outputB);
            }
        }

        private int LimitWeight(int weight) => Math.Abs(weight) > Config.L ? Sign(weight) * Config.L : weight;

        public int GetOutput() => Sign(Enumerable.Range(0, Weights.Length - 1).Sum(i => Weights[i] * Inputs![i]));

        private static int Sign(int x) => x <= 0 ? -1 : 1;

        // private static int Theta(int x) => x >= 0 ? 1 : 0; 
    }
}