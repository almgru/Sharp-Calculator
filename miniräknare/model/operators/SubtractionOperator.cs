namespace Calculator.model.operators
{
    // Binary operator that subtracts the second operand from the first operand.
    class SubtractionOperator : BinaryOperator
    {
        public override double Calculate(double operand1, double operand2)
        {
            return operand1 - operand2;
        }

        public override string ToString()
        {
            return "-";
        }
    }
}
