using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public class OutputAction
    {
        public List<Action> ActionsToPerform { get; set; }
        public Dictionary<string, double[]> RawOutput { get; set; }
    }
}