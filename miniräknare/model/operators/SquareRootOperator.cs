using System;

namespace Calculator.model.operators
{
    class SquareRootOperator : UnaryOperator
    {
        public override double Calculate(double operand)
        {
            return Math.Sqrt(operand);
        }

        public override string ToString()
        {
            return "sqrt";
        }
    }
}
