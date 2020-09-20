namespace Calculator.model
{
    /* An operator is a mathematical function like addition or multiplication and is part of a 
     * calculation. It can be either a unary or binary operator, see abstract classes
     * UnaryOperator and BinaryOperator. 
     * 
     * The class has two Calculate functions which evaluates the operator given operands, one that
     * takes a single argument and one that takes two arguments. UnaryOperator implements the
     * method that takes one argument and BinaryOperator implements the one that takes two
     * arguments.
     * 
     * Since a non-abstract class has to implement all abstract methods of its parent, and since we
     * do not want UnaryOperator to accept two arguments or BinaryOperator to accept one argument,
     * we raise an exception if the wrong method is called (see UnaryOperator and BinaryOperator.
     * There is probably ways to avoid having to do this but I couldn't think of any. */
    public abstract class Operator
    {
        /* Calculates or evaluates the operator given an operand. This is implemented by
         * UnaryOperator. */
        public abstract double Calculate(double operand);

        /* Calculates or evaluates the operator given two operands. This is implemented by
         * BinaryOperator. */
        public abstract double Calculate(double operand1, double operand2);
    }
}
