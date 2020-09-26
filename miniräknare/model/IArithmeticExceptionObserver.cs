namespace Calculator.model
{
    /* Observer interface that observers of a calculation should implement if they want to be
     * notified whenever a calculation results in an arithmetic exception. To register as an
     * observer, use Calculation.AddObserver. */
    interface IArithmeticExceptionObserver
    {
        // Called when the second operand to a divide operation is zero.
        void OnDivideByZeroException();

        // Called when the result of the calculation is infinity or negative infinity.
        void OnOverflowException();

        // Called when the operand to a square root operation is negative.
        void OnNegativeSquareRootException();
    }
}
