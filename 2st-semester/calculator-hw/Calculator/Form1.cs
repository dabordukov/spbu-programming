namespace Calculator
{
    /// <summary>
    /// Represents the main form of the calculator application.
    /// </summary>
    public partial class CalculatorForm : Form
    {
        private State state = State.Reset;
        private bool operandIsEmpty = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorForm"/> class.
        /// </summary>
        public CalculatorForm()
        {
            this.InitializeComponent();
        }

        private enum State
        {
            Reset,
            LeftOperand,
            RightOperand,
            Error,
        }

        private void ResetCalculator()
        {
            this.expressionInput.Text = "0";
            this.expressionBufferLabel.Text = string.Empty;
            this.state = State.Reset;
            this.operandIsEmpty = true;
        }

        private void KeypadAnyNumber_Click(object sender, EventArgs e)
        {
            if (this.state == State.Error)
            {
                this.ResetCalculator();
            }

            if (this.state == State.Reset)
            {
                this.expressionInput.Text = string.Empty;
                this.state = State.LeftOperand;
            }

            if (this.expressionInput.Text == "0")
            {
                this.expressionInput.Text = string.Empty;
            }

            if (this.expressionInput.Text.Length == 16)
            {
                return;
            }

            this.expressionInput.Text += ((Button)sender).Text;
            this.operandIsEmpty = false;
        }

        private void KeypadComma_Click(object sender, EventArgs e)
        {
            if (!this.expressionInput.Text.Contains(','))
            {
                this.expressionInput.Text += ",";
            }
        }

        private void KeypadDelete_Click(object sender, EventArgs e)
        {
            if (this.expressionInput.Text != "0")
            {
                this.expressionInput.Text = this.expressionInput.Text[..^1];
            }

            if (this.expressionInput.Text == string.Empty)
            {
                this.expressionInput.Text = "0";
                this.operandIsEmpty = true;
            }
        }

        private (bool Ok, string Result) Evaluate()
        {
            char oldOperation = this.expressionBufferLabel.Text[^1];
            if (oldOperation == '/' && this.expressionInput.Text == "0")
            {
                return (false, "Error: Division by zero");
            }

            string result = Calculator.Calculate(this.expressionBufferLabel.Text + " " + this.expressionInput.Text);
            if (result.Length > 16)
            {
                return (false, "Error: Too long");
            }

            return (true, result);
        }

        private void KeypadEqual_Click(object sender, EventArgs e)
        {
            if (this.state == State.RightOperand)
            {
                var (ok, result) = this.Evaluate();
                if (!ok)
                {
                    this.SetError(result);
                    return;
                }

                this.expressionBufferLabel.Text = string.Empty;
                this.expressionInput.Text = result;
                this.state = State.LeftOperand;
            }
        }

        private void KeypadOperation_Click(object sender, EventArgs e)
        {
            if (sender is null)
            {
                return;
            }

            var button = sender as Button;
            if (button is null || button.Tag is null)
            {
                return;
            }

            var operationString = button.Tag.ToString();
            if (operationString is null)
            {
                return;
            }

            char operation = operationString[0];

            if (this.expressionBufferLabel.Text != string.Empty && (this.state == State.Reset || this.operandIsEmpty))
            {
                if (this.expressionBufferLabel.Text[^1] is '+' or '-' or '/' or '*')
                {
                    this.expressionBufferLabel.Text = this.expressionBufferLabel.Text[..^1] + operation;
                }

                return;
            }

            if (this.state == State.LeftOperand)
            {
                this.expressionBufferLabel.Text = this.expressionInput.Text + " " + operation;
                this.expressionInput.Text = "0";
                this.state = State.RightOperand;
                this.operandIsEmpty = true;
                return;
            }

            if (this.state == State.RightOperand)
            {
                var (ok, result) = this.Evaluate();

                if (!ok)
                {
                    this.SetError(result);
                    return;
                }

                this.expressionBufferLabel.Text += " " + this.expressionInput.Text;
                this.expressionBufferLabel.Text = Calculator.Calculate(this.expressionBufferLabel.Text) + " " + operation;
                this.expressionInput.Text = "0";
            }
        }

        private void SetError(string error)
        {
            this.expressionBufferLabel.Text = string.Empty;
            this.expressionInput.Text = error;
            this.state = State.Error;
        }

        private void KeypadClear_Click(object sender, EventArgs e)
        {
            this.ResetCalculator();
        }

        private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.D0 or Keys.NumPad0:
                    this.keypad0.PerformClick();
                    break;
                case Keys.D1 or Keys.NumPad1:
                    this.keypad1.PerformClick();
                    break;
                case Keys.D2 or Keys.NumPad2:
                    this.keypad2.PerformClick();
                    break;
                case Keys.D3 or Keys.NumPad3:
                    this.keypad3.PerformClick();
                    break;
                case Keys.D4 or Keys.NumPad4:
                    this.keypad4.PerformClick();
                    break;
                case Keys.D5 or Keys.NumPad5:
                    this.keypad5.PerformClick();
                    break;
                case Keys.D6 or Keys.NumPad6:
                    this.keypad6.PerformClick();
                    break;
                case Keys.D7 or Keys.NumPad7:
                    this.keypad7.PerformClick();
                    break;
                case Keys.D8 or Keys.NumPad8:
                    this.keypad8.PerformClick();
                    break;
                case Keys.D9 or Keys.NumPad9:
                    this.keypad9.PerformClick();
                    break;
                case Keys.Shift | Keys.Oemplus:
                    this.keypadPlus.PerformClick();
                    break;
                case Keys.OemMinus:
                    this.keypadMinus.PerformClick();
                    break;
                case Keys.Shift | Keys.D8:
                    this.keypadMultiply.PerformClick();
                    break;
                case Keys.Divide:
                    this.keypadDivide.PerformClick();
                    break;
                case Keys.Escape:
                    this.keypadClear.PerformClick();
                    break;
                case Keys.Back:
                    this.keypadDelete.PerformClick();
                    break;
                case Keys.Enter or Keys.Oemplus:
                    this.keypadEqual.PerformClick();
                    break;
                case Keys.Oemcomma:
                    this.keypadComma.PerformClick();
                    break;
            }
        }
    }
}
