using Calculator.model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculator
{
    /* The central model of the application. A calculation is an abstraction of the ongoing
     * calculation that the user makes during interaction with the calculator. It consists of
     * a current operand, an operator and - for operators that require two operands - the previous
     * operand. Numbers, decimal separators and negative signs are part of operands, and
     * mathematical operators like plus, minus and square root are operators.
     * 
     * The core logic works like this: When the user enters a number, decimal separator or a
     * negative sign it is added to the current operand, with the operator at that time unknown.
     * If the user then enters a unary operator, like square root or power of two, the current
     * operand is finalized (converted into a number) and used as the argument for the unary
     * operator. The unary operator is immediately evaluated and the result stored in the current 
     * operand. If the user enters a binary operator like addition or multiplication, the current
     * operand is finalized and stored as the previous operand, which will later be used as the 
     * first argument to the operator. A new operand is then instanciated and set as the current
     * operand, and this operand will later be used as the second operand to the operator. The 
     * binary operator is then finally evaluated either when the user clicks the equal button, or
     * if they enter another operator.
     * 
     * The calculation also allows for registering observers that are notified whenever the
     * calculation is changed in any way, for example if an operator is appended to or if the
     * calculation is cleared. This is used by the main form in order to update the calculator
     * screen. */
    class Calculation
    {
        // Variables used as shorthands for culture dependent decimal separator and negative sign
        private static readonly string decimalSeparator =
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string negativeSign =
            CultureInfo.CurrentCulture.NumberFormat.NegativeSign;

        // List of observers to be notified whenever the calculation is changed.
        private readonly List<ICalculationChangedObserver> changeObservers;
        private readonly List<IArithmeticExceptionObserver> arithmeticExceptionObservers;

        private Operator _operator; // The current operator of the calculation

        /* The current operand of the calculation. Used as first and only argument to unary
         * operators and as the second argument to binary operators. Results are also stored in
         * this variable, because it simplifies the model and allows the user to easily append
         * numbers or decimal separators to - or change the sign of - the result. */
        private string currentOperand;

        /* The previous, finalized, operand. For binary operators, this is used as the first
         * argument. A nullable type is used to identify whether the value is unset. */
        private double? previousOperand;

        public Calculation()
        {
            currentOperand = "";
            changeObservers = new List<ICalculationChangedObserver>();
            arithmeticExceptionObservers = new List<IArithmeticExceptionObserver>();
        }

        private bool CanFinalize(string operand)
        {
            return operand.Length > 0 &&                    // Must be non-empty..
                   !operand.EndsWith(decimalSeparator) &&   // ..and not be unfinished decimal nr..
                   operand != negativeSign;                 // ..and not be unfinished negative nr.
        }

        // Adds 'digit' to the current operand and notifies the observers.
        public void AddDigit(int digit)
        {
            if (CanFinalize(currentOperand)) // Logic to prevent appending to NaN and Infinity operands
            {
                double finalized = double.Parse(currentOperand);

                if (!double.IsNaN(finalized) && !double.IsInfinity(finalized))
                {
                    currentOperand += digit;
                }
            }
            else
            {
                currentOperand += digit;
            }

            NotifyChangeObservers();
        }

        // Adds a decimal separator to the current operand and notifies the observers.
        public void AddDecimalSeparator()
        {
            if (currentOperand == "") // Be helpful and add omitted zeroes before decimal separator
            {
                currentOperand = $"0{decimalSeparator}";
            }
            else if (!currentOperand.Contains(decimalSeparator))
            {
                currentOperand += decimalSeparator;
            }

            NotifyChangeObservers();
        }

        /* Changes the sign (positive to negative and negative to positive) of the current operand
         * and notifies the observers */
        public void ChangeSign()
        {
            // Toggle sign depending on whether it contains a negative sign
            if (currentOperand.StartsWith(negativeSign))
            {
                currentOperand = currentOperand.Remove(0, 1);
            }
            else
            {
                currentOperand = currentOperand.Insert(0, negativeSign);
            }

            NotifyChangeObservers();
        }

        /* Called when AddOperator is called with a unary operator. Adds operator 'op' to the 
         * calculation if the current operand can be finalized. If the current operand cannot be
         * finalized - for example if it ends with a decimal separator, calling this method does
         * nothing.
         * 
         * If the calculation contains an existing operator when this method is called, that
         * operator is calculated/evaluated and the result is stored in the current operand. The
         * current operator is then replaced with 'op'.
         * 
         * Since 'op' is a unary operator and its operand is known at the time the operator is
         * added, it is immediately calculated and the result stored in the current operand.
         * 
         * Any observers are finally notified that the calculation has changed. */
        public void AddOperator(UnaryOperator op)
        {
            if (CanFinalize(currentOperand))
            {
                // Handle any existing operator if it exists
                if (_operator != null)
                {
                    if (Calculate())
                    {
                        _operator = op;
                        Calculate();
                    }
                }
                else
                {
                    _operator = op;
                    Calculate();
                }

                NotifyChangeObservers();
            }
        }

        /* Called when AddOperator is called with a binary operator. Adds operator 'op' to the 
         * calculation if the current operand can be finalized. If the current operand cannot be
         * finalized - for example if it ends with a decimal separator, calling this method does
         * nothing.
         * 
         * If the calculation contains an existing operator when this method is called, that
         * operator is calculated/evaluated and the result is stored in the current operand. The 
         * current operator is then replaced with 'op'.
         * 
         * Since 'op' is a binary operator and second operand is _not_ known at the time the
         * operator is added, its calculation is deferred until further input has been received
         * from the user.
         * 
         * Any observers are finally notified that the calculation has changed. */
        public void AddOperator(BinaryOperator op)
        {
            if (CanFinalize(currentOperand))
            {
                if (Calculate())
                {
                    _operator = op;

                    // Store the current operand and create a new one
                    previousOperand = double.Parse(currentOperand);
                    currentOperand = "";
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
                currentOperand = "";

                return false;
            }
            catch (OverflowException)
            {
                NotifyExceptionObserversOfOverflow();
                _operator = null;

                return false;
            }
            catch (NotFiniteNumberException)
            {
                NotifyExceptionObserverOfNegativeSquareRoot();
                _operator = null;

                return false;
            }
            finally
            {
                NotifyChangeObservers();
            }

            NotifyChangeObservers();

            return true;
        }

        /* Calculates or evaluates the current operator using the current and optionally the
         * previous operands as arguments. If there is no current operator, calling this method
         * does nothing.
         * 
         * The result of the calculation is stored in the current operand.
         * 
         * After calling this method, the current operator will be unset. Any observers will also
         * be notified that the calculation has changed. */
        private void Evaluate()
        {
            if (_operator != null && CanFinalize(currentOperand))
            {
                double result;

                /* Checking for an instance of an object is usually a code smell. However, since
                 * the operator needs to be called with different number of arguments depending on
                 * whether it's a unary or binary operator, I think this is okay in this case. */
                if (_operator is UnaryOperator)
                {
                    // Variables used for readability
                    double argument = double.Parse(currentOperand);

                    result = _operator.Calculate(argument);
                }
                else if (_operator is BinaryOperator)
                {
                    // Variables used for readability
                    double firstArgument = previousOperand.Value;
                    double secondArgument = double.Parse(currentOperand);

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
                    currentOperand = result.ToString();
                    _operator = null;
                }
            }
        }

        // Clears or resets the calculation and notifies any observers.
        public void Clear()
        {
            _operator = null;
            currentOperand = "";
            previousOperand = null;

            NotifyChangeObservers();
        }

        public override string ToString()
        {
            // If no operator has ben entered, the textual representation will just be the operand
            if (_operator == null)
            {
                return currentOperand;
            }
            /* Otherwise, it will be the previous operand, followed by the operator and finally the
             * current operand */
            else
            {
                return $"{previousOperand} {_operator} {currentOperand}";
            }
        }

        /* Adds or registers an object implementing the ICalculationChangedObserver interface to 
         * be notified whenever this calculation changes. */
        public void AddChangeObserver(ICalculationChangedObserver observer)
        {
            changeObservers.Add(observer);
        }

        public void AddArithmeticExceptionObserver(IArithmeticExceptionObserver observer)
        {
            arithmeticExceptionObservers.Add(observer);
        }

        // Notifies any observers that the calculation has changed.
        private void NotifyChangeObservers()
        {
            foreach (ICalculationChangedObserver obs in changeObservers)
            {
                obs.OnCalculationChanged();
            }
        }

        private void NotifyExceptionObserversOfDivideByZero()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnDivideByZeroException();
            }
        }

        private void NotifyExceptionObserversOfOverflow()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnOverflowException();
            }
        }

        private void NotifyExceptionObserverOfNegativeSquareRoot()
        {
            foreach (IArithmeticExceptionObserver observer in arithmeticExceptionObservers)
            {
                observer.OnNegativeSquareRootException();
            }
        }
    }
}
