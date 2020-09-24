using System;
using System.Windows.Forms;
using Calculator.model;
using Calculator.model.operators;

namespace Calculator
{
    /* Main form of the calculator program. Manages the state of the calculation and updates the
     * UI.
     * 
     * Implements ICalculationChangedObserver in order to update the calculator screen whenever
     * the calculation model changes. */
    public partial class CalculatorForm : 
        Form,
        ICalculationChangedObserver,
        IArithmeticExceptionObserver
    {
        /* The calculation is readonly and is in practice a singleton in the sense that only one
         * instance of Calculation is created. Previously a new calculation was created when
         * the clear button was clicked, since I think that makes the most sense. However, when
         * implementing ICaluclationChangedObserver I encountered a bug that caused the screen
         * to not update when the clear button was clicked. The bug was caused by the form (the
         * observer) not being notified that the calculation was cleared, since the 
         * subject-observer link was broken when a new calculation instance was created. In order
         * to avoid duplicate code I chose to make calculation a readonly, single-instance object
         * and implemented a Clear() function in Calculation instead. */
        private readonly Calculation calculation;

        public CalculatorForm()
        {
            InitializeComponent();
            calculation = new Calculation();

            /* Set up this form to be notified whenever the calculation is updated/changed. The
             * reason for using the observer pattern here is to avoid duplicated code updating
             * the screen in each button clicked method. */
            calculation.AddChangeObserver(this);

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
            ShowErrorDialog("Division med noll är odefinierat.");
        }

        public void OnOverflowException()
        {
            ShowErrorDialog(
                "Operationen resulterade i ett tal som var för stort eller för litet för att " +
                "kunna visas."
             );
        }

        public void OnNegativeSquareRootException()
        {
            ShowErrorDialog("Roten ur negativa tal är odefinierat för reella tal.");
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(
                this,
                message,
                "Felmeddelande",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        // Handling of keyboard shortcuts.
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
