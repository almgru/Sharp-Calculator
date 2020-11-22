using System;
using System.Windows.Forms;

using Calculator.model;
using Calculator.model.operators;

namespace Calculator
{
    /* Main form of the calculator program. Manages the state of the calculation and updates the UI.
     * 
     * Implements ICalculationChangedObserver in order to update the calculator screen whenever the
     * calculation model changes, and IArithmeticExceptionObserver in order to show error messages
     * when arithmetic errors occurr. */
    public partial class CalculatorForm : 
        Form,
        ICalculationChangedObserver,
        IArithmeticExceptionObserver
    {
        /* The calculation is readonly and is in practice a singleton in the sense that only one
         * instance of Calculation is created. */
        private readonly Calculation calculation;

        public CalculatorForm()
        {
            InitializeComponent();
            calculation = new Calculation();

            /* Set up this form to be notified whenever the calculation is updated/changed. The
             * reason for using the observer pattern here is to avoid duplicated code updating the
             * screen in each button clicked method. */
            calculation.AddChangeObserver(this);

            /* Set up this form to be notified whenever an arithmetic exception occurs. See
             * IArithmeticExceptionObserver for the kinds of exceptions that can occurr and when
             * they occurr. */
            calculation.AddArithmeticExceptionObserver(this);
        }

        private void Button0_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(0);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(1);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(2);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(3);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(4);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(5);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(6);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(7);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(8);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(9);
        }

        private void ButtonDecimalPoint_Click(object sender, EventArgs e)
        {
            calculation.AddDecimalSeparator();
        }

        private void ButtonChangeSign_Click(object sender, EventArgs e)
        {
            calculation.ChangeSign();
        }

        private void ButtonPlus_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new AdditionOperator());
        }

        private void ButtonSubtract_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new SubtractionOperator());
        }

        private void ButtonMultiply_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new MultiplicationOperator());
        }

        private void ButtonDivide_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new DivisionOperator());
        }

        private void ButtonRootExtraction_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new SquareRootOperator());
        }

        private void ButtonExponentiation_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new PowerOfTwoOperator());
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            calculation.Calculate();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            calculation.Clear();
        }

        public void OnCalculationChanged()
        {
            TextBoxScreen.Text = calculation.ToString();
        }

        public void OnDivideByZeroException()
        {
            ShowErrorDialog("Division by zero is undefined.");
        }

        public void OnOverflowException()
        {
            ShowErrorDialog(
                "The operation resulted in a number that was too big or too" +
                "small."
             );
        }

        public void OnNegativeSquareRootException()
        {
            ShowErrorDialog(
                "The square root of negative numbers is undefined for real " +
                "numbers."
            );
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Error Message",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        /* Handling of shortcuts. Shortcut for power of two and square root operators are currently
         * not implemented. */
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    {
                        Button0_Click(null, null);
                        break;
                    }

                case Keys.D1:
                case Keys.NumPad1:
                    {
                        Button1_Click(null, null);
                        break;
                    }

                case Keys.D2:
                case Keys.NumPad2:
                    {
                        Button2_Click(null, null);
                        break;
                    }

                case Keys.D3:
                case Keys.NumPad3:
                    {
                        Button3_Click(null, null);
                        break;
                    }

                case Keys.D4:
                case Keys.NumPad4:
                    {
                        Button4_Click(null, null);
                        break;
                    }

                case Keys.D5:
                case Keys.NumPad5:
                    {
                        Button5_Click(null, null);
                        break;
                    }

                case Keys.D6:
                case Keys.NumPad6:
                    {
                        Button6_Click(null, null);
                        break;
                    }

                case Keys.D7:
                case Keys.NumPad7:
                    {
                        Button7_Click(null, null);
                        break;
                    }

                case Keys.D8:
                case Keys.NumPad8:
                    {
                        Button8_Click(null, null);
                        break;
                    }

                case Keys.D9:
                case Keys.NumPad9:
                    {
                        Button9_Click(null, null);
                        break;
                    }

                case Keys.Decimal:
                    {
                        ButtonDecimalPoint_Click(null, null);
                        break;
                    }

                case Keys.Add:
                    {
                        ButtonPlus_Click(null, null);
                        break;
                    }

                case Keys.Subtract:
                    {
                        ButtonSubtract_Click(null, null);
                        break;
                    }

                case Keys.Multiply:
                    {
                        ButtonMultiply_Click(null, null);
                        break;
                    }

                case Keys.Divide:
                    {
                        ButtonDivide_Click(null, null);
                        break;
                    }

                case Keys.Back:
                case Keys.Delete:
                    {
                        ButtonClear_Click(null, null);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
