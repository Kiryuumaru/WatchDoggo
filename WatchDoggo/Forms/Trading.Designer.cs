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
            this.buttonReset = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numericUpDownLRSize = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panelAutoBuy = new System.Windows.Forms.Panel();
            this.buttonAutoBuy = new System.Windows.Forms.Button();
            this.checkBoxCuttoff = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelLoseCurr = new System.Windows.Forms.Label();
            this.textBoxLoseCutoff = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelWinCurr = new System.Windows.Forms.Label();
            this.textBoxWinCutoff = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSlopeThres = new System.Windows.Forms.TextBox();
            this.textBoxR2Thres = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSlope = new System.Windows.Forms.Label();
            this.labelR2 = new System.Windows.Forms.Label();
            this.labelAccuracy = new System.Windows.Forms.Label();
            this.labelProfit = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.comboBoxUnit = new System.Windows.Forms.ComboBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.labelCurrencyTrade = new System.Windows.Forms.Label();
            this.textBoxBuyAmount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLRSize)).BeginInit();
            this.panelAutoBuy.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.buttonReset);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Controls.Add(this.panelAutoBuy);
            this.panel6.Controls.Add(this.labelSlope);
            this.panel6.Controls.Add(this.labelR2);
            this.panel6.Controls.Add(this.labelAccuracy);
            this.panel6.Controls.Add(this.labelProfit);
            this.panel6.Controls.Add(this.numericUpDownDuration);
            this.panel6.Controls.Add(this.buttonDown);
            this.panel6.Controls.Add(this.buttonUp);
            this.panel6.Controls.Add(this.comboBoxUnit);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.label10);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(649, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10);
            this.panel6.Size = new System.Drawing.Size(235, 461);
            this.panel6.TabIndex = 9;
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Location = new System.Drawing.Point(7, 332);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(216, 23);
            this.buttonReset.TabIndex = 59;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.ButtonReset_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.numericUpDownLRSize);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(8, 298);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(214, 28);
            this.panel2.TabIndex = 51;
            // 
            // numericUpDownLRSize
            // 
            this.numericUpDownLRSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownLRSize.Location = new System.Drawing.Point(131, 3);
            this.numericUpDownLRSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownLRSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownLRSize.Name = "numericUpDownLRSize";
            this.numericUpDownLRSize.Size = new System.Drawing.Size(65, 20);
            this.numericUpDownLRSize.TabIndex = 48;
            this.numericUpDownLRSize.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownLRSize.ValueChanged += new System.EventHandler(this.NumericUpDownLRSize_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 5);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "LR Spectrum Size:";
            // 
            // panelAutoBuy
            // 
            this.panelAutoBuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAutoBuy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAutoBuy.Controls.Add(this.buttonAutoBuy);
            this.panelAutoBuy.Controls.Add(this.checkBoxCuttoff);
            this.panelAutoBuy.Controls.Add(this.label12);
            this.panelAutoBuy.Controls.Add(this.panel4);
            this.panelAutoBuy.Controls.Add(this.label11);
            this.panelAutoBuy.Controls.Add(this.panel3);
            this.panelAutoBuy.Controls.Add(this.label7);
            this.panelAutoBuy.Controls.Add(this.textBoxSlopeThres);
            this.panelAutoBuy.Controls.Add(this.textBoxR2Thres);
            this.panelAutoBuy.Controls.Add(this.label4);
            this.panelAutoBuy.Controls.Add(this.label3);
            this.panelAutoBuy.Controls.Add(this.label2);
            this.panelAutoBuy.Location = new System.Drawing.Point(8, 101);
            this.panelAutoBuy.Name = "panelAutoBuy";
            this.panelAutoBuy.Size = new System.Drawing.Size(214, 191);
            this.panelAutoBuy.TabIndex = 46;
            // 
            // buttonAutoBuy
            // 
            this.buttonAutoBuy.Location = new System.Drawing.Point(54, 160);
            this.buttonAutoBuy.Name = "buttonAutoBuy";
            this.buttonAutoBuy.Size = new System.Drawing.Size(104, 23);
            this.buttonAutoBuy.TabIndex = 58;
            this.buttonAutoBuy.Text = "Start Auto Buy";
            this.buttonAutoBuy.UseVisualStyleBackColor = true;
            this.buttonAutoBuy.Click += new System.EventHandler(this.ButtonAutoBuy_Click);
            // 
            // checkBoxCuttoff
            // 
            this.checkBoxCuttoff.AutoSize = true;
            this.checkBoxCuttoff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCuttoff.Location = new System.Drawing.Point(144, 80);
            this.checkBoxCuttoff.Name = "checkBoxCuttoff";
            this.checkBoxCuttoff.Size = new System.Drawing.Size(65, 17);
            this.checkBoxCuttoff.TabIndex = 57;
            this.checkBoxCuttoff.Text = "Enable";
            this.checkBoxCuttoff.UseVisualStyleBackColor = true;
            this.checkBoxCuttoff.CheckedChanged += new System.EventHandler(this.CheckBoxCuttoff_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 81);
            this.label12.Margin = new System.Windows.Forms.Padding(3);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 56;
            this.label12.Text = "Cutoffs";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.labelLoseCurr);
            this.panel4.Controls.Add(this.textBoxLoseCutoff);
            this.panel4.Location = new System.Drawing.Point(67, 129);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(117, 25);
            this.panel4.TabIndex = 55;
            // 
            // labelLoseCurr
            // 
            this.labelLoseCurr.AutoSize = true;
            this.labelLoseCurr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoseCurr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoseCurr.Location = new System.Drawing.Point(0, 0);
            this.labelLoseCurr.Name = "labelLoseCurr";
            this.labelLoseCurr.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.labelLoseCurr.Size = new System.Drawing.Size(33, 16);
            this.labelLoseCurr.TabIndex = 23;
            this.labelLoseCurr.Text = "N/A";
            // 
            // textBoxLoseCutoff
            // 
            this.textBoxLoseCutoff.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxLoseCutoff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLoseCutoff.Location = new System.Drawing.Point(33, 0);
            this.textBoxLoseCutoff.Name = "textBoxLoseCutoff";
            this.textBoxLoseCutoff.Size = new System.Drawing.Size(84, 20);
            this.textBoxLoseCutoff.TabIndex = 19;
            this.textBoxLoseCutoff.TextChanged += new System.EventHandler(this.TextBoxLoseCutoff_TextChanged);
            this.textBoxLoseCutoff.Leave += new System.EventHandler(this.TextBoxLoseCutoff_Leave);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(26, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(34, 13);
            this.label11.TabIndex = 54;
            this.label11.Text = "Lose";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.labelWinCurr);
            this.panel3.Controls.Add(this.textBoxWinCutoff);
            this.panel3.Location = new System.Drawing.Point(67, 103);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(117, 23);
            this.panel3.TabIndex = 53;
            // 
            // labelWinCurr
            // 
            this.labelWinCurr.AutoSize = true;
            this.labelWinCurr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWinCurr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWinCurr.Location = new System.Drawing.Point(0, 0);
            this.labelWinCurr.Name = "labelWinCurr";
            this.labelWinCurr.Padding = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.labelWinCurr.Size = new System.Drawing.Size(33, 16);
            this.labelWinCurr.TabIndex = 23;
            this.labelWinCurr.Text = "N/A";
            // 
            // textBoxWinCutoff
            // 
            this.textBoxWinCutoff.Dock = System.Windows.Forms.DockStyle.Right;
            this.textBoxWinCutoff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxWinCutoff.Location = new System.Drawing.Point(33, 0);
            this.textBoxWinCutoff.Name = "textBoxWinCutoff";
            this.textBoxWinCutoff.Size = new System.Drawing.Size(84, 20);
            this.textBoxWinCutoff.TabIndex = 19;
            this.textBoxWinCutoff.TextChanged += new System.EventHandler(this.TextBoxWinCutoff_TextChanged);
            this.textBoxWinCutoff.Leave += new System.EventHandler(this.TextBoxWinCutoff_Leave);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(31, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 52;
            this.label7.Text = "Win";
            // 
            // textBoxSlopeThres
            // 
            this.textBoxSlopeThres.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSlopeThres.Location = new System.Drawing.Point(100, 54);
            this.textBoxSlopeThres.Name = "textBoxSlopeThres";
            this.textBoxSlopeThres.Size = new System.Drawing.Size(84, 20);
            this.textBoxSlopeThres.TabIndex = 51;
            this.textBoxSlopeThres.TextChanged += new System.EventHandler(this.TextBoxSlopeThres_TextChanged);
            this.textBoxSlopeThres.Leave += new System.EventHandler(this.TextBoxSlopeThres_Leave);
            // 
            // textBoxR2Thres
            // 
            this.textBoxR2Thres.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxR2Thres.Location = new System.Drawing.Point(100, 28);
            this.textBoxR2Thres.Name = "textBoxR2Thres";
            this.textBoxR2Thres.Size = new System.Drawing.Size(84, 20);
            this.textBoxR2Thres.TabIndex = 50;
            this.textBoxR2Thres.TextChanged += new System.EventHandler(this.TextBoxR2Thres_TextChanged);
            this.textBoxR2Thres.Leave += new System.EventHandler(this.TextBoxR2Thres_Leave);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(51, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Slope:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(23, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "R Squared:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Auto Buy Config";
            // 
            // labelSlope
            // 
            this.labelSlope.AutoSize = true;
            this.labelSlope.Location = new System.Drawing.Point(13, 380);
            this.labelSlope.Margin = new System.Windows.Forms.Padding(3);
            this.labelSlope.Name = "labelSlope";
            this.labelSlope.Size = new System.Drawing.Size(52, 13);
            this.labelSlope.TabIndex = 44;
            this.labelSlope.Text = "Slope = 0";
            // 
            // labelR2
            // 
            this.labelR2.AutoSize = true;
            this.labelR2.Location = new System.Drawing.Point(13, 361);
            this.labelR2.Margin = new System.Windows.Forms.Padding(3);
            this.labelR2.Name = "labelR2";
            this.labelR2.Size = new System.Drawing.Size(76, 13);
            this.labelR2.TabIndex = 43;
            this.labelR2.Text = "R Squared = 0";
            // 
            // labelAccuracy
            // 
            this.labelAccuracy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAccuracy.AutoSize = true;
            this.labelAccuracy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccuracy.Location = new System.Drawing.Point(13, 435);
            this.labelAccuracy.Margin = new System.Windows.Forms.Padding(3);
            this.labelAccuracy.Name = "labelAccuracy";
            this.labelAccuracy.Size = new System.Drawing.Size(145, 13);
            this.labelAccuracy.TabIndex = 42;
            this.labelAccuracy.Text = "Trading Accuracy: 100%";
            // 
            // labelProfit
            // 
            this.labelProfit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelProfit.AutoSize = true;
            this.labelProfit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProfit.Location = new System.Drawing.Point(13, 416);
            this.labelProfit.Margin = new System.Windows.Forms.Padding(3);
            this.labelProfit.Name = "labelProfit";
            this.labelProfit.Size = new System.Drawing.Size(152, 13);
            this.labelProfit.TabIndex = 41;
            this.labelProfit.Text = "Current Profit: USD 0.000";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(71, 43);
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownDuration.TabIndex = 40;
            this.numericUpDownDuration.ValueChanged += new System.EventHandler(this.NumericUpDownDuration_ValueChanged);
            // 
            // buttonDown
            // 
            this.buttonDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDown.Location = new System.Drawing.Point(118, 72);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(105, 23);
            this.buttonDown.TabIndex = 39;
            this.buttonDown.Text = "DOWN";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.ButtonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUp.Location = new System.Drawing.Point(7, 72);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(105, 23);
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
            this.comboBoxUnit.Location = new System.Drawing.Point(138, 43);
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
            this.panel9.Location = new System.Drawing.Point(106, 12);
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
            this.label9.Location = new System.Drawing.Point(10, 15);
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
            this.label10.Location = new System.Drawing.Point(10, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Duration:";
            // 
            // cartesianChart
            // 
            this.cartesianChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cartesianChart.Location = new System.Drawing.Point(0, 0);
            this.cartesianChart.Margin = new System.Windows.Forms.Padding(0);
            this.cartesianChart.Name = "cartesianChart";
            this.cartesianChart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cartesianChart.Size = new System.Drawing.Size(649, 461);
            this.cartesianChart.TabIndex = 12;
            this.cartesianChart.Text = "cartesianChart";
            // 
            // Trading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.cartesianChart);
            this.Controls.Add(this.panel6);
            this.Enabled = false;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Trading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trading";
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLRSize)).EndInit();
            this.panelAutoBuy.ResumeLayout(false);
            this.panelAutoBuy.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
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
        private LiveCharts.WinForms.CartesianChart cartesianChart;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.Label labelProfit;
        private System.Windows.Forms.Label labelAccuracy;
        private System.Windows.Forms.Label labelR2;
        private System.Windows.Forms.Label labelSlope;
        private System.Windows.Forms.Panel panelAutoBuy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numericUpDownLRSize;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSlopeThres;
        private System.Windows.Forms.TextBox textBoxR2Thres;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label labelLoseCurr;
        private System.Windows.Forms.TextBox textBoxLoseCutoff;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelWinCurr;
        private System.Windows.Forms.TextBox textBoxWinCutoff;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxCuttoff;
        private System.Windows.Forms.Button buttonAutoBuy;
        private System.Windows.Forms.Button buttonReset;
    }
}