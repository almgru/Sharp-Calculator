using Calculator.model;
using System.Collections.Generic;

namespace Calculator
{
    class Calculation
    {
        private IOperator _operator;
        private Operand operand;
        private double previousOperand;

        public Calculation()
        {
            operand = new Operand();
        }

        public void AddDigit(int digit)
        {
            operand.AddDigit(digit);
        }

        public void AddDecimalPoint()
        {
            operand.AddDecimalPoint();
        }

        public void ChangeSign()
        {
            operand.ChangeSign();
        }

        public void AddOperator(IOperator op)
        {
            if (_operator != null)
            {
                Calculate();
            }

            _operator = op;

            if (_operator.ExpectedOperandsCount == 1)
            {
                Calculate();
            } else
            {
                previousOperand = operand.Finalize();
                operand = new Operand();
            }
        }

        public void Calculate()
        {
            if (_operator != null)
            {
                List<double> operands = new List<double> { previousOperand };

                // Operator requires two arguments?
                if (_operator.ExpectedOperandsCount == 2)
                {
                    operands.Add(operand.Finalize());
                }

                operand = new Operand(_operator.Calculate(operands));
                _operator = null;
            }
        }

        public override string ToString()
        {
            if (_operator == null)
            {
                return operand.ToString();
            }
            else
            {
                return
                    $"{previousOperand} {_operator} {operand}";
            }
        }
    }
}
