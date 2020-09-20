using System;

namespace Calculator.model
{
    public abstract class BinaryOperator : Operator
    {
        public override double Calculate(double operand)
        {
            throw new ArgumentException("A binary operator expects two operands.");
        }

        public abstract override double Calculate(double operand1, double operand2);
    }
}
