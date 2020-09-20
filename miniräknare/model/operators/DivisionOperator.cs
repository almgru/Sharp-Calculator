namespace Calculator.model.operators
{
    // Binary operator that divides the first operand with the second operand.
    class DivisionOperator : BinaryOperator
    {
        public override double Calculate(double operand1, double operand2)
        {
            return operand1 / operand2;
        }

        public override string ToString()
        {
            return "/";
        }
    }
}
