using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;

public class NeuralNetsAiModel : IAiModel
{
    private Dictionary<string, ActivationNetwork> Networks = new Dictionary<string, ActivationNetwork>();

    public void BuildModel(InputData sampleInput)
    {
        foreach (var inputGroup in sampleInput.InputGroups)
        {
            int inputLayerSize = inputGroup.Properties.Count;
            int hiddenLayerSize = inputLayerSize + 2;
            int outputLayerSize = inputGroup.PossibleActions.Count;
            ActivationNetwork network = new ActivationNetwork(new SigmoidFunction(), inputLayerSize, hiddenLayerSize, outputLayerSize);
            Networks.Add(inputGroup.Name, network);
        }
    }

    public List<Action> Evaluate(InputData input)
    {
        List<Action> intermediateActions = new List<Action>();
        foreach (var inputGroup in input.InputGroups)
        {
            ActivationNetwork network = Networks[inputGroup.Name];
            double[] result = network.Compute(InputGroupToPropertiesArray(inputGroup));
            for (int i = 0; i < result.Length; i++)
            {
                intermediateActions.Add(new Action { Name = inputGroup.PossibleActions[i], Probability = result[i] });
            }
        }
        return intermediateActions;
    }

    public void Train(InputData input, OutputAction expectedOutput)
    {
        foreach (var inputGroup in input.InputGroups)
        {
            ActivationNetwork network = Networks[inputGroup.Name];
            ISupervisedLearning learning = new BackPropagationLearning(network);

            learning.Run(InputGroupToPropertiesArray(inputGroup), expectedOutput.DetailedOutput[inputGroup.Name]);
        }
    }

    private static double[] InputGroupToPropertiesArray(InputGroup inputGroup)
    {
        return inputGroup.Properties.Select(property => property.Value).ToArray();
    }
}
