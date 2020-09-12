namespace Calculator.model
{
    class Operand
    {
        private string digits;

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
            if (!digits.EndsWith("."))
            {
                digits += ".";
            }
        }

        public void ChangeSign()
        {
            if (digits.StartsWith("-"))
            {
                digits = digits.Remove(0, 1);
            }
            else
            {
                digits = digits.Insert(0, "-");
            }
        }

        public double Finalize()
        {
            if (digits.EndsWith("."))
            {
                digits += "0";
            }

            return double.Parse(digits.ToString());
        }

        public override string ToString()
        {
            return digits;
        }
    }
}
