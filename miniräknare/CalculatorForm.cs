using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        private CalculatorOperator calcOperator;
        private string currentOperand;
        private string firstOperand;
        private string secondOperand;

        public CalculatorForm()
        {
            InitializeComponent();
        }

        private void UpdateScreen()
        {
            if (calcOperator == CalculatorOperator.None)
            {
                TextBoxScreen.Text = currentOperand;
            } else
            {
                TextBoxScreen.Text =
                    $"{firstOperand} {CalculatorOperatorToString(calcOperator)} {currentOperand}";
            }
        }

        private void PerformCalculatorOperation()
        {
            if (calcOperator != CalculatorOperator.None)
            {
                secondOperand = currentOperand;

                firstOperand =
                    CalculateResult(calcOperator, firstOperand, secondOperand)
                    .ToString();

                calcOperator = CalculatorOperator.None;
                currentOperand = firstOperand;
                secondOperand = null;
            }

            UpdateScreen();
        }

        private double CalculateResult(CalculatorOperator op,
                                     string firstOperand,
                                     string secondOperand)
        {
            switch (op)
            {
                case CalculatorOperator.None:
                    return double.Parse(firstOperand);

                case CalculatorOperator.Addition:
                    return double.Parse(firstOperand) + double.Parse(secondOperand);

                case CalculatorOperator.Subtraction:
                    return double.Parse(firstOperand) - double.Parse(secondOperand);

                case CalculatorOperator.Multiplication:
                    return double.Parse(firstOperand) * double.Parse(secondOperand);

                case CalculatorOperator.Division:
                    return double.Parse(firstOperand) / double.Parse(secondOperand);

                case CalculatorOperator.Exponentation:
                    return Math.Pow(double.Parse(firstOperand), 2);

                case CalculatorOperator.RootExtraction:
                    return Math.Sqrt(double.Parse(firstOperand));

                default:
                    throw new ArgumentException("Unknown operator.");
            }
        }

        private void AddDigitToCurrentOperand(string digit)
        {
            if (currentOperand == null)
            {
                currentOperand = "";
            }

            currentOperand += digit;
            UpdateScreen();
        }

        private void AddDecimalPointToCurrentOperand()
        {
            if (currentOperand == null)
            {
                currentOperand = "";
            }

            currentOperand += ".";
            UpdateScreen();
        }

        private void ChangeCurrentOperandSign()
        {
            if (currentOperand == null)
            {
                currentOperand = "";
            }

            if (currentOperand.StartsWith("-"))
            {
                currentOperand = currentOperand.Remove(0, 1);
            } else
            {
                currentOperand = currentOperand.Insert(0, "-");
            }

            UpdateScreen();
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
            AddDigitToCurrentOperand("0");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("1");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("2");
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("3");
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("4");
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("5");
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("6");
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("7");
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("8");
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            AddDigitToCurrentOperand("9");
        }

        private void ButtonDecimalPoint_Click(object sender, EventArgs e)
        {
            AddDecimalPointToCurrentOperand();
        }

        private void ButtonChangeSign_Click(object sender, EventArgs e)
        {
            ChangeCurrentOperandSign();
        }

        private void ButtonPlus_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            } else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.Addition;
            currentOperand = "";
            UpdateScreen();
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            PerformCalculatorOperation();
        }

        private void ButtonSubtract_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            }
            else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.Subtraction;
            currentOperand = "";
            UpdateScreen();
        }

        private void ButtonMultiply_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            }
            else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.Multiplication;
            currentOperand = "";
            UpdateScreen();
        }

        private void ButtonDivide_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            }
            else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.Division;
            currentOperand = "";
            UpdateScreen();
        }

        private void ButtonRootExtraction_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            }
            else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.RootExtraction;
            currentOperand = "";
            PerformCalculatorOperation();
            UpdateScreen();
        }

        private void ButtonExponentiation_Click(object sender, EventArgs e)
        {
            if (calcOperator == CalculatorOperator.None)
            {
                firstOperand = currentOperand;
            }
            else
            {
                PerformCalculatorOperation();
            }

            calcOperator = CalculatorOperator.Exponentation;
            currentOperand = "";
            PerformCalculatorOperation();
            UpdateScreen();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            calcOperator = CalculatorOperator.None;
            firstOperand = null;
            secondOperand = null;
            currentOperand = null;
            UpdateScreen();
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
