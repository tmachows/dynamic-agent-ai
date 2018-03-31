using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public class InputGroup
    {
        public string Name { get; set; }
        public List<string> PossibleActions { get; set; }
        public List<Property> Properties { get; set; }
    }
}