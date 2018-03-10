using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Interpreter
{
    public OutputAction ApplyRules(List<Action> intermediateResult, List<Rule> rules)
    {
        OutputAction outputAction = new OutputAction();
        outputAction.ActionsToPerform.AddRange(intermediateResult);
        // todo set intermediate result on detailed output
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
                case Relation.PRIORITIZES:
                    ApplyPrioritizes(rule, outputAction);
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
        if (actionNames.Contains(rule.action1))
        {
            outputAction.ActionsToPerform.RemoveAll(action => action.Name.Equals(rule.action2));
        }
    }

    private void ApplyForces(Rule rule, OutputAction outputAction)
    {
        List<string> actionNames = outputAction.ActionsToPerform.Select(action => action.Name).ToList();
        if (actionNames.Contains(rule.action1) && !actionNames.Contains(rule.action2))
        {
            Action actionToAdd = new Action();
            actionToAdd.Name = rule.action2;
            actionToAdd.Probability =
                outputAction.ActionsToPerform.Find(action => action.Name.Equals(rule.action1)).Probability;
            outputAction.ActionsToPerform.Add(actionToAdd);
        }
    }

    private void ApplyPrioritizes(Rule rule, OutputAction outputAction)
    {
        throw new NotImplementedException();
    }
}
