namespace Calculator.model
{
    /* Observer interface that observers of a calculation should implement if they want to be
     * notified whenever a calculation is changed. To register as an observer, use
     * Calculation.AddObserver. */
    interface ICalculationChangedObserver
    {
        /* Called whenever the calculation is changed. This means whenever a digit or decimal
         * separator is added to an operand, an operand changes sign, an operator is added
         * to the calculation, an operator is calculated/evaluated or the calculation is cleared. 
         */
        void OnCalculationChanged();
    }
}
