namespace Calculator
{
    partial class CalculatorForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalculatorForm));
            tableLayoutPanel2 = new TableLayoutPanel();
            keypadDelete = new Button();
            keypadClear = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            keypadComma = new Button();
            keypadEqual = new Button();
            keypad0 = new Button();
            keypad3 = new Button();
            keypad2 = new Button();
            keypad1 = new Button();
            keypad6 = new Button();
            keypad5 = new Button();
            keypad4 = new Button();
            keypad9 = new Button();
            keypad8 = new Button();
            keypad7 = new Button();
            tableLayoutPanel5 = new TableLayoutPanel();
            keypadPlus = new Button();
            keypadMinus = new Button();
            keypadMultiply = new Button();
            keypadDivide = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            expressionInput = new TextBox();
            expressionBufferLabel = new Label();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(keypadDelete, 1, 0);
            tableLayoutPanel2.Controls.Add(keypadClear, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 102);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(334, 61);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // keypadDelete
            // 
            keypadDelete.Dock = DockStyle.Fill;
            keypadDelete.Font = new Font("Sans Serif Collection", 14.2499981F, FontStyle.Regular, GraphicsUnit.Point, 0);
            keypadDelete.Location = new Point(167, 0);
            keypadDelete.Margin = new Padding(0);
            keypadDelete.Name = "keypadDelete";
            keypadDelete.Size = new Size(167, 61);
            keypadDelete.TabIndex = 1;
            keypadDelete.Text = "⌫";
            keypadDelete.UseCompatibleTextRendering = true;
            keypadDelete.Click += KeypadDelete_Click;
            // 
            // keypadClear
            // 
            keypadClear.Dock = DockStyle.Fill;
            keypadClear.Font = new Font("Sans Serif Collection", 14.2499981F, FontStyle.Regular, GraphicsUnit.Point, 0);
            keypadClear.Location = new Point(0, 0);
            keypadClear.Margin = new Padding(0);
            keypadClear.Name = "keypadClear";
            keypadClear.Size = new Size(167, 61);
            keypadClear.TabIndex = 0;
            keypadClear.Text = "Clear";
            keypadClear.UseCompatibleTextRendering = true;
            keypadClear.Click += KeypadClear_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Size = new Size(334, 411);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel5, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Font = new Font("Sans Serif Collection", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tableLayoutPanel3.Location = new Point(0, 163);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(334, 248);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33334F));
            tableLayoutPanel4.Controls.Add(keypadComma, 0, 3);
            tableLayoutPanel4.Controls.Add(keypadEqual, 2, 3);
            tableLayoutPanel4.Controls.Add(keypad0, 1, 3);
            tableLayoutPanel4.Controls.Add(keypad3, 2, 2);
            tableLayoutPanel4.Controls.Add(keypad2, 1, 2);
            tableLayoutPanel4.Controls.Add(keypad1, 0, 2);
            tableLayoutPanel4.Controls.Add(keypad6, 2, 1);
            tableLayoutPanel4.Controls.Add(keypad5, 1, 1);
            tableLayoutPanel4.Controls.Add(keypad4, 0, 1);
            tableLayoutPanel4.Controls.Add(keypad9, 2, 0);
            tableLayoutPanel4.Controls.Add(keypad8, 1, 0);
            tableLayoutPanel4.Controls.Add(keypad7, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 4;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Size = new Size(250, 248);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // keypadComma
            // 
            keypadComma.Dock = DockStyle.Fill;
            keypadComma.Font = new Font("Sans Serif Collection", 18F);
            keypadComma.Location = new Point(0, 186);
            keypadComma.Margin = new Padding(0);
            keypadComma.Name = "keypadComma";
            keypadComma.Size = new Size(83, 62);
            keypadComma.TabIndex = 9;
            keypadComma.Text = ",";
            keypadComma.UseCompatibleTextRendering = true;
            keypadComma.Click += KeypadComma_Click;
            // 
            // keypadEqual
            // 
            keypadEqual.Dock = DockStyle.Fill;
            keypadEqual.Font = new Font("Sans Serif Collection", 18F);
            keypadEqual.Location = new Point(166, 186);
            keypadEqual.Margin = new Padding(0);
            keypadEqual.Name = "keypadEqual";
            keypadEqual.Size = new Size(84, 62);
            keypadEqual.TabIndex = 11;
            keypadEqual.Text = "=";
            keypadEqual.UseCompatibleTextRendering = true;
            keypadEqual.Click += KeypadEqual_Click;
            // 
            // keypad0
            // 
            keypad0.Dock = DockStyle.Fill;
            keypad0.Location = new Point(83, 186);
            keypad0.Margin = new Padding(0);
            keypad0.Name = "keypad0";
            keypad0.Size = new Size(83, 62);
            keypad0.TabIndex = 10;
            keypad0.Text = "0";
            keypad0.UseCompatibleTextRendering = true;
            keypad0.Click += KeypadAnyNumber_Click;
            // 
            // keypad3
            // 
            keypad3.Dock = DockStyle.Fill;
            keypad3.Location = new Point(166, 124);
            keypad3.Margin = new Padding(0);
            keypad3.Name = "keypad3";
            keypad3.Size = new Size(84, 62);
            keypad3.TabIndex = 8;
            keypad3.Text = "3";
            keypad3.UseCompatibleTextRendering = true;
            keypad3.Click += KeypadAnyNumber_Click;
            // 
            // keypad2
            // 
            keypad2.Dock = DockStyle.Fill;
            keypad2.Location = new Point(83, 124);
            keypad2.Margin = new Padding(0);
            keypad2.Name = "keypad2";
            keypad2.Size = new Size(83, 62);
            keypad2.TabIndex = 7;
            keypad2.Text = "2";
            keypad2.UseCompatibleTextRendering = true;
            keypad2.Click += KeypadAnyNumber_Click;
            // 
            // keypad1
            // 
            keypad1.Dock = DockStyle.Fill;
            keypad1.Location = new Point(0, 124);
            keypad1.Margin = new Padding(0);
            keypad1.Name = "keypad1";
            keypad1.Size = new Size(83, 62);
            keypad1.TabIndex = 6;
            keypad1.Text = "1";
            keypad1.UseCompatibleTextRendering = true;
            keypad1.Click += KeypadAnyNumber_Click;
            // 
            // keypad6
            // 
            keypad6.Dock = DockStyle.Fill;
            keypad6.Location = new Point(166, 62);
            keypad6.Margin = new Padding(0);
            keypad6.Name = "keypad6";
            keypad6.Size = new Size(84, 62);
            keypad6.TabIndex = 5;
            keypad6.Text = "6";
            keypad6.UseCompatibleTextRendering = true;
            keypad6.Click += KeypadAnyNumber_Click;
            // 
            // keypad5
            // 
            keypad5.Dock = DockStyle.Fill;
            keypad5.Location = new Point(83, 62);
            keypad5.Margin = new Padding(0);
            keypad5.Name = "keypad5";
            keypad5.Size = new Size(83, 62);
            keypad5.TabIndex = 4;
            keypad5.Text = "5";
            keypad5.UseCompatibleTextRendering = true;
            keypad5.Click += KeypadAnyNumber_Click;
            // 
            // keypad4
            // 
            keypad4.Dock = DockStyle.Fill;
            keypad4.Location = new Point(0, 62);
            keypad4.Margin = new Padding(0);
            keypad4.Name = "keypad4";
            keypad4.Size = new Size(83, 62);
            keypad4.TabIndex = 3;
            keypad4.Text = "4";
            keypad4.UseCompatibleTextRendering = true;
            keypad4.Click += KeypadAnyNumber_Click;
            // 
            // keypad9
            // 
            keypad9.Dock = DockStyle.Fill;
            keypad9.Location = new Point(166, 0);
            keypad9.Margin = new Padding(0);
            keypad9.Name = "keypad9";
            keypad9.Size = new Size(84, 62);
            keypad9.TabIndex = 2;
            keypad9.Text = "9";
            keypad9.UseCompatibleTextRendering = true;
            keypad9.Click += KeypadAnyNumber_Click;
            // 
            // keypad8
            // 
            keypad8.Dock = DockStyle.Fill;
            keypad8.Location = new Point(83, 0);
            keypad8.Margin = new Padding(0);
            keypad8.Name = "keypad8";
            keypad8.Size = new Size(83, 62);
            keypad8.TabIndex = 1;
            keypad8.Text = "8";
            keypad8.UseCompatibleTextRendering = true;
            keypad8.Click += KeypadAnyNumber_Click;
            // 
            // keypad7
            // 
            keypad7.Dock = DockStyle.Fill;
            keypad7.Location = new Point(0, 0);
            keypad7.Margin = new Padding(0);
            keypad7.Name = "keypad7";
            keypad7.Size = new Size(83, 62);
            keypad7.TabIndex = 0;
            keypad7.Text = "7";
            keypad7.UseCompatibleTextRendering = true;
            keypad7.Click += KeypadAnyNumber_Click;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(keypadPlus, 0, 3);
            tableLayoutPanel5.Controls.Add(keypadMinus, 0, 2);
            tableLayoutPanel5.Controls.Add(keypadMultiply, 0, 1);
            tableLayoutPanel5.Controls.Add(keypadDivide, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(250, 0);
            tableLayoutPanel5.Margin = new Padding(0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 4;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel5.Size = new Size(84, 248);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // keypadPlus
            // 
            keypadPlus.Dock = DockStyle.Fill;
            keypadPlus.Font = new Font("Sans Serif Collection", 18F);
            keypadPlus.Location = new Point(0, 186);
            keypadPlus.Margin = new Padding(0);
            keypadPlus.Name = "keypadPlus";
            keypadPlus.Size = new Size(84, 62);
            keypadPlus.TabIndex = 3;
            keypadPlus.Tag = "+";
            keypadPlus.Text = "+";
            keypadPlus.UseCompatibleTextRendering = true;
            keypadPlus.Click += KeypadOperation_Click;
            // 
            // keypadMinus
            // 
            keypadMinus.Dock = DockStyle.Fill;
            keypadMinus.Font = new Font("Sans Serif Collection", 18F);
            keypadMinus.Location = new Point(0, 124);
            keypadMinus.Margin = new Padding(0);
            keypadMinus.Name = "keypadMinus";
            keypadMinus.Size = new Size(84, 62);
            keypadMinus.TabIndex = 2;
            keypadMinus.Tag = "-";
            keypadMinus.Text = "−";
            keypadMinus.UseCompatibleTextRendering = true;
            keypadMinus.Click += KeypadOperation_Click;
            // 
            // keypadMultiply
            // 
            keypadMultiply.Dock = DockStyle.Fill;
            keypadMultiply.Font = new Font("Sans Serif Collection", 18F);
            keypadMultiply.Location = new Point(0, 62);
            keypadMultiply.Margin = new Padding(0);
            keypadMultiply.Name = "keypadMultiply";
            keypadMultiply.Size = new Size(84, 62);
            keypadMultiply.TabIndex = 1;
            keypadMultiply.Tag = "*";
            keypadMultiply.Text = "×";
            keypadMultiply.UseCompatibleTextRendering = true;
            keypadMultiply.Click += KeypadOperation_Click;
            // 
            // keypadDivide
            // 
            keypadDivide.Dock = DockStyle.Fill;
            keypadDivide.Font = new Font("Sans Serif Collection", 18F);
            keypadDivide.Location = new Point(0, 0);
            keypadDivide.Margin = new Padding(0);
            keypadDivide.Name = "keypadDivide";
            keypadDivide.Size = new Size(84, 62);
            keypadDivide.TabIndex = 0;
            keypadDivide.Tag = "/";
            keypadDivide.Text = "÷";
            keypadDivide.UseCompatibleTextRendering = true;
            keypadDivide.Click += KeypadOperation_Click;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(expressionInput, 0, 1);
            tableLayoutPanel6.Controls.Add(expressionBufferLabel, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 66.6666641F));
            tableLayoutPanel6.Size = new Size(328, 96);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // expressionInput
            // 
            expressionInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            expressionInput.BorderStyle = BorderStyle.None;
            expressionInput.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 204);
            expressionInput.Location = new Point(0, 32);
            expressionInput.Margin = new Padding(0, 0, 0, 3);
            expressionInput.MinimumSize = new Size(300, 0);
            expressionInput.Multiline = true;
            expressionInput.Name = "expressionInput";
            expressionInput.PlaceholderText = "0";
            expressionInput.ReadOnly = true;
            expressionInput.Size = new Size(328, 61);
            expressionInput.TabIndex = 1;
            expressionInput.TabStop = false;
            expressionInput.Text = "0";
            expressionInput.TextAlign = HorizontalAlignment.Right;
            expressionInput.UseWaitCursor = true;
            // 
            // expressionBufferLabel
            // 
            expressionBufferLabel.AutoSize = true;
            expressionBufferLabel.Dock = DockStyle.Fill;
            expressionBufferLabel.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            expressionBufferLabel.Location = new Point(0, 0);
            expressionBufferLabel.Margin = new Padding(0);
            expressionBufferLabel.Name = "expressionBufferLabel";
            expressionBufferLabel.Size = new Size(328, 32);
            expressionBufferLabel.TabIndex = 0;
            expressionBufferLabel.TextAlign = ContentAlignment.MiddleRight;
            expressionBufferLabel.UseCompatibleTextRendering = true;
            // 
            // CalculatorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 411);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximumSize = new Size(350, 450);
            MinimumSize = new Size(350, 450);
            Name = "CalculatorForm";
            Text = "Calculator";
            KeyDown += CalculatorForm_KeyDown;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel2;
        private Button keypadDelete;
        private Button keypadClear;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Button keypadComma;
        private Button keypadEqual;
        private Button keypad0;
        private Button keypad3;
        private Button keypad2;
        private Button keypad1;
        private Button keypad6;
        private Button keypad5;
        private Button keypad4;
        private Button keypad9;
        private Button keypad8;
        private Button keypad7;
        private TableLayoutPanel tableLayoutPanel5;
        private Button keypadPlus;
        private Button keypadMinus;
        private Button keypadMultiply;
        private Button keypadDivide;
        private TableLayoutPanel tableLayoutPanel6;
        private TextBox expressionInput;
        private Label expressionBufferLabel;
    }
}
