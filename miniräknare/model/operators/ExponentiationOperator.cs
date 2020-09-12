using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.model.operators
{
    class ExponentiationOperator : IOperator
    {
        public int ExpectedOperandsCount => 1;

        public double Calculate(ICollection<double> operands)
        {
            return Math.Pow(operands.ElementAt(0), 2);
        }

        public override string ToString()
        {
            return "^";
        }
    }
}
