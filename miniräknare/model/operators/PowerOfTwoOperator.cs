using System;

namespace Calculator.model.operators
{
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
