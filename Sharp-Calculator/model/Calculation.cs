using System;
using System.Collections.Generic;

using Calculator.model;

namespace Calculator
{
    /* A calculation is an abstraction of the ongoing calculation that the user makes during
     * interaction with the calculator. It consists of a current operand, an operator and - for
     * operators that require two operands - the previous operand. Numbers, decimal separators and
     * negative signs are part of operands, and mathematical operators like plus, minus and square
     * root are operators.
     * 
     * The core logic works like this: When the user enters a number, decimal separator or a
     * negative sign, it is added to the current operand, with the operator at that time unknown. If
     * the user then enters a unary operator, like square root or power of two, the current operand
     * is parsed into a number and used as the argument for the unary operator. The unary operator
     * is immediately evaluated and the result stored in the current operand. If the user enters a
     * binary operator like addition or multiplication, the current operand is parsed and stored as
     * the previous operand, which will later be used as the first argument to the operator. A new
     * operand is then created and set as the current operand, and this operand will later be used
     * as the second operand to the operator. The binary operator is then finally evaluated either
     * when the user clicks the equal button, or if they enter another operator.
     * 
     * The calculation also allows for registering observers that are notified whenever 1) the
     * calculation is changed in any way, for example if an operator is appended to or if the
     * calculation is cleared, or 2) an arithmetic exception occurs. This is used by the main form
     * in order to update the calculator screen and show error messages. */
    class Calculation
    {
        private readonly List<ICalculationChangedObserver> changeObservers;
        private readonly List<IArithmeticExceptionObserver> arithmeticExceptionObservers;

        private Operator _operator; // The current operator of the calculation

        /* The current operand of the calculation. Used as first and only argument to unary
         * operators and as the second argument to binary operators. Results are also stored in this
         * variable, because it simplifies the model and allows the user to easily append numbers or
         * decimal separators to - or change the sign of - the result. */
        private Operand operand;

        /* The previous, parsed, operand. For binary operators, this is used as the first argument.
         * A nullable type is used to identify whether the value is unset. */
        private double? previousOperand;

        public Calculation()
        {
            operand = new Operand();
            changeObservers = new List<ICalculationChangedObserver>();
            arithmeticExceptionObservers = new List<IArithmeticExceptionObserver>();
        }

        // Adds 'digit' to the current operand and notifies the change observers.
        public void AddDigit(int digit)
        {
            operand.AddDigit(digit);

            NotifyChangeObservers();
        }

        // Adds a decimal separator to the current operand and notifies the change observers.
        public void AddDecimalSeparator()
        {
            operand.AddDecimalSeparator();

            NotifyChangeObservers();
        }

        /* Changes the sign (positive to negative and negative to positive) of the current operand
         * and notifies the change observers */
        public void ChangeSign()
        {
            operand.ChangeSign();

            NotifyChangeObservers();
        }

        /* Called when AddOperator is called with a unary operator. Adds operator 'op' to the 
         * calculation if the current operand can be parsed to a number. If the current operand
         * cannot be parsed - for example if it ends with a decimal separator, calling this method
         * does nothing.
         * 
         * If the calculation contains an existing operator when this method is called, that
         * operator is calculated/evaluated and the result is stored in the current operand. The
         * current operator is then replaced with 'op'.
         * 
         * Since 'op' is a unary operator and its operand is known at the time the operator is
         * added, it is immediately calculated and the result stored in the current operand.
         * 
         * Any change observers are finally notified that the calculation has changed. If an
         * arithemtic exception occurs, any previous operator is not replaced, the result is not
         * updated and any exception observers are notified.*/
        public void AddOperator(UnaryOperator op)
        {
            if (operand.CanParse)
            {
                /* If an existing operator exists, calculate it and only update the operator and
                 * calculate the new result if the previous operator is calculated successfully. */
                if (_operator == null || (_operator != null && Calculate()))
                {
                    _operator = op;
                    Calculate();
                }

                NotifyChangeObservers();
            }
        }

        /* Called when AddOperator is called with a binary operator. Adds operator 'op' to the 
         * calculation if the current operand can be parsed into a number. If the current operand
         * cannot be parsed - for example if it ends with a decimal separator, calling this method
         * does nothing.
         * 
         * If the calculation contains an existing operator when this method is called, that
         * operator is calculated/evaluated and the result is stored in the current operand. The 
         * current operator is then replaced with 'op'.
         * 
         * Since 'op' is a binary operator and second operand is _not_ known at the time the
         * operator is added, its calculation is deferred until further input has been received
         * from the user.
         * 
         * Any change observers are finally notified that the calculation has changed. If an
         * arithemtic exception occurs, any previous operator is not replaced, the result is not
         * updated and any exception observers are notified.*/
        public void AddOperator(BinaryOperator op)
        {
            if (operand.CanParse)
            {
                if (_operator == null || (_operator != null && Calculate()))
                {
                    _operator = op;

                    // Store the current operand and create a new one
                    previousOperand = operand.Parse();
                    operand = new Operand();
                }

                NotifyChangeObservers();
            }
        }

        /* Publicly exposed wrapper around Evaluate that handles exceptions.
         * 
         * Returns a bool indicating if it was possible to perform the calculation. */
        public bool Calculate()
        {
            try
            {
                Evaluate();
            }
            catch (DivideByZeroException)
            {
                NotifyExceptionObserversOfDivideByZero();

                return false;
            }
            catch (OverflowException)
            {
                NotifyExceptionObserversOfOverflow();

                return false;
            }
            catch (NotFiniteNumberException)
            {
                NotifyExceptionObserverOfNegativeSquareRoot();

                return false;
            }
            finally
            {
                ResetAfterException();
                NotifyChangeObservers();
            }

            NotifyChangeObservers();

            return true;
        }

        // Clears or resets the calculation and notifies any change observers.
        public void Clear()
        {
            _operator = null;
            operand = new Operand();
            previousOperand = null;

            NotifyChangeObservers();
        }

        public override string ToString()
        {
            // If no operator has ben entered, the textual representation will just be the operand
            if (_operator == null)
            {
                return operand.ToString();
            }
            /* Otherwise, it will be the previous operand, followed by the operator and finally the
             * current operand */
            else
            {
                return $"{previousOperand} {_operator} {operand}";
            }
        }

        /* Adds or registers an object implementing the ICalculationChangedObserver interface to 
         * be notified whenever this calculation changes. */
        public void AddChangeObserver(ICalculationChangedObserver observer)
        {
            changeObservers.Add(observer);
        }

        /* Adds or registers an object implementing the IArithmeticExceptionObserver interface to 
         * be notified whenever the calculation results in an arithmetic exceptions. */
        public void AddArithmeticExceptionObserver(IArithmeticExceptionObserver observer)
        {
            arithmeticExceptionObservers.Add(observer);
        }

        /* Evaluates the current operator using the current and optionally the previous operands as
         * arguments. If there is no current operator, calling this method does nothing.
         * 
         * The result of the calculation is stored in the current operand.
         * 
         * After calling this method, the current operator will be unset. Any observers will also
         * be notified that the calculation has changed. */
        private void Evaluate()
        {
            if (_operator != null && operand.CanParse)
            {
                double result;

                if (_operator is UnaryOperator)
                {
                    double argument = operand.Parse();

                    result = _operator.Calculate(argument);
                }
                else if (_operator is BinaryOperator)
                {
                    double firstArgument = previousOperand.Value;
                    double secondArgument = operand.Parse();

                    result = _operator.Calculate(firstArgument, secondArgument);
                }
                else
                {
                    throw new InvalidOperationException("Operator has an invalid state.");
                }

                /* Overflow is checked here instead of in the operator classes to avoid duplicate
                 * code. */
                if (double.IsInfinity(result))
                {
                    throw new OverflowException();
                }
                else
                {
                    // Create a new operand with the result of the calculation as a starting point
                    operand = new Operand(result);
                    _operator = null;
                }
            }
        }

        /* Resets the calculation to a state that makes sense after an arithmetic exception has 
         * occurred. */
        private void ResetAfterException()
        {
            if (_operator is UnaryOperator)
            {
                _operator = null;
            }
            else if (_operator is BinaryOperator)
            {
                operand = new Operand();
            }
        }

        // Notifies any change observers that the calculation has changed.
        private void NotifyChangeObservers()
        {
            foreach (ICalculationChangedObserver obs in changeObservers)
            {
                obs.OnCalculationChanged();
            }
        }

        // Notifies any exception observers that a divide by zero exception has occurred.
        private void NotifyExceptionObserversOfDivideByZero()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnDivideByZeroException();
            }
        }

        // Notifies any exception observers that an overflow exception has occurred.
        private void NotifyExceptionObserversOfOverflow()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnOverflowException();
            }
        }

        /* Notifies any exception observers that a negative square root operand exception has
         * occurred. */
        private void NotifyExceptionObserverOfNegativeSquareRoot()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnNegativeSquareRootException();
            }
        }
    }
}
