using Calculator.model;
using System;
using System.Collections.Generic;

namespace Calculator
{
    /* The central model of the application. A calculation is an abstraction of the ongoing
     * calculation that the user makes during interaction with the calculator. It consists of
     * a current operand, an operator and, for operatings that require two operands, the previous
     * operand. Numbers, decimal separators and negative signs are part of operands, and
     * mathematical operators like plus, minus and square root are operators.
     * 
     * The core logic works as follows: When the user enters a number, decimal separator or a
     * negative sign it is appended to the current operand, with the operator at that time unknown.
     * If the user then enters a unary operator, like square root or power of two, the current
     * operand is finalized (converted into a number) and the unary operator is immediately
     * evaluated and the result stored in the current operand. If the user enters a binary operator
     * like addition or multiplication, the current operand is finalized and stored as the previous
     * operand, which will later be used as the first argument to the operator. A new operand is
     * then instanciated and set as the current operand, and this operand will later be used as
     * the second operand to the operator. The binary operator is then finally evaluated either
     * when the user clicks the equal button, or if they enter another operator.
     * 
     * The calculation also allows for registering observers that are notified whenever the
     * calculation is changed in any way, for example if an operator is appended to or if the
     * calculation is cleared. This is used by the main form in order to update the calculator
     * screen. */
    class Calculation
    {
        // List of observers to be notified whenever the calculation is changed.
        private readonly List<ICalculationChangedObserver> changeObservers;

        // The current operator of the calculation
        private Operator _operator;

        /* The current operand of the calculation. Used as first and only argument to unary
         * operators and as the second argument to binary operators. Results are also stored in
         * this variable, because it simplifies the model and allows the user to easily append
         * numbers or decimal separators to - or change the sign of - the result. */
        private Operand operand;

        /* The previous, finalized, operand. For binary operators, this is used as the first
         * argument. */
        private double? previousOperand;

        public Calculation()
        {
            operand = new Operand();
            changeObservers = new List<ICalculationChangedObserver>();
        }

        // Adds 'digit' to the current operand and notifies the observers.
        public void AddDigit(int digit)
        {
            operand.AddDigit(digit);
            NotifyObservers();
        }

        // Adds a decimal separator to the current operand and notifies the observers.
        public void AddDecimalPoint()
        {
            operand.AddDecimalPoint();
            NotifyObservers();
        }

        /* Changes the sign (positive to negative and negative to positive) of the current operand
         * and notifies the observers */
        public void ChangeSign()
        {
            operand.ChangeSign();
            NotifyObservers();
        }

        /* Adds an operator 'op' to the calculation if the current operand can be finalized. If the
         * current operand cannot be finalized - for example if it ends with a decimal separator -
         * calling this method does nothing.
         * 
         * If the calculation contains an existing operator when this method is called, that
         * operator is calculated/evaluated with the current operand and optionally the previous
         * operand, and the result is stored in the current operand. The current operator is then
         * replaced with 'op'.
         * 
         * If 'op' is a unary operator, it is immediately calculated and the result stored in the
         * current operand.
         * 
         * Any observers are finally notified that the calculation has changed. */
        public void AddOperator(UnaryOperator op)
        {
            /* Do nothing if the current operand cannot be finalized, and therefore cannot be used
             * as an argument/operand to the operator */
            if (!operand.CanFinalize) { return; }

            // Handle any existing operator if it exists
            if (_operator != null)
            {
                Calculate();
            }

            _operator = op;
            Calculate();

            NotifyObservers();
        }

        public void AddOperator(BinaryOperator op)
        {
            /* Do nothing if the current operand cannot be finalized, and therefore cannot be used
             * as an argument/operand to the operator */
            if (!operand.CanFinalize) { return; }

            // Handle any existing operator if it exists
            if (_operator != null)
            {
                Calculate();
            }

            _operator = op;

            // Store the current operand and create a new one
            previousOperand = operand.Finalize();
            operand = new Operand();

            NotifyObservers();
        }

        /* Calculates or evaluates the current operator using the current and optionally the
         * previous operands as arguments. If there is no current operator, calling this method
         * does nothing.
         * 
         * The result of the calculation is stored in the current operand.
         * 
         * After calling this method, the current operator will be unset. Any observers will also
         * be notified that the calculation has changed.
         */
        public void Calculate()
        {
            if (_operator != null)
            {
                double result;

                /* Checking for an instance of an object is usually a code smell. However, since
                 * the operator needs to be called with different number of arguments depending on
                 * whether it's a unary or binary operator, I think this is okay in this case. */
                if (_operator is UnaryOperator)
                {
                    result = _operator.Calculate(operand.Finalize());
                }
                else if (_operator is BinaryOperator)
                {
                    result = _operator.Calculate(previousOperand.Value, operand.Finalize());
                } else
                {
                    throw new InvalidOperationException("Operator has an invalid state.");
                }

                operand = new Operand(result);
                _operator = null;
                NotifyObservers();
            }
        }

        // Clears or resets the calculation and notifies any observers.
        public void Clear()
        {
            _operator = null;
            operand = new Operand();
            previousOperand = null;
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

        /* Adds or registers an object implementing the ICalculationChangedObserver interface to 
         * be notified whenever this calculation changes. */
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
