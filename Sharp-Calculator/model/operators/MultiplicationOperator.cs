namespace Calculator.model.operators
{
    // Binary operator that multiplies two operands.
    class MultiplicationOperator : BinaryOperator
    {
        public override double Calculate(double operand1, double operand2)
        {
            return operand1 * operand2;
        }

        public override string ToString()
        {
            return "x";
        }
    }
}
