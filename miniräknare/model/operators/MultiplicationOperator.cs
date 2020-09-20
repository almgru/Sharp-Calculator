namespace Calculator.model.operators
{
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
