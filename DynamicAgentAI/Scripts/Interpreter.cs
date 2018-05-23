using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamicAgentAI
{
    public class Interpreter
    {
        public OutputAction ApplyRules(List<Action> intermediateResult, List<Rule> rules)
        {
            OutputAction outputAction = new OutputAction();
            outputAction.ActionsToPerform = intermediateResult;
            SetRawOutput(intermediateResult, outputAction);
            foreach (var rule in rules)
            {
                switch (rule.Relation)
                {
                    case Relation.EXCLUDES:
                        ApplyExcludes(rule, outputAction);
                        break;
                    case Relation.FORCES:
                        ApplyForces(rule, outputAction);
                        break;
                    case Relation.REINFORCES:
                        ApplyReinforces(rule, outputAction);
                        break;
                    default:
                        break;
                }
            }
            return outputAction;
        }

        private void ApplyExcludes(Rule rule, OutputAction outputAction)
        {
            List<string> actionNames = outputAction.ActionsToPerform.Select(action => action.Name).ToList();
            if (actionNames.Contains(rule.Action1))
            {
                outputAction.ActionsToPerform.RemoveAll(action => action.Name.Equals(rule.Action2));
            }
        }

        private void ApplyForces(Rule rule, OutputAction outputAction)
        {
            List<string> actionNames = outputAction.ActionsToPerform.Select(action => action.Name).ToList();
            if (actionNames.Contains(rule.Action1) && !actionNames.Contains(rule.Action2))
            {
                Action actionToAdd = new Action();
                actionToAdd.Name = rule.Action2;
                actionToAdd.Probability =
                    outputAction.ActionsToPerform.Find(action => action.Name.Equals(rule.Action1)).Probability;
                outputAction.ActionsToPerform.Add(actionToAdd);
            }
        }

        private void ApplyReinforces(Rule rule, OutputAction outputAction)
        {
            List<string> actionNames = outputAction.ActionsToPerform.Select(action => action.Name).ToList();
            if (actionNames.Contains(rule.Action1))
            {
                Action desiredAction = outputAction.ActionsToPerform.Find(action => action.Name.Equals(rule.Action2));
                desiredAction.Probability = Math.Min(1.0, desiredAction.Probability * 2);
            }
        }

        private void SetRawOutput(List<Action> intermediateResult, OutputAction outputAction)
        {
            List<double> probabilities = new List<double>();
            outputAction.RawOutput = new Dictionary<string, double[]>();
            string currentGroup = intermediateResult[0].GroupName;
            foreach (var action in intermediateResult)
            {
                if (!currentGroup.Equals(action.GroupName))
                {
                    outputAction.RawOutput.Add(currentGroup, probabilities.ToArray());
                    currentGroup = action.GroupName;
                    probabilities.Clear();
                }
                probabilities.Add(action.Probability);
            }
            outputAction.RawOutput.Add(currentGroup, probabilities.ToArray());
        }
    }
}