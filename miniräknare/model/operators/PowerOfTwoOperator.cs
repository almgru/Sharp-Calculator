using System;

namespace Calculator.model.operators
{
    // Unary operator that raises the operand to the power of 2.
    class PowerOfTwoOperator : UnaryOperator
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
