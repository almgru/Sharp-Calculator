using System.Collections.Generic;

namespace Calculator.model
{
    interface IOperator
    {
        int ExpectedOperandsCount { get; }

        double Calculate(ICollection<double> operands);
    }
}
