using System;

namespace Calculator.model
{
    public abstract class UnaryOperator : Operator
    {
        public abstract override double Calculate(double operand);

        public override double Calculate(double operand1, double operand2)
        {
            throw new ArgumentException("A unary operator expects only one operand.");
        }
    }
}
