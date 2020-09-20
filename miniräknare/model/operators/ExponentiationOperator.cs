using System;

namespace Calculator.model.operators
{
    class ExponentiationOperator : UnaryOperator
    {
        public override double Calculate(double operand)
        {
            return Math.Pow(operand, 2);
        }

        public override string ToString()
        {
            return "^";
        }
    }
}
