using System;

namespace Calculator.model
{
    /* An operator that takes two operand. If the overridden method Calculate is called with 
     * a single argument, an ArgumentException exception is raised. */
    public abstract class BinaryOperator : Operator
    {
        public override double Calculate(double operand)
        {
            throw new ArgumentException("A binary operator expects two operands.");
        }

        public abstract override double Calculate(double operand1, double operand2);
    }
}
