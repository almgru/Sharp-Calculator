using System;

namespace Calculator.model
{
    interface IArithmeticExceptionObserver
    {
        void OnDivideByZero();
        void OnOverflow();
        void OnNegativeSquareRoot();
    }
}
