using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoggoWire.Models;
using DoggoWire.Services;

namespace WatchDoggo.Forms
{
    public partial class Main : Form
    {
        #region Properties



        #endregion

        #region Initializers

        public Main()
        {
            InitializeComponent();
            Session.SetOnConnectionChanges(OnConnectionChanges);
            Session.Current.SetOnStateChanges(OnStateChanges);
            Session.Current.SetOnActiveSymbolAvailable(OnActiveSymbolAvailable);
            Session.Current.SetOnBalanceChanges(OnBalanceChanges);
            Session.Current.SetOnPurchase(OnPurchase);
            Session.Current.SetOnQuote(OnQuote);
            Session.Current.SetOnCalculateStateChange(OnCalculateStateChange);
            Session.Current.SetOnCutoff(OnCutoff);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Session.Init(new Storage());
            if (!Session.Current.Exist)
            {
                Hide();
                using (SwitchAccount switchAccount = new SwitchAccount())
                {
                    switchAccount.ShowDialog();
                    if (Session.Current.Exist)
                    {
                        Show();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Session.Stop();
            base.OnClosing(e);
        }

        #endregion

        #region EventHandlers

        private void OnConnectionChanges(Session.ConnectionChangesEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                if (args.Connected)
                {
                    buttonStartStop.Text = "Connected";
                }
                else
                {
                    buttonStartStop.Enabled = false;
                    buttonStartStop.Text = "Reconnecting . . .";
                }
            }));
        }

        private void OnStateChanges(Session.Current.StateChangesEventArgs args)
        {
            if (args.Reset)
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    dataGridViewPurchases.Rows.Clear();
                    dataGridViewMarket.Rows.Clear();
                }));
            }
            if (args.LoggedIn)
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    Text = Session.Current.Email;
                    labelAccount.Text = Session.Current.LoginId + (Session.Current.Virtual ? " (virtual)" : "");
                    labelAccuracy.Text = Session.Current.PurchasesCount > 0 ? ((Session.Current.Accuracy * 100).ToString("0.##") + "%") : "N/A";
                    labelPurchases.Text = Session.Current.PurchasesCount.ToString();
                    labelCurrencyTrade.Text = Session.Current.Currency;
                    labelCurrencyStart.Text = Session.Current.Currency;
                    labelCurrencyEnd.Text = Session.Current.Currency;
                    labelTotalProfitLoss.Text =
                        Session.Current.TotalSessionAmount.ToString("0.################") + " " +
                        Session.Current.Currency;
                    if (Session.Current.TotalSessionAmount < 0)
                    {
                        labelTotalProfitLoss.ForeColor = Color.Red;
                    }
                    else
                    {
                        labelTotalProfitLoss.ForeColor = Color.Green;
                    }
                    textBoxLRSize.Text = Session.Current.LRSize.ToString();
                    textBoxLRTailSize.Text = Session.Current.LRTailSize.ToString();
                    textBoxLRHeadSize.Text = Session.Current.LRHeadSize.ToString();
                    textBoxR2TailBarrier.Text = Session.Current.LRTailR2Barrier.ToString();
                    textBoxR2HeadBarrier.Text = Session.Current.LRHeadR2Barrier.ToString();
                    textBoxSlopeTailBarrier.Text = Session.Current.LRTailSlopeBarrier.ToString();
                    textBoxSlopeHeadBarrier.Text = Session.Current.LRHeadSlopeBarrier.ToString();
                    textBoxBuyAmount.Text = Session.Current.BuyAmount.ToString();
                    textBoxBuyDuration.Text = Session.Current.BuyDuration.ToString();
                    textBoxCutoffLose.Text = Session.Current.CutoffLoseAmount.ToString();
                    textBoxCutoffWin.Text = Session.Current.CutoffWinAmount.ToString();
                    checkBoxReverse.Checked = Session.Current.ReverseLogic;
                    checkBoxAllowStraight.Checked = Session.Current.AllowStraight;
                    checkBoxCutoffLose.Checked = Session.Current.CutoffLoseEnable;
                    checkBoxCutoffWin.Checked = Session.Current.CutoffWinEnable;
                    switch (Session.Current.BuyDurationUnit)
                    {
                        case Session.Current.DurationUnit.Ticks:
                            comboBoxUnit.SelectedIndex = 0;
                            break;
                        case Session.Current.DurationUnit.Seconds:
                            comboBoxUnit.SelectedIndex = 1;
                            break;
                        case Session.Current.DurationUnit.Minutes:
                            comboBoxUnit.SelectedIndex = 2;
                            break;
                        case Session.Current.DurationUnit.Hours:
                            comboBoxUnit.SelectedIndex = 3;
                            break;
                        case Session.Current.DurationUnit.Days:
                            comboBoxUnit.SelectedIndex = 4;
                            break;
                    }

                    buttonReset.Enabled = true;
                    buttonSwitch.Enabled = true;

                    if (!Session.Current.Calculating && !Session.Current.Started)
                    {
                        buttonCalc.Enabled = true;
                        buttonStartStop.Enabled = true;
                        textBoxLRSize.Enabled = true;
                        textBoxLRTailSize.Enabled = true;
                        textBoxLRHeadSize.Enabled = true;
                        textBoxR2TailBarrier.Enabled = true;
                        textBoxR2HeadBarrier.Enabled = true;
                        textBoxSlopeTailBarrier.Enabled = true;
                        textBoxSlopeHeadBarrier.Enabled = true;
                        textBoxBuyAmount.Enabled = true;
                        textBoxBuyDuration.Enabled = true;
                        comboBoxUnit.Enabled = true;
                        checkBoxReverse.Enabled = true;
                        checkBoxAllowStraight.Enabled = true;
                        checkBoxCutoffLose.Enabled = true;
                        checkBoxCutoffWin.Enabled = true;
                        textBoxCutoffLose.Enabled = checkBoxCutoffLose.Checked;
                        textBoxCutoffWin.Enabled = checkBoxCutoffWin.Checked;
                    }
                    else
                    {
                        buttonCalc.Enabled = false;
                        textBoxLRSize.Enabled = false;
                        textBoxLRTailSize.Enabled = false;
                        textBoxLRHeadSize.Enabled = false;
                        textBoxR2TailBarrier.Enabled = false;
                        textBoxR2HeadBarrier.Enabled = false;
                        textBoxSlopeTailBarrier.Enabled = false;
                        textBoxSlopeHeadBarrier.Enabled = false;
                        textBoxBuyAmount.Enabled = false;
                        textBoxBuyDuration.Enabled = false;
                        textBoxCutoffLose.Enabled = false;
                        textBoxCutoffWin.Enabled = false;
                        comboBoxUnit.Enabled = false;
                        checkBoxReverse.Enabled = false;
                        checkBoxAllowStraight.Enabled = false;
                        checkBoxCutoffLose.Enabled = false;
                        checkBoxCutoffWin.Enabled = false;
                    }

                    if (Session.Current.Started)
                    {
                        buttonStartStop.Text = "Stop Trading";
                    }
                    else
                    {
                        buttonStartStop.Text = "Start Trading";
                    }
                    buttonReset.Text = "Reset";
                    buttonReset.FlatStyle = FlatStyle.System;
                }));
            }
            else
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    labelAccount.Text = "N/A";
                    labelAccuracy.Text = "N/A";
                    labelWinCount.Text = "N/A";
                    labelLoseCount.Text = "N/A";
                    labelExpectedAccuracy.Text = "N/A";
                    labelExpectedCount.Text = "N/A";
                    labelPurchases.Text = "0";
                    labelCurrencyTrade.Text = "N/A";
                    labelCurrencyStart.Text = "N/A";
                    labelCurrencyEnd.Text = "N/A";
                    labelTotalProfitLoss.Text = "N/A";
                    labelTotalProfitLoss.ForeColor = Color.Black;
                    textBoxLRSize.Enabled = false;
                    textBoxLRTailSize.Enabled = false;
                    textBoxLRHeadSize.Enabled = false;
                    textBoxR2TailBarrier.Enabled = false;
                    textBoxR2HeadBarrier.Enabled = false;
                    textBoxSlopeTailBarrier.Enabled = false;
                    textBoxSlopeHeadBarrier.Enabled = false;
                    textBoxBuyAmount.Enabled = false;
                    textBoxBuyDuration.Enabled = false;
                    textBoxCutoffLose.Enabled = false;
                    textBoxCutoffWin.Enabled = false;
                    comboBoxUnit.Enabled = false;
                    buttonStartStop.Enabled = false;
                    buttonReset.Enabled = false;
                    buttonCalc.Enabled = false;
                    buttonSwitch.Enabled = false;
                    checkBoxReverse.Enabled = false;
                    checkBoxAllowStraight.Enabled = false;
                    checkBoxCutoffLose.Enabled = false;
                    checkBoxCutoffWin.Enabled = false;
                }));
            }
        }

        private void OnBalanceChanges(Session.Current.BalanceChangesEventArgs args)
        {
            if (Session.LoggedIn)
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    labelBalance.Text = args.Balance.ToString("0.################") + " " + args.Currency;
                }));
            }
            else
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    labelBalance.Text = "N/A";
                }));
            }
        }

        private void OnActiveSymbolAvailable(Session.Current.ActiveSymbolAvaliableEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                dataGridViewMarket.Rows.Clear();
                foreach (SymbolQuotes symbolQuotes in Session.Current.SymbolsQuotes)
                {
                    DataGridViewRow row = new DataGridViewRow { Tag = symbolQuotes.ActiveSymbol.Symbol };
                    row.CreateCells(
                        dataGridViewMarket,
                        //symbolQuotes.ActiveSymbol.MarketDisplayName + " - " +
                        symbolQuotes.ActiveSymbol.DisplayName);
                    dataGridViewMarket.Rows.Add(row);
                }
            }));
        }

        private void OnPurchase(Session.Current.PurchaseEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                labelAccuracy.Text = Session.Current.PurchasesCount > 0 ? ((Session.Current.Accuracy * 100).ToString("0.##") + "%") : "N/A";
                labelPurchases.Text = Session.Current.PurchasesCount.ToString();
                labelTotalProfitLoss.Text =
                    Session.Current.TotalSessionAmount.ToString("0.################") + " " +
                    Session.Current.Currency;
                if (Session.Current.TotalSessionAmount < 0)
                {
                    labelTotalProfitLoss.ForeColor = Color.Red;
                }
                else
                {
                    labelTotalProfitLoss.ForeColor = Color.Green;
                }
                if (args.Purchase.PurchaseType == PurchaseType.Ongoing)
                {
                    DataGridViewRow row = new DataGridViewRow { Tag = args.Purchase.BuyTransaction.TransactionId };
                    row.CreateCells(
                        dataGridViewPurchases,
                        args.Purchase.BuyTransaction.TransactionTime.ToString("MM/dd/yyyy HH:mm:ss"),
                        args.Purchase.ActiveSymbol == null ? "Unknown" : args.Purchase.ActiveSymbol.DisplayName,
                        "Ongoing");
                    dataGridViewPurchases.Rows.Insert(0, row);
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridViewPurchases.Rows)
                    {
                        if ((long)row.Tag == args.Purchase.BuyTransaction.TransactionId)
                        {
                            row.Cells[2].Value = args.Purchase.Amount.ToString("0.################") + " " + args.Purchase.BuyTransaction.Currency;
                            row.Cells[2].Style.ForeColor = args.Purchase.PurchaseType == PurchaseType.Win ? Color.Green : Color.Red;
                            break;
                        }
                    }
                }
            }));
        }

        private void OnQuote(Session.Current.QuoteEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                labelWinCount.Text = Session.Current.TestWins.ToString();
                labelLoseCount.Text = Session.Current.TestLoses.ToString();
                labelExpectedAccuracy.Text = (Session.Current.ExpectedAccuracy * 100).ToString("0.##") + "%";
                labelExpectedCount.Text = Session.Current.ExpectedPurchases.ToString();
                if (Session.Current.Calculating)
                {
                    buttonCalc.Text = "Calculating: " + Session.Current.CalculatingProgress.ToString() + " (Stop)";
                    if (Session.Current.CalculatingProgress == Session.Current.LRSize - 1)
                    {
                        buttonCalc.Text = "Calculate";
                        buttonCalc.Enabled = false;
                    }
                    else
                    {
                        buttonCalc.Enabled = true;
                    }
                }
            }));
            foreach (DataGridViewRow row in dataGridViewMarket.Rows)
            {
                if ((string)row.Tag == args.SymbolQuotes.ActiveSymbol.Symbol)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        if (Disposing) return;
                        //row.Cells[2].Value = args.SymbolQuotes.MaxValue().ToString("0.##");
                        //row.Cells[3].Value = args.SymbolQuotes.HeadMaxSlope.ToString("0.##") + " - " + args.SymbolQuotes.HeadMaxSlopeDateTime.ToString();
                        row.Cells[1].Value = args.SymbolQuotes.TailR2.ToString("0.##");
                        row.Cells[2].Value = args.SymbolQuotes.HeadR2.ToString("0.##");
                        row.Cells[3].Value = args.SymbolQuotes.TailSlope.ToString("0.##");
                        row.Cells[4].Value = args.SymbolQuotes.HeadSlope.ToString("0.##");
                    }));
                    break;
                }
            }
        }

        //bool freeze = false;

        private void OnCalculateStateChange(Session.Current.CalculateEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                if (args.Calculating)
                {
                    buttonCalc.Text = "Calculating (Stop)";
                    buttonCalc.Enabled = false;
                }
                else
                {
                    buttonCalc.Text = "Calculate";
                    buttonCalc.Enabled = true;
                }
                if (!Session.Current.Started)
                {
                    buttonStartStop.Enabled = !args.Calculating;
                    textBoxLRSize.Enabled = !args.Calculating;
                    textBoxLRTailSize.Enabled = !args.Calculating;
                    textBoxLRHeadSize.Enabled = !args.Calculating;
                    textBoxR2TailBarrier.Enabled = !args.Calculating;
                    textBoxR2HeadBarrier.Enabled = !args.Calculating;
                    textBoxSlopeTailBarrier.Enabled = !args.Calculating;
                    textBoxSlopeHeadBarrier.Enabled = !args.Calculating;
                    textBoxBuyAmount.Enabled = !args.Calculating;
                    textBoxBuyDuration.Enabled = !args.Calculating;
                    comboBoxUnit.Enabled = !args.Calculating;
                    checkBoxReverse.Enabled = !args.Calculating;
                    checkBoxAllowStraight.Enabled = !args.Calculating;
                    checkBoxCutoffLose.Enabled = !args.Calculating;
                    textBoxCutoffLose.Enabled = checkBoxCutoffLose.Checked && !args.Calculating;
                    checkBoxCutoffWin.Enabled = !args.Calculating;
                    textBoxCutoffWin.Enabled = checkBoxCutoffWin.Checked && !args.Calculating;
                }
            }));
            //if (!args.Calculating && !freeze)
            //{
            //    freeze = true;

            //    File.AppendAllText(@"d:\daaamp.txt",
            //        "Accuracy: " + (Session.Current.ExpectedAccuracy * 100).ToString("0.##") +
            //        " Count: " + (Session.Current.TestWins + Session.Current.TestLoses) +
            //        " Head Size: " + Session.Current.LRHeadSize +
            //        " Head R2: " + Session.Current.LRHeadR2Barrier +
            //        " Head Slope: " + Session.Current.LRHeadSlopeBarrier +
            //        " Duration: " + Session.Current.BuyDuration + "\n");

            //    if (Session.Current.BuyDuration >= 10)
            //    {
            //        Session.Current.BuyDuration = 1;
            //        if (Session.Current.LRHeadSlopeBarrier >= 120)
            //        {
            //            Session.Current.LRHeadSlopeBarrier = 0;
            //            if (Session.Current.LRHeadR2Barrier >= 100)
            //            {
            //                Session.Current.LRHeadR2Barrier = 0;
            //                Session.Current.LRHeadSize++;
            //            }
            //            else
            //            {
            //                Session.Current.LRHeadR2Barrier += 10;
            //            }
            //        }
            //        else
            //        {
            //            Session.Current.LRHeadSlopeBarrier += 10;
            //        }
            //    }
            //    else
            //    {
            //        Session.Current.BuyDuration++;
            //    }

            //    Task.Run(async delegate
            //    {
            //        await Task.Delay(2000);
            //        Invoke(new MethodInvoker(delegate
            //        {
            //            buttonCalc.PerformClick();
            //            freeze = false;
            //        }));
            //    });
            //}
        }

        private void OnCutoff(Session.Current.CutoffEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (args.WasWin)
                {
                    buttonReset.Text = "Cutoff Reached Win Barrier (Reset)";
                    buttonReset.FlatStyle = FlatStyle.Popup;
                    buttonReset.BackColor = Color.LightGreen;
                }
                else
                {
                    buttonReset.Text = "Cutoff Reached Lose Barrier (Reset)";
                    buttonReset.FlatStyle = FlatStyle.Popup;
                    buttonReset.BackColor = Color.Red;
                }
            }));
        }

        private void ButtonSwitch_Click(object sender, EventArgs e)
        {
            using (SwitchAccount switchAccount = new SwitchAccount())
            {
                switchAccount.ShowDialog();
                if (!Session.Current.Exist)
                {
                    Close();
                }
            }
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset current session?", "Reset Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                textBoxLRSize.Enabled = false;
                textBoxLRTailSize.Enabled = false;
                textBoxLRHeadSize.Enabled = false;
                textBoxR2TailBarrier.Enabled = false;
                textBoxR2HeadBarrier.Enabled = false;
                textBoxSlopeTailBarrier.Enabled = false;
                textBoxSlopeHeadBarrier.Enabled = false;
                textBoxBuyAmount.Enabled = false;
                textBoxBuyDuration.Enabled = false;
                textBoxCutoffLose.Enabled = false;
                textBoxCutoffWin.Enabled = false;
                comboBoxUnit.Enabled = false;
                buttonStartStop.Enabled = false;
                buttonReset.Enabled = false;
                buttonCalc.Enabled = false;
                checkBoxReverse.Enabled = false;
                checkBoxAllowStraight.Enabled = false;
                checkBoxCutoffLose.Enabled = false;
                checkBoxCutoffWin.Enabled = false;
                Session.Current.Reset();
            }
        }

        private void ButtonCalc_Click(object sender, EventArgs e)
        {
            if (Session.Current.Calculating)
            {
                buttonCalc.Text = "Calculate";
                Session.Current.Calculating = false;
            }
            else
            {
                textBoxLRSize.Enabled = false;
                textBoxLRTailSize.Enabled = false;
                textBoxLRHeadSize.Enabled = false;
                textBoxR2TailBarrier.Enabled = false;
                textBoxR2HeadBarrier.Enabled = false;
                textBoxSlopeTailBarrier.Enabled = false;
                textBoxSlopeHeadBarrier.Enabled = false;
                textBoxBuyAmount.Enabled = false;
                textBoxBuyDuration.Enabled = false;
                textBoxCutoffLose.Enabled = false;
                textBoxCutoffWin.Enabled = false;
                comboBoxUnit.Enabled = false;
                buttonStartStop.Enabled = false;
                checkBoxReverse.Enabled = false;
                checkBoxAllowStraight.Enabled = false;
                checkBoxCutoffLose.Enabled = false;
                checkBoxCutoffWin.Enabled = false;
                buttonCalc.Text = "Stop";
                Session.Current.Calculate();
            }
            buttonCalc.Enabled = false;
        }

        private void ButtonStartStop_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;
            if (!Session.Current.Started)
            {
                result = MessageBox.Show("Are you sure you want to start trading?", "Trading Confirmation", MessageBoxButtons.YesNo);
            }
            if (result == DialogResult.Yes)
            {
                Session.Current.Started = !Session.Current.Started;
            }
        }

        private void ConfigOnLeave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox == textBoxLRSize)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRSize.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRSize = Convert.ToInt32(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRSize.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxLRTailSize)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRTailSize.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRTailSize = Convert.ToInt32(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRTailSize.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxLRHeadSize)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRHeadSize.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRHeadSize = Convert.ToInt32(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRHeadSize.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxR2TailBarrier)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRTailR2Barrier.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRTailR2Barrier = Convert.ToDouble(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRTailR2Barrier.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxR2HeadBarrier)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRHeadR2Barrier.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRHeadR2Barrier = Convert.ToDouble(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRHeadR2Barrier.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxSlopeTailBarrier)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRTailSlopeBarrier.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRTailSlopeBarrier = Convert.ToDouble(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRTailSlopeBarrier.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxSlopeHeadBarrier)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.LRHeadSlopeBarrier.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.LRHeadSlopeBarrier = Convert.ToDouble(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.LRHeadSlopeBarrier.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxBuyAmount)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.BuyAmount.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.BuyAmount = Convert.ToDecimal(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.BuyAmount.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxBuyDuration)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.BuyDuration.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.BuyDuration = Convert.ToInt32(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.BuyDuration.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxCutoffLose)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.CutoffLoseAmount.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.CutoffLoseAmount = Convert.ToDecimal(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.CutoffLoseAmount.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
            else if (textBox == textBoxCutoffWin)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = Session.Current.CutoffWinAmount.ToString();
                }
                else
                {
                    try
                    {
                        Session.Current.CutoffWinAmount = Convert.ToDecimal(textBox.Text);
                    }
                    catch
                    {
                        textBox.Text = Session.Current.CutoffWinAmount.ToString();
                        MessageBox.Show("The values you entered is invalid", "Invalid Input", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void ComboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxUnit.SelectedIndex)
            {
                case 0:
                    Session.Current.BuyDurationUnit = Session.Current.DurationUnit.Ticks;
                    break;
                case 1:
                    Session.Current.BuyDurationUnit = Session.Current.DurationUnit.Seconds;
                    break;
                case 2:
                    Session.Current.BuyDurationUnit = Session.Current.DurationUnit.Minutes;
                    break;
                case 3:
                    Session.Current.BuyDurationUnit = Session.Current.DurationUnit.Hours;
                    break;
                case 4:
                    Session.Current.BuyDurationUnit = Session.Current.DurationUnit.Days;
                    break;
            }
        }

        private void CheckBox_CheckStateChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox == checkBoxReverse)
            {
                Session.Current.ReverseLogic = checkBoxReverse.Checked;
            }
            else if (checkBox == checkBoxAllowStraight)
            {
                Session.Current.AllowStraight = checkBoxAllowStraight.Checked;
            }
            else if (checkBox == checkBoxCutoffLose)
            {
                Session.Current.CutoffLoseEnable = checkBoxCutoffLose.Checked;
                textBoxCutoffLose.Enabled = checkBoxCutoffLose.Checked && !Session.Current.Calculating;
            }
            else if (checkBox == checkBoxCutoffWin)
            {
                Session.Current.CutoffWinEnable = checkBoxCutoffWin.Checked;
                textBoxCutoffWin.Enabled = checkBoxCutoffWin.Checked && !Session.Current.Calculating;
            }
        }

        #endregion
    }
}
