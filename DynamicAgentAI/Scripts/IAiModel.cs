using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public interface IAiModel
    {
        void BuildModel(InputData sampleInput);

        List<Action> ComputeActions(InputData input);

        void Train(InputData input, OutputAction expectedOutput, int iterations);
    }
}