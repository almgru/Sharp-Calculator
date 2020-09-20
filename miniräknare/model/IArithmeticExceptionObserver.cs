using System;

namespace Calculator.model
{
    interface IArithmeticExceptionObserver
    {
        void OnDivideByZeroException();
        void OnOverflowException();
    }
}
