using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Calculator.model;
using Calculator.model.operators;

namespace Calculator
{
    public partial class CalculatorForm : Form, ICalculationChangedObserver
    {
        private Calculation calculation;

        public CalculatorForm()
        {
            InitializeComponent();
            calculation = new Calculation();
            calculation.AddChangeObserver(this);
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
            calculation.AddDecimalPoint();
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
            calculation.AddOperator(new RootExtractionOperator());
        }

        private void ButtonExponentiation_Click(object sender, EventArgs e)
        {
            calculation.AddOperator(new ExponentiationOperator());
        }

        private void ButtonEquals_Click(object sender, EventArgs e)
        {
            calculation.Calculate();
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            calculation = new Calculation();
            calculation.AddChangeObserver(this);
        }

        public void OnCalculationChanged()
        {
            TextBoxScreen.Text = calculation.ToString();
        }
    }
}
