using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public class AgentAI
    {
        public IAiModel AiModel { get; set; } = new NeuralNetsAiModel();
        private Interpreter Interpreter = new Interpreter();
        private List<Rule> Rules = new List<Rule>();

        public void PrepareModel(IAiModel model, InputData sampleInput)
        {
            AiModel = new NeuralNetsAiModel();
            AiModel.BuildModel(sampleInput);
        }

        public OutputAction ComputeActions(InputData input)
        {
            List<Action> intermediateResult = AiModel.ComputeActions(input);
            return Interpreter.ApplyRules(intermediateResult, Rules);
        }

        public void ProvideRules(List<Rule> rules)
        {
            this.Rules = rules;
        }

        public void Train(InputData input, OutputAction expectedOutput, int iterations)
        {
            AiModel.Train(input, expectedOutput, iterations);
        }

        public void TrainSelectively(InputGroup inputGroup, double[] expectedOutput, int iterations)
        {
            AiModel.TrainSelectively(inputGroup, expectedOutput, iterations);
        }
    }
}