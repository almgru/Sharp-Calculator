using System.Globalization;

namespace Calculator.model
{
    class Operand
    {
        private static readonly string decimalPoint = 
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private static readonly string negativeSign =
            CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
        
        private string digits;

        public bool CanFinalize => digits.Length > 0 && !digits.EndsWith(decimalPoint);

        public Operand()
        {
            digits = "";
        }

        public Operand(double startingValue)
        {
            digits = startingValue.ToString();
        }

        public void AddDigit(int digit)
        {
            digits += digit;
        }

        public void AddDecimalPoint()
        {
            if (!digits.Contains(decimalPoint) && digits.Length > 0)
            {
                digits += decimalPoint;
            }
        }

        public void ChangeSign()
        {
            if (digits.StartsWith(negativeSign))
            {
                digits = digits.Remove(0, 1);
            }
            else
            {
                digits = digits.Insert(0, negativeSign);
            }
        }

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
