using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Parsing.ExpressionParsing
{
    public enum Associativity
    {
        Left,
        Right
    }

    public class PrecedenceAssociativity
    {
        public int Weight { get; }
        public Associativity Associativity { get; }

        public PrecedenceAssociativity(int weight, Associativity associativity)
        {
            Weight = weight;
            Associativity = associativity;
        }
    }
}
