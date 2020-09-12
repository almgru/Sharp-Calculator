using Calculator.model;
using System.Collections.Generic;

namespace Calculator
{
    class Calculation
    {
        private IOperator _operator;
        private Operand operand;
        private double previousOperand;
        private List<ICalculationChangedObserver> changeObservers;

        public Calculation()
        {
            operand = new Operand();
            changeObservers = new List<ICalculationChangedObserver>();
        }

        public void AddDigit(int digit)
        {
            operand.AddDigit(digit);
            NotifyObservers();
        }

        public void AddDecimalPoint()
        {
            operand.AddDecimalPoint();
            NotifyObservers();
        }

        public void ChangeSign()
        {
            operand.ChangeSign();
            NotifyObservers();
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

            NotifyObservers();
        }

        public void Calculate()
        {
            if (_operator != null)
            {
                List<double> operands = new List<double>();

                // Operator requires two arguments?
                if (_operator.ExpectedOperandsCount == 2)
                {
                    operands.Add(previousOperand);
                }
                
                operands.Add(operand.Finalize());
                operand = new Operand(_operator.Calculate(operands));
                _operator = null;

                NotifyObservers();
            }
        }

        public void Clear()
        {
            _operator = null;
            operand = new Operand();
            previousOperand = 0;
            NotifyObservers();
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

        public void AddChangeObserver(ICalculationChangedObserver observer)
        {
            changeObservers.Add(observer);
        }

        private void NotifyObservers()
        {
            foreach (ICalculationChangedObserver obs in changeObservers)
            {
                obs.OnCalculationChanged();
            }
        }
    }
}
