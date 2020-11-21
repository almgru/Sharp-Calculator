using System;

namespace Calculator.model.operators
{
    // Unary operator that takes the square root of an operand.
    class SquareRootOperator : UnaryOperator
    {
        public override double Calculate(double operand)
        {
            if (operand < 0)
            {
                throw new NotFiniteNumberException();
            }

            return Math.Sqrt(operand);
        }

        public override string ToString()
        {
            return "sqrt";
        }
    }
}
