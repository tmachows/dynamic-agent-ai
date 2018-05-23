using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;

namespace DynamicAgentAI
{
    public class NeuralNetsAiModel : IAiModel
    {
        private Dictionary<string, ActivationNetwork> Networks = new Dictionary<string, ActivationNetwork>();
        private double LearningRate = 0.8;
        private double Momentum = 0.9;

        public void BuildModel(InputData sampleInput)
        {
            foreach (var inputGroup in sampleInput.InputGroups)
            {
                int inputLayerSize = inputGroup.Properties.Count;
                int hiddenLayerSize = inputLayerSize * 2;
                int outputLayerSize = inputGroup.PossibleActions.Count;
                ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), inputLayerSize, hiddenLayerSize, outputLayerSize);
                Networks.Add(inputGroup.Name, network);
            }
        }

        public List<Action> ComputeActions(InputData input)
        {
            List<Action> intermediateActions = new List<Action>();
            foreach (var inputGroup in input.InputGroups)
            {
                ActivationNetwork network = Networks[inputGroup.Name];
                double[] result = network.Compute(InputGroupToPropertiesArray(inputGroup));
                for (int i = 0; i < result.Length; i++)
                {
                    intermediateActions.Add(new Action { Name = inputGroup.PossibleActions[i], Probability = result[i], GroupName = inputGroup.Name });
                }
            }
            return intermediateActions;
        }

        public void Train(InputData input, OutputAction expectedOutput, int iterations)
        {
            foreach (var inputGroup in input.InputGroups)
            {
                TrainSelectively(inputGroup, expectedOutput.RawOutput[inputGroup.Name], iterations);
            }
            if (LearningRate > 0.2)
            {
                LearningRate *= 0.99;
            }
            if (Momentum > 0.0)
            {
                Momentum *= 0.99;
            }
        }

        private static double[] InputGroupToPropertiesArray(InputGroup inputGroup)
        {
            return inputGroup.Properties.Select(property => property.Value).ToArray();
        }

        public void TrainSelectively(InputGroup inputGroup, double[] expectedOutput, int iterations)
        {
            ActivationNetwork network = Networks[inputGroup.Name];
            BackPropagationLearning learning = new BackPropagationLearning(network);
            learning.LearningRate = LearningRate;
            learning.Momentum = Momentum;

            for (int i = 0; i < iterations; i++)
            {
                learning.Run(InputGroupToPropertiesArray(inputGroup), expectedOutput);
            }
            if (LearningRate > 0.2)
            {
                LearningRate *= 0.99;
            }
            if (Momentum > 0.0)
            {
                Momentum *= 0.99;
            }
        }
    }
}