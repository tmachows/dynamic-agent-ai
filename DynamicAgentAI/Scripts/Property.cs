﻿using System.Collections;
using System.Collections.Generic;

namespace DynamicAgentAI
{
    public class Property
    {
        public string Name { get; set; }
        public double Value { get; set; }

        public Property(string name, double value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}