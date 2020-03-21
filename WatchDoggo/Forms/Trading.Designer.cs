namespace WatchDoggo.Forms
{
    partial class Trading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel6 = new System.Windows.Forms.Panel();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.labelCurrencyTrade = new System.Windows.Forms.Label();
            this.textBoxBuyAmount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.numericUpDownDuration);
            this.panel6.Controls.Add(this.buttonDown);
            this.panel6.Controls.Add(this.buttonUp);
            this.panel6.Controls.Add(this.comboBoxUnit);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(664, 10);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(235, 466);
            this.panel6.TabIndex = 9;
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(73, 36);
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownDuration.TabIndex = 40;
            this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.NumericUpDownDuration_ValueChanged);
            // 
            // buttonDown
            // 
            this.buttonDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDown.Location = new System.Drawing.Point(9, 100);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(216, 32);
            this.buttonDown.TabIndex = 39;
            this.buttonDown.Text = "DOWN";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.ButtonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUp.Location = new System.Drawing.Point(9, 62);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(216, 32);
            this.buttonUp.TabIndex = 38;
            this.buttonUp.Text = "UP";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.ButtonUp_Click);
            // 
            // comboBoxUnit
            // 
            this.comboBoxUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxUnit.FormattingEnabled = true;
            this.comboBoxUnit.Items.AddRange(new object[] {
            " ticks",
            " seconds",
            " minutes",
            " hours",
            " days"});
            this.comboBoxUnit.Location = new System.Drawing.Point(140, 36);
            this.comboBoxUnit.Name = "comboBoxUnit";
            this.comboBoxUnit.Size = new System.Drawing.Size(84, 20);
            this.comboBoxUnit.TabIndex = 12;
            this.comboBoxUnit.SelectedIndexChanged += new System.EventHandler(this.ComboBoxUnit_SelectedIndexChanged);
            // 
            // panel9
            // 
            this.panel9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel9.AutoSize = true;
            this.panel9.Controls.Add(this.labelCurrencyTrade);
            this.panel9.Controls.Add(this.textBoxBuyAmount);
            this.panel9.Location = new System.Drawing.Point(107, 7);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(117, 25);
            this.panel9.TabIndex = 20;
            // 
            // labelCurrencyTrade
            // 
            this.labelCurrencyTrade.AutoSize = true;
            this.labelCurrencyTrade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrencyTrade.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrencyTrade.Location = new System.Drawing.Point(0, 0);
            this.labelCurrencyTrade.Name = "labelCurrencyTrade";
            this.labelCurrencyTrade.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.labelCurrencyTrade.Size = new System.Drawing.Size(33, 16);
            this.labelCurrencyTrade.TabIndex = 23;
            this.labelCurrencyTrade.Text = "N/A";
            // 
            // textBoxBuyAmount
            // 
            this.textBoxBuyAmount.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxBuyAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBuyAmount.Location = new System.Drawing.Point(33, 0);
            this.textBoxBuyAmount.Name = "textBoxBuyAmount";
            this.textBoxBuyAmount.Size = new System.Drawing.Size(84, 20);
            this.textBoxBuyAmount.TabIndex = 19;
            this.textBoxBuyAmount.TextChanged += new System.EventHandler(this.TextBoxBuyAmount_TextChanged);
            this.textBoxBuyAmount.Leave += new System.EventHandler(this.TextBoxBuyAmount_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Trade Amount:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Duration:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 26);
            this.panel1.TabIndex = 11;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            " ticks",
            " seconds",
            " minutes",
            " hours",
            " days"});
            this.comboBox1.Location = new System.Drawing.Point(23, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 20);
            this.comboBox1.TabIndex = 38;
            // 
            // cartesianChart
            // 
            this.cartesianChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart.Location = new System.Drawing.Point(10, 36);
            this.cartesianChart.Name = "cartesianChart";
            this.cartesianChart.Size = new System.Drawing.Size(654, 440);
            this.cartesianChart.TabIndex = 12;
            this.cartesianChart.Text = "cartesianChart";
            // 
            // Trading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 486);
            this.Controls.Add(this.cartesianChart);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.MinimumSize = new System.Drawing.Size(925, 525);
            this.Name = "Trading";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trading";
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox comboBoxUnit;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label labelCurrencyTrade;
        private System.Windows.Forms.TextBox textBoxBuyAmount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox1;
        private LiveCharts.WinForms.CartesianChart cartesianChart;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
    }
}