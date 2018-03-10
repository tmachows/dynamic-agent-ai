using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IAiModel
{
    void BuildModel(InputData sampleInput);

    List<Action> Evaluate(InputData input);

    void Train(InputData input, OutputAction expectedOutput);
}
