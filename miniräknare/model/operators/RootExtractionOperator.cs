using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.model.operators
{
    class RootExtractionOperator : IOperator
    {
        public int ExpectedOperandsCount => 1;

        public double Calculate(ICollection<double> operands)
        {
            return Math.Sqrt(operands.ElementAt(0));
        }

        public override string ToString()
        {
            return "sqrt";
        }
    }
}
