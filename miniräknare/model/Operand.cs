using System.Globalization;

namespace Calculator.model
{
    /* An operand is an instance of an argument to an operator. It is stored as a string
     * until it can be finalized into a double. */
    class Operand
    {
        // Variables used as shorthands for culture dependent decimal separator and negative sign
        private static readonly string decimalSeparator = 
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string negativeSign =
            CultureInfo.CurrentCulture.NumberFormat.NegativeSign;


        private string digits;

        // Checks whether the operand can be finalized (converted to double)
        public bool CanFinalize => (
            digits.Length > 0 &&                    // Must be non-empty..
            !digits.EndsWith(decimalSeparator) &&   // ..and not be unfinished decimal nr..
            digits != negativeSign                  // ..and not be unfinished negative nr.
        );

        public Operand()
        {
            digits = "";
        }

        // Alternate constructor which sets a starting value for the operand.
        public Operand(double startingValue)
        {
            digits = startingValue.ToString();
        }

        public void AddDigit(int digit)
        {
            /* Do not add digit if current operand is large enough to be represented with scientific
             * notation, since it's unclear where the number should be appended. */
            if (!digits.Contains("E"))
            {
                digits += digit;
            }
        }

        public void AddDecimalSeparator()
        {
            if (!digits.Contains("E"))
            {
                // Be helpful and add omitted zeroes before decimal separator
                if (digits == "" || digits == negativeSign)
                {
                    digits += "0";
                }


                if (!digits.Contains(decimalSeparator))
                {
                    digits += decimalSeparator;
                }
            }
        }

        public void ChangeSign()
        {
            // Toggle sign depending on whether it contains a negative sign
            if (digits.StartsWith(negativeSign))
            {
                digits = digits.Remove(0, 1);
            }
            else
            {
                digits = digits.Insert(0, negativeSign);
            }
        }

        // Convert the operand to a double
        public double Finalize()
        {
            return double.Parse(digits.ToString());
        }

        public override string ToString()
        {
            return digits;
        }
    }
}
