using DoggoWire.Models;
using DoggoWire.Services;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace WatchDoggo.Forms
{
    public partial class Trading : Form, Session.TradingInstance.ITradingInstanceUI
    {
        #region Properties

        private readonly List<Purchase> purchasesToDraw = new List<Purchase>();
        private readonly ChartValues<Quote> chartValues;
        private readonly ChartValues<Quote> chartLRValues;
        private readonly ChartValues<PredictionQuote> predictionValues;
        private bool labelsInit = false;
        private bool justBuy = false;
        private Session.TradingInstance instance;
        private readonly string symbol = "";

        #endregion

        #region Initializers

        public Trading(string activeSymbol)
        {
            symbol = activeSymbol;
            InitializeComponent();
            Session.OnConnectionChanges += OnConnectionChanges;
            Init();
            Charting.For<Quote>(Mappers.Xy<Quote>()
                .X(model => model.Epoch)
                .Y(model => model.Value)
                .Stroke(model => Brushes.Transparent)
                .Fill(model =>
                {
                    if (model.Transactions.Count > 0)
                    {
                        if (model.Transactions[0].Action == TransactionAction.Buy)
                        {
                            return Brushes.SkyBlue;
                        }
                        else if (model.Transactions[0].Action == TransactionAction.Sell)
                        {
                            if (model.Transactions[0].Amount == 0)
                            {
                                return Brushes.Red;
                            }
                            else
                            {
                                return Brushes.Green;
                            }
                        }
                        return Brushes.Transparent;
                    }
                    else
                    {
                        return Brushes.Transparent;
                    }
                }));
            Charting.For<PredictionQuote>(Mappers.Xy<PredictionQuote>()
                .X(model => model.Epoch)
                .Y(model => model.Value)
                .Stroke(model => Brushes.Transparent)
                .Fill(model => Brushes.Transparent));

            chartValues = new ChartValues<Quote>();
            chartLRValues = new ChartValues<Quote>();
            predictionValues = new ChartValues<PredictionQuote>();

            cartesianChart.Series.Add(new LineSeries
            {
                Values = chartValues,
                PointGeometrySize = 15,
                StrokeThickness = 2,
                DataLabels = false,
                LabelPoint = point =>
                {
                    Quote model = point.Instance as Quote;
                    if (model.Transactions.Count > 0)
                    {
                        if (model.Transactions[0].Action == TransactionAction.Buy)
                        {
                            if (model.Transactions[0].Longcode.Contains("higher"))
                            {
                                return "Call↗ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                            }
                            else if (model.Transactions[0].Longcode.Contains("lower"))
                            {
                                return "Put↘ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                            }
                        }
                        else if (model.Transactions[0].Action == TransactionAction.Sell)
                        {
                            return Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("0.##");
                        }
                        return model.Value.ToString("#.##");
                    }
                    else
                    {
                        return model.Value.ToString("#.##");
                    }
                }
            });
            cartesianChart.Series.Add(new LineSeries
            {
                Values = predictionValues,
                PointGeometrySize = 15,
                StrokeThickness = 2
            });
            cartesianChart.Series.Add(new LineSeries
            {
                Values = chartLRValues,
                PointGeometrySize = 15,
                StrokeThickness = 2,
                DataLabels = false,
                LabelPoint = point =>
                {
                    Quote model = point.Instance as Quote;
                    if (model.Transactions.Count > 0)
                    {
                        if (model.Transactions[0].Action == TransactionAction.Buy)
                        {
                            if (model.Transactions[0].Longcode.Contains("higher"))
                            {
                                return "Call↗ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                            }
                            else if (model.Transactions[0].Longcode.Contains("lower"))
                            {
                                return "Put↘ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                            }
                        }
                        else if (model.Transactions[0].Action == TransactionAction.Sell)
                        {
                            return Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("0.##");
                        }
                        return model.Value.ToString("#.##");
                    }
                    else
                    {
                        return model.Value.ToString("#.##");
                    }
                }
            });
            cartesianChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => Helpers.ConvertEpoch((long)value).ToString("mm:ss")
            });
            cartesianChart.AxisY.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => value.ToString("#.##"),
                Position = AxisPosition.RightTop
            });
        }

        private void Init()
        {
            if (instance != null) instance.Dispose();
            instance = Session.Current.StartTrading(symbol);
            instance.SetTradingInstanceUI(this);
            if (instance == null)
            {
                Close();
                return;
            }
            Enabled = false;
            instance.SetTradingInstanceUI(this);
            Text = instance.ActiveSymbol.DisplayName;
            labelCurrencyTrade.Text = Session.Current.Currency;
            labelWinCurr.Text = Session.Current.Currency;
            labelLoseCurr.Text = Session.Current.Currency;
            textBoxBuyAmount.Text = instance.BuyAmount.ToString();
            numericUpDownDuration.Value = instance.BuyDuration;
            textBoxSlopeThres.Text = instance.AutoBuySlopeBarrier.ToString();
            textBoxR2Thres.Text = instance.AutoBuyR2Barrier.ToString();
            checkBoxCuttoff.Checked = instance.CutoffEnable;
            textBoxLoseCutoff.Text = instance.CutoffLoseAmount.ToString();
            textBoxWinCutoff.Text = instance.CutoffWinAmount.ToString();
            numericUpDownLRSize.Value = instance.LRSize;
            switch (instance.BuyDurationUnit)
            {
                case DurationUnit.Ticks:
                    comboBoxUnit.SelectedIndex = 0;
                    break;
                case DurationUnit.Seconds:
                    comboBoxUnit.SelectedIndex = 1;
                    break;
                case DurationUnit.Minutes:
                    comboBoxUnit.SelectedIndex = 2;
                    break;
                case DurationUnit.Hours:
                    comboBoxUnit.SelectedIndex = 3;
                    break;
                case DurationUnit.Days:
                    comboBoxUnit.SelectedIndex = 4;
                    break;
            }
            RefreshLabels();
        }

        #endregion

        #region Callbacks

        private void OnConnectionChanges(Session.ConnectionChangesEventArgs args)
        {
            if (args.Connected)
            {
                instance.Start();
            }
            else
            {
                instance.Stop();
                Invoke(new MethodInvoker(delegate
                {
                    Text = instance.ActiveSymbol.DisplayName + " (Disconnected)";
                    Enabled = false;
                }));
            }
        }

        public void OnQuoteHistoryReady(Session.TradingInstance.QuoteHistoryEventArgs args)
        {
            chartValues.Clear();
            chartLRValues.Clear();
            foreach (Quote quote in args.Quotes)
            {
                AddChartValue(quote);
            }
            Invoke(new MethodInvoker(delegate
            {
                Text = instance.ActiveSymbol.DisplayName;
                Enabled = true;
            }));
        }

        public void OnQuote(Session.TradingInstance.QuoteEventArgs args)
        {
            AddChartValue(args.Quote);
            if (justBuy)
            {
                justBuy = false;
                Invoke(new MethodInvoker(delegate
                {
                    buttonUp.Enabled = true;
                    buttonDown.Enabled = true;
                }));
            }
            if (instance.LRReady)
            {
                predictionValues.Clear();
                for (int i = 0; i < instance.ConvertToTicks(instance.BuyDurationUnit, instance.BuyDuration) + 1; i++)
                {
                    Quote latest = instance[instance.Count - 1];
                    double value = i * instance.LRSlope + latest.Value;
                    long epoch = latest.Epoch + (instance.TickDuration * i);
                    predictionValues.Add(new PredictionQuote(value, epoch));
                }
                Invoke(new MethodInvoker(delegate
                {
                    labelSlope.Text = "Slope = " + instance.LRSlope.ToString();
                    labelR2.Text = "RS = " + instance.LRR2.ToString();
                }));
            }
        }

        public void OnTransaction(Session.TradingInstance.PurchaseEventArgs args)
        {
            purchasesToDraw.Add(args.Purchase);
            Invoke(new MethodInvoker(RefreshLabels));
        }

        public void OnCutoff(Session.TradingInstance.CutoffEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                new Cutoff(instance, Init).Show();
            }));
        }

        public void OnAutoBuyChanges(Session.TradingInstance.AutoBuyChangesArgs args)
        {
            try
            {
                buttonAutoBuy.Text = instance.AutoBuyEnable ? "Stop Auto Buy" : "Start Auto Buy";
                textBoxBuyAmount.Enabled = !instance.AutoBuyEnable;
                numericUpDownDuration.Enabled = !instance.AutoBuyEnable;
                comboBoxUnit.Enabled = !instance.AutoBuyEnable;
                textBoxR2Thres.Enabled = !instance.AutoBuyEnable;
                textBoxSlopeThres.Enabled = !instance.AutoBuyEnable;
                checkBoxCuttoff.Enabled = !instance.AutoBuyEnable;
                textBoxWinCutoff.Enabled = !instance.AutoBuyEnable;
                textBoxLoseCutoff.Enabled = !instance.AutoBuyEnable;
                numericUpDownLRSize.Enabled = !instance.AutoBuyEnable;
                labelCurrencyTrade.Enabled = !instance.AutoBuyEnable;
                labelWinCurr.Enabled = !instance.AutoBuyEnable;
                labelLoseCurr.Enabled = !instance.AutoBuyEnable;
                label9.Enabled = !instance.AutoBuyEnable;
                label10.Enabled = !instance.AutoBuyEnable;
                label2.Enabled = !instance.AutoBuyEnable;
                label3.Enabled = !instance.AutoBuyEnable;
                label4.Enabled = !instance.AutoBuyEnable;
                label12.Enabled = !instance.AutoBuyEnable;
                label7.Enabled = !instance.AutoBuyEnable;
                label11.Enabled = !instance.AutoBuyEnable;
                label6.Enabled = !instance.AutoBuyEnable;
            }
            catch
            {
                Invoke(new MethodInvoker(delegate
                {
                    buttonAutoBuy.Text = instance.AutoBuyEnable ? "Stop Auto Buy" : "Start Auto Buy";
                    textBoxBuyAmount.Enabled = !instance.AutoBuyEnable;
                    numericUpDownDuration.Enabled = !instance.AutoBuyEnable;
                    comboBoxUnit.Enabled = !instance.AutoBuyEnable;
                    textBoxR2Thres.Enabled = !instance.AutoBuyEnable;
                    textBoxSlopeThres.Enabled = !instance.AutoBuyEnable;
                    checkBoxCuttoff.Enabled = !instance.AutoBuyEnable;
                    textBoxWinCutoff.Enabled = !instance.AutoBuyEnable;
                    textBoxLoseCutoff.Enabled = !instance.AutoBuyEnable;
                    numericUpDownLRSize.Enabled = !instance.AutoBuyEnable;
                    labelCurrencyTrade.Enabled = !instance.AutoBuyEnable;
                    labelWinCurr.Enabled = !instance.AutoBuyEnable;
                    labelLoseCurr.Enabled = !instance.AutoBuyEnable;
                    label9.Enabled = !instance.AutoBuyEnable;
                    label10.Enabled = !instance.AutoBuyEnable;
                    label2.Enabled = !instance.AutoBuyEnable;
                    label3.Enabled = !instance.AutoBuyEnable;
                    label4.Enabled = !instance.AutoBuyEnable;
                    label12.Enabled = !instance.AutoBuyEnable;
                    label7.Enabled = !instance.AutoBuyEnable;
                    label11.Enabled = !instance.AutoBuyEnable;
                    label6.Enabled = !instance.AutoBuyEnable;
                }));
            }
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            instance.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            instance.Dispose();
            Session.OnConnectionChanges -= OnConnectionChanges;
            base.OnClosing(e);
        }

        #endregion

        #region Methods

        private void Call()
        {
            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            justBuy = true;
            instance.Buy(ContractType.Call);
        }
        private void Put()
        {
            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            justBuy = true;
            instance.Buy(ContractType.Put);
        }

        private void RefreshLabels()
        {
            labelProfit.Text = "Current Profit: " + Session.Current.Currency + " " + instance.TotalSessionAmount.ToString();
            if (instance.TotalSessionAmount >= 0)
            {
                labelProfit.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                labelProfit.ForeColor = System.Drawing.Color.Red;
            }
            labelAccuracy.Text = labelsInit ? "Trading Accuracy: " + (instance.Accuracy * 100).ToString("0.##") + "% (" + instance.WinCount.ToString() + "/" + instance.PurchasesCount.ToString() + ")" :
                                              "Trading Accuracy: N/A";
            if (!labelsInit) labelsInit = true;
        }

        private void AddChartValue(Quote quote)
        {
            if (chartLRValues.Any(item => item.Epoch == quote.Epoch)) return;
            if (chartValues.Any(item => item.Epoch == quote.Epoch)) return;
            Quote quoteBeforeLR = chartLRValues.LastOrDefault(item => item.Epoch < quote.Epoch);
            if (chartLRValues.Contains(quoteBeforeLR))
            {
                chartLRValues.Insert(chartLRValues.IndexOf(quoteBeforeLR) + 1, quote);
            }
            else
            {
                chartLRValues.Add(quote);
            }
            if (instance.Size - instance.LRSize < instance.Count) chartLRValues.RemoveAt(0);
            while (chartLRValues.Count >= instance.LRSize)
            {
                Quote quoteRemove = chartLRValues[0];
                chartLRValues.RemoveAt(0);
                Quote quoteBefore = chartValues.LastOrDefault(item => item.Epoch < quoteRemove.Epoch);
                if (chartValues.Contains(quoteBefore))
                {
                    chartValues.Insert(chartValues.IndexOf(quoteBefore) + 1, quoteRemove);
                }
                else
                {
                    chartValues.Add(quoteRemove);
                }
            }
            while (chartValues.Count > instance.Size - instance.LRSize)
            {
                Quote quoteRemove = chartValues[chartValues.Count - 1];
                chartValues.RemoveAt(chartValues.Count - 1);
                Quote quoteAfter = chartLRValues.LastOrDefault(item => item.Epoch < quoteRemove.Epoch);
                if (chartLRValues.Contains(quoteAfter))
                {
                    chartLRValues.Insert(chartLRValues.IndexOf(quoteAfter) - 1, quoteRemove);
                }
                else
                {
                    chartLRValues.Insert(0, quoteRemove);
                }
            }
            while (chartLRValues.Count + chartValues.Count >= instance.Size) chartValues.RemoveAt(0);
            if (instance.Size - instance.LRSize < instance.Count)
            {
                chartLRValues.Insert(0, instance[instance.Size - instance.LRSize - 1]);
            }
        }

        #endregion

        #region UIHandler

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            Call();
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            Put();
        }

        private void TextBoxBuyAmount_TextChanged(object sender, EventArgs e)
        {
            if (textBoxBuyAmount.Text.Equals(instance.BuyAmount.ToString())) return;
            if (decimal.TryParse(textBoxBuyAmount.Text, out decimal result))
            {
                instance.BuyAmount = result;
            }
        }

        private void TextBoxBuyAmount_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxBuyAmount.Text, out decimal result))
            {
                instance.BuyAmount = result;
            }
            else
            {
                textBoxBuyAmount.Text = instance.BuyAmount.ToString();
            }
        }

        private void NumericUpDownDuration_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownDuration.Value == instance.BuyDuration) return;
            numericUpDownDuration.Value = (int)numericUpDownDuration.Value;
            instance.BuyDuration = (int)numericUpDownDuration.Value;
        }

        private void ComboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxUnit.SelectedIndex)
            {
                case 0:
                    instance.BuyDurationUnit = DurationUnit.Ticks;
                    break;
                case 1:
                    instance.BuyDurationUnit = DurationUnit.Seconds;
                    break;
                case 2:
                    instance.BuyDurationUnit = DurationUnit.Minutes;
                    break;
                case 3:
                    instance.BuyDurationUnit = DurationUnit.Hours;
                    break;
                case 4:
                    instance.BuyDurationUnit = DurationUnit.Days;
                    break;
            }
        }

        private void CheckBoxCuttoff_CheckedChanged(object sender, EventArgs e)
        {
            instance.CutoffEnable = checkBoxCuttoff.Checked;
        }

        private void TextBoxR2Thres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxR2Thres.Text.Equals(instance.AutoBuyR2Barrier.ToString())) return;
            if (double.TryParse(textBoxR2Thres.Text, out double result))
            {
                instance.AutoBuyR2Barrier = result;
            }
        }

        private void TextBoxR2Thres_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxR2Thres.Text, out double result))
            {
                instance.AutoBuyR2Barrier = result;
            }
            else
            {
                textBoxR2Thres.Text = instance.AutoBuyR2Barrier.ToString();
            }
        }

        private void TextBoxSlopeThres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSlopeThres.Text.Equals(instance.AutoBuySlopeBarrier.ToString())) return;
            if (double.TryParse(textBoxSlopeThres.Text, out double result))
            {
                instance.AutoBuySlopeBarrier = result;
            }
        }

        private void TextBoxSlopeThres_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxSlopeThres.Text, out double result))
            {
                instance.AutoBuySlopeBarrier = result;
            }
            else
            {
                textBoxSlopeThres.Text = instance.AutoBuySlopeBarrier.ToString();
            }
        }

        private void TextBoxWinCutoff_TextChanged(object sender, EventArgs e)
        {
            if (textBoxWinCutoff.Text.Equals(instance.CutoffWinAmount.ToString())) return;
            if (decimal.TryParse(textBoxWinCutoff.Text, out decimal result))
            {
                instance.CutoffWinAmount = result;
            }
        }

        private void TextBoxWinCutoff_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxWinCutoff.Text, out decimal result))
            {
                instance.CutoffWinAmount = result;
            }
            else
            {
                textBoxWinCutoff.Text = instance.CutoffWinAmount.ToString();
            }
        }

        private void TextBoxLoseCutoff_TextChanged(object sender, EventArgs e)
        {
            if (textBoxLoseCutoff.Text.Equals(instance.CutoffLoseAmount.ToString())) return;
            if (decimal.TryParse(textBoxLoseCutoff.Text, out decimal result))
            {
                instance.CutoffLoseAmount = result;
            }
        }

        private void TextBoxLoseCutoff_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBoxLoseCutoff.Text, out decimal result))
            {
                instance.CutoffLoseAmount = result;
            }
            else
            {
                textBoxLoseCutoff.Text = instance.CutoffLoseAmount.ToString();
            }
        }

        private void NumericUpDownLRSize_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownLRSize.Value == instance.LRSize) return;
            numericUpDownLRSize.Value = (int)numericUpDownLRSize.Value;
            instance.LRSize = (int)numericUpDownLRSize.Value;
        }

        private void ButtonAutoBuy_Click(object sender, EventArgs e)
        {
            instance.AutoBuyEnable = instance.AutoBuyEnable ? false : true;
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to reset current session?", "Reset Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Init();
            }
        }

        #endregion
    }
}
