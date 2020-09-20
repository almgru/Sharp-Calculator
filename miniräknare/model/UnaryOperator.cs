using System;

namespace Calculator.model
{
    /* An operator that takes a single operand. If the overridden method Calculate is called with 
     * two arguments, an ArgumentException exception is raised. */
    public abstract class UnaryOperator : Operator
    {
        public abstract override double Calculate(double operand);

        public override double Calculate(double operand1, double operand2)
        {
            throw new ArgumentException("A unary operator expects only one operand.");
        }
    }
}
