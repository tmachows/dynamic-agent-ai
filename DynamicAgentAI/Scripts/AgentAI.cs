using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public class AgentAI
    {
        private IAiModel AiModel;
        private Interpreter Interpreter;
        private List<Rule> Rules;

        public void PrepareModel(InputData sampleInput)
        {
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
    }
}