using System.Collections.Generic;

namespace Calculator.model
{
    /* An operator is a mathematical function like addition or multiplication and is part of a 
     * calculation. It can be either a unary or binary operator, i.e. take one or two operands
     * respectively. */
    public abstract class Operator
    {
        /* Calculates or evaluates the operator given a list of operands.
         * 
         * The reason that the operands is a collection instead of parameters is that I wanted a
         * generic interface for both unary and binary operators, and you cannot define a function
         */
        public abstract double Calculate(double operand);
        public abstract double Calculate(double operand1, double operand2);
    }
}
