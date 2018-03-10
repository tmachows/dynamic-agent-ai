using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAI : MonoBehaviour
{
    private IAiModel AiModel;
    private Interpreter Interpreter;
    private List<Rule> Rules;

    public void PrepareModel(InputData sampleInput)
    {
        AiModel.BuildModel(sampleInput);
    }

    public OutputAction Evaluate(InputData input)
    {
        List<Action> intermediateResult = AiModel.Evaluate(input);
        return Interpreter.ApplyRules(intermediateResult, Rules);
    }

    public void ProvideRules(List<Rule> rules)
    {
        this.Rules = rules;
    }

    public void Train(InputData input, OutputAction expectedOutput)
    {
        AiModel.Train(input, expectedOutput);
    }
}
