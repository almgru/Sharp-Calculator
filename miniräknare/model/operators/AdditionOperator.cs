using System.Collections.Generic;
using System.Linq;

namespace Calculator.model.operators
{
    class AdditionOperator : IOperator
    {
        public int ExpectedOperandsCount => 2;

        public double Calculate(ICollection<double> operands)
        {
            return operands.ElementAt(0) + operands.ElementAt(1);
        }

        public override string ToString()
        {
            return "+";
        }
    }
}
