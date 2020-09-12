using System;
using System.Windows.Forms;
using Calculator.model.operators;

namespace Calculator
{
    public partial class CalculatorForm : Form
    {
        private Calculation calculation;

        public CalculatorForm()
        {
            InitializeComponent();
            calculation = new Calculation();
        }

        private void UpdateScreen()
        {
            TextBoxScreen.Text = calculation.ToString();
        }

        private void Button0_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(0);
            UpdateScreen();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(1);
            UpdateScreen();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(2);
            UpdateScreen();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(3);
            UpdateScreen();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(4);
            UpdateScreen();
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(5);
            UpdateScreen();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(6);
            UpdateScreen();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(7);
            UpdateScreen();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(8);
            UpdateScreen();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            calculation.AddDigit(9);
            UpdateScreen();
        }

        private void ButtonDecimalPoint_Click(object sender, EventArgs e)
        {
            calculation.AddDecimalPoint();
            UpdateScreen();
        }

        private void ButtonChangeSign_Click(object sender, EventArgs e)
        {
            calculation.ChangeSign();
            UpdateScreen();
        }

        private void ButtonPlus_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new AdditionOperator());
            UpdateScreen();
        }

        private void ButtonSubtract_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new SubtractionOperator());
            UpdateScreen();
        }

        private void ButtonMultiply_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new MultiplicationOperator());
            UpdateScreen();
        }

        private void ButtonDivide_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new DivisionOperator());
            UpdateScreen();
        }

        private void ButtonRootExtraction_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new RootExtractionOperator());
            UpdateScreen();
        }

        private void ButtonExponentiation_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new ExponentiationOperator());
            UpdateScreen();
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            calculation.Calculate();
            UpdateScreen();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            calculation = new Calculation();
            UpdateScreen();
        }
    }
}
