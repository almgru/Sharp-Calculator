using System;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        private CalculatorOperator calcOperator;
        private CalculatorOperand currentOperand;
        private double firstOperand;
        private double secondOperand;

        public CalculatorForm()
        {
            InitializeComponent();
            currentOperand = new CalculatorOperand();
        }

        private void UpdateScreen()
        {
            if (calcOperator == CalculatorOperator.None)
            {
                TextBoxScreen.Text = currentOperand.ToString();
            } else
            {
                TextBoxScreen.Text =
                    $"{firstOperand} {CalculatorOperatorToString(calcOperator)} {currentOperand}";
            }
        }

        private void EvaluateCurrentOperation()
        {
            if (calcOperator != CalculatorOperator.None)
            {
                // Operator requires two arguments?
                if (calcOperator != CalculatorOperator.Exponentation &&
                    calcOperator != CalculatorOperator.RootExtraction)
                {
                    secondOperand = currentOperand.Finalize();
                }

                firstOperand =
                    CalculateResult(calcOperator, firstOperand, secondOperand);

                calcOperator = CalculatorOperator.None;
                currentOperand = new CalculatorOperand(firstOperand);
            }

            UpdateScreen();
        }

        private double CalculateResult(CalculatorOperator op,
                                     double firstOperand,
                                     double secondOperand)
        {
            switch (op)
            {
                case CalculatorOperator.None:
                    return firstOperand;

                case CalculatorOperator.Addition:
                    return firstOperand + secondOperand;

                case CalculatorOperator.Subtraction:
                    return firstOperand - secondOperand;

                case CalculatorOperator.Multiplication:
                    return firstOperand * secondOperand;

                case CalculatorOperator.Division:
                    return firstOperand / secondOperand;

                case CalculatorOperator.Exponentation:
                    return Math.Pow(firstOperand, 2);

                case CalculatorOperator.RootExtraction:
                    return Math.Sqrt(firstOperand);

                default:
                    throw new ArgumentException("Unknown operator.");
            }
        }

        private string CalculatorOperatorToString(CalculatorOperator op)
        {
            switch (op)
            {
                case CalculatorOperator.None:
                    return "";

                case CalculatorOperator.Addition:
                    return "+";

                case CalculatorOperator.Subtraction:
                    return "-";

                case CalculatorOperator.Multiplication:
                    return "x";

                case CalculatorOperator.Division:
                    return "/";

                case CalculatorOperator.Exponentation:
                    return "^";

                case CalculatorOperator.RootExtraction:
                    return "sqrt";

                default:
                    throw new ArgumentException("Unknown operator.");
            }
        }

        private void Button0_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(0);
            UpdateScreen();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(1);
            UpdateScreen();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(2);
            UpdateScreen();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(3);
            UpdateScreen();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(4);
            UpdateScreen();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(5);
            UpdateScreen();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(6);
            UpdateScreen();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(7);
            UpdateScreen();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(8);
            UpdateScreen();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            currentOperand.AddDigit(9);
            UpdateScreen();
        }

        private void ButtonDecimalPoint_Click(object sender, EventArgs e)
        {
            currentOperand.AddDecimalPoint();
            UpdateScreen();
        }

        private void ButtonChangeSign_Click(object sender, EventArgs e)
        {
            currentOperand.ChangeSign();
            UpdateScreen();
        }

        private void ButtonPlus_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            } else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.Addition;
            currentOperand = new CalculatorOperand();
            UpdateScreen();
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            EvaluateCurrentOperation();
        }

        private void ButtonSubtract_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            }
            else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.Subtraction;
            currentOperand = new CalculatorOperand();
            UpdateScreen();
        }

        private void ButtonMultiply_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            }
            else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.Multiplication;
            currentOperand = new CalculatorOperand();
            UpdateScreen();
        }

        private void ButtonDivide_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            }
            else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.Division;
            currentOperand = new CalculatorOperand();
            UpdateScreen();
        }

        private void ButtonRootExtraction_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            }
            else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.RootExtraction;
            currentOperand = new CalculatorOperand();
            EvaluateCurrentOperation();
            UpdateScreen();
        }

        private void ButtonExponentiation_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand.Finalize();
            }
            else
            {
                EvaluateCurrentOperation();
            }

            calcOperator = CalculatorOperator.Exponentation;
            currentOperand = new CalculatorOperand();
            EvaluateCurrentOperation();
            UpdateScreen();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            calcOperator = CalculatorOperator.None;
            currentOperand = new CalculatorOperand();
            UpdateScreen();
        }
    }

    class CalculatorOperand
    {
        private string digits;

        public CalculatorOperand() 
        {
            digits = "";
        }

        public CalculatorOperand(double startingValue)
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
            } else
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

    enum CalculatorOperator
    {
        None,
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Exponentation,
        RootExtraction
    }
}
