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

        private readonly ChartValues<Quote> chartValues;
        private readonly ChartValues<PredictionQuote> predictionHeadValues;
        private readonly ChartValues<PredictionQuote> predictionTailValues;
        private readonly AxisSection headAxisSection;
        private readonly AxisSection tailAxisSection;
        private readonly string symbol = "";
        private bool labelsInit = false;
        private bool justBuy = false;
        private Session.TradingInstance instance;

        private readonly SolidColorBrush transparentBrush = Brushes.Transparent;
        private readonly SolidColorBrush blackBrush = Brushes.Black;
        private readonly SolidColorBrush skyBlueBrush = Brushes.SkyBlue;
        private readonly SolidColorBrush blueBrush = Brushes.Blue;
        private readonly SolidColorBrush redBrush = Brushes.Red;
        private readonly SolidColorBrush greenBrush = Brushes.Green;
        private readonly SolidColorBrush orangeBrush = Brushes.Orange;

        #endregion

        #region Initializers

        public Trading(string activeSymbol)
        {
            symbol = activeSymbol;
            InitializeComponent();

            transparentBrush.Freeze();
            blackBrush.Freeze();
            skyBlueBrush.Freeze();
            blueBrush.Freeze();
            redBrush.Freeze();
            greenBrush.Freeze();
            orangeBrush.Freeze();

            Charting.For<Quote>(Mappers.Xy<Quote>()
                .X(model => model.Epoch)
                .Y(model => model.Value)
                .Stroke(model => transparentBrush)
                .Fill(model =>
                {
                    if (model.MockBuys.Count > 0) return blackBrush;
                    else if (model.Transactions.Count > 0)
                    {
                        if (model.Transactions[0].Action == TransactionAction.Buy) return skyBlueBrush;
                        else if (model.Transactions[0].Action == TransactionAction.Sell)
                        {
                            if (model.Transactions[0].Amount == 0) return redBrush;
                            else return greenBrush;
                        }
                    }
                    return transparentBrush;
                }));
            Charting.For<PredictionQuote>(Mappers.Xy<PredictionQuote>()
                .X(model => model.Epoch)
                .Y(model => model.Value)
                .Stroke(model => transparentBrush)
                .Fill(model => transparentBrush));

            chartValues = new ChartValues<Quote>();
            predictionHeadValues = new ChartValues<PredictionQuote>();
            predictionTailValues = new ChartValues<PredictionQuote>();
            headAxisSection = new AxisSection
            {
                Fill = new SolidColorBrush
                {
                    Color = redBrush.Color,
                    Opacity = .4
                }
            };
            tailAxisSection = new AxisSection
            {
                Fill = new SolidColorBrush
                {
                    Color = orangeBrush.Color,
                    Opacity = .4
                }
            };

            cartesianChart.Hoverable = false;
            cartesianChart.DisableAnimations = true;
            cartesianChart.Series.Add(new LineSeries
            {
                Values = chartValues,
                PointGeometrySize = 15,
                StrokeThickness = 2,
                DataLabels = false,
                LabelPoint = point =>
                {
                    Quote model = point.Instance as Quote;
                    if (model.MockBuys.Count > 0)
                    {
                        if (model.MockBuys[0].ContractType == ContractType.Call)
                        {
                            return "Call↗ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                        }
                        else if (model.MockBuys[0].ContractType == ContractType.Put)
                        {
                            return "Put↘ " + Session.Current.Currency + " " + model.Transactions[0].Amount.ToString("#.##");
                        }
                    }
                    else if (model.Transactions.Count > 0)
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
                    }
                    return model.Value.ToString("#.##");
                }
            });
            cartesianChart.Series.Add(new LineSeries
            {
                Values = predictionHeadValues,
                Fill = transparentBrush,
                Stroke = redBrush,
                PointGeometry = null
            });
            cartesianChart.Series.Add(new LineSeries
            {
                Values = predictionTailValues,
                Fill = transparentBrush,
                Stroke = orangeBrush,
                PointGeometry = null
            });
            cartesianChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => Helpers.ConvertEpoch((long)value).ToString("mm:ss"),
                Sections = new SectionsCollection { headAxisSection, tailAxisSection }
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
            instance = Session.Current.StartTrading(this, symbol);
            if (instance == null)
            {
                Close();
                return;
            }
            instance.Restart();
            panelMain.Enabled = false;
            Text = instance.ActiveSymbol.DisplayName;
            labelCurrencyTrade.Text = Session.Current.Currency;
            labelWinCurr.Text = Session.Current.Currency;
            labelLoseCurr.Text = Session.Current.Currency;
            textBoxBuyAmount.Text = instance.BuyAmount.ToString();
            numericUpDownDuration.Value = instance.BuyDuration;
            textBoxHeadSlopeThres.Text = instance.HeadSlopeBarrier.ToString();
            textBoxHeadR2Thres.Text = instance.HeadR2Barrier.ToString();
            textBoxTailSlopeThres.Text = instance.TailSlopeBarrier.ToString();
            textBoxTailR2Thres.Text = instance.TailR2Barrier.ToString();
            checkBoxCuttoff.Checked = instance.CutoffEnable;
            checkBoxReverse.Checked = instance.ReverseLogic;
            checkBoxBuyBan.Checked = instance.BuyBanEnable;
            textBoxLoseCutoff.Text = instance.CutoffLoseAmount.ToString();
            textBoxWinCutoff.Text = instance.CutoffWinAmount.ToString();
            numericUpDownBuyBanLose.Value = instance.BuyBanLose;
            numericUpDownBuyBanDuration.Value = instance.BuyBanDuration;
            numericUpDownHeadLRSize.Value = instance.LRHeadSize;
            numericUpDownTailLRSize.Value = instance.LRTailSize;
            numericUpDownOffset.Value = instance.LROffset;
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

        public void OnConnectionChanges(Session.TradingInstance.ConnectionChangesEventArgs args)
        {
            if (args.Connected)
            {
                Invoke(new MethodInvoker(delegate
                {
                    Text = instance.ActiveSymbol.DisplayName;
                    panelMain.Enabled = true;
                }));
            }
            else
            {
                Invoke(new MethodInvoker(delegate
                {
                    Text = instance.ActiveSymbol.DisplayName + " (Disconnected)";
                    panelMain.Enabled = false;
                }));
            }
        }

        public void OnQuoteHistoryReady(Session.TradingInstance.QuoteHistoryEventArgs args)
        {
            chartValues.Clear();
            predictionHeadValues.Clear();
            predictionTailValues.Clear();
            foreach (Quote quote in args.Quotes)
            {
                AddChartValue(quote);
            }
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
                predictionHeadValues.Clear();
                predictionTailValues.Clear();
                for (int i = 0; i < instance.ConvertToTicks(instance.BuyDurationUnit, instance.BuyDuration) + 1; i++)
                {
                    Quote latestHead = instance[instance.Count - 1];
                    double value = i * instance.LRHeadSlope + latestHead.Value;
                    long epoch = latestHead.Epoch + (instance.TickEpochDuration * i);
                    predictionHeadValues.Add(new PredictionQuote(value, epoch));
                }
                for (int i = 0; i < instance.ConvertToTicks(instance.BuyDurationUnit, instance.BuyDuration) + 1; i++)
                {
                    Quote latestTail = instance[instance.Count - instance.LROffset - 1];
                    double value = i * instance.LRTailSlope + latestTail.Value;
                    long epoch = latestTail.Epoch + (instance.TickEpochDuration * i);
                    predictionTailValues.Add(new PredictionQuote(value, epoch));
                }
                Invoke(new MethodInvoker(delegate
                {
                    labelSlopeHead.Text = "Head Slope = " + instance.LRHeadSlope.ToString();
                    labelR2Head.Text = "Head RS = " + instance.LRHeadR2.ToString();
                    labelSlopeTail.Text = "Tail Slope = " + instance.LRTailSlope.ToString();
                    labelR2Tail.Text = "Tail RS = " + instance.LRTailR2.ToString();
                }));
            }
        }

        public void OnTransaction(Session.TradingInstance.PurchaseEventArgs args)
        {
            Invoke(new MethodInvoker(RefreshLabels));
        }

        public void OnMockTransaction(Session.TradingInstance.MockPurchaseEventArgs args)
        {

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
                textBoxHeadR2Thres.Enabled = !instance.AutoBuyEnable;
                textBoxHeadSlopeThres.Enabled = !instance.AutoBuyEnable;
                textBoxTailR2Thres.Enabled = !instance.AutoBuyEnable;
                textBoxTailSlopeThres.Enabled = !instance.AutoBuyEnable;
                checkBoxCuttoff.Enabled = !instance.AutoBuyEnable;
                checkBoxReverse.Enabled = !instance.AutoBuyEnable;
                checkBoxBuyBan.Enabled = !instance.AutoBuyEnable;
                textBoxWinCutoff.Enabled = !instance.AutoBuyEnable;
                textBoxLoseCutoff.Enabled = !instance.AutoBuyEnable;
                numericUpDownBuyBanLose.Enabled = !instance.AutoBuyEnable;
                numericUpDownBuyBanDuration.Enabled = !instance.AutoBuyEnable;
                numericUpDownHeadLRSize.Enabled = !instance.AutoBuyEnable;
                numericUpDownTailLRSize.Enabled = !instance.AutoBuyEnable;
                numericUpDownOffset.Enabled = !instance.AutoBuyEnable;
                labelCurrencyTrade.Enabled = !instance.AutoBuyEnable;
                labelWinCurr.Enabled = !instance.AutoBuyEnable;
                labelLoseCurr.Enabled = !instance.AutoBuyEnable;
                label9.Enabled = !instance.AutoBuyEnable;
                label10.Enabled = !instance.AutoBuyEnable;
                label2.Enabled = !instance.AutoBuyEnable;
                label3.Enabled = !instance.AutoBuyEnable;
                label4.Enabled = !instance.AutoBuyEnable;
                label7.Enabled = !instance.AutoBuyEnable;
                label11.Enabled = !instance.AutoBuyEnable;
                label6.Enabled = !instance.AutoBuyEnable;
                label5.Enabled = !instance.AutoBuyEnable;
                label1.Enabled = !instance.AutoBuyEnable;
                label8.Enabled = !instance.AutoBuyEnable;
                label12.Enabled = !instance.AutoBuyEnable;
                label14.Enabled = !instance.AutoBuyEnable;
                label13.Enabled = !instance.AutoBuyEnable;
            }
            catch
            {
                Invoke(new MethodInvoker(delegate
                {
                    buttonAutoBuy.Text = instance.AutoBuyEnable ? "Stop Auto Buy" : "Start Auto Buy";
                    textBoxBuyAmount.Enabled = !instance.AutoBuyEnable;
                    numericUpDownDuration.Enabled = !instance.AutoBuyEnable;
                    comboBoxUnit.Enabled = !instance.AutoBuyEnable;
                    textBoxHeadR2Thres.Enabled = !instance.AutoBuyEnable;
                    textBoxHeadSlopeThres.Enabled = !instance.AutoBuyEnable;
                    textBoxTailR2Thres.Enabled = !instance.AutoBuyEnable;
                    textBoxTailSlopeThres.Enabled = !instance.AutoBuyEnable;
                    checkBoxCuttoff.Enabled = !instance.AutoBuyEnable;
                    checkBoxReverse.Enabled = !instance.AutoBuyEnable;
                    checkBoxBuyBan.Enabled = !instance.AutoBuyEnable;
                    textBoxWinCutoff.Enabled = !instance.AutoBuyEnable;
                    textBoxLoseCutoff.Enabled = !instance.AutoBuyEnable;
                    numericUpDownBuyBanLose.Enabled = !instance.AutoBuyEnable;
                    numericUpDownBuyBanDuration.Enabled = !instance.AutoBuyEnable;
                    numericUpDownHeadLRSize.Enabled = !instance.AutoBuyEnable;
                    numericUpDownTailLRSize.Enabled = !instance.AutoBuyEnable;
                    numericUpDownOffset.Enabled = !instance.AutoBuyEnable;
                    labelCurrencyTrade.Enabled = !instance.AutoBuyEnable;
                    labelWinCurr.Enabled = !instance.AutoBuyEnable;
                    labelLoseCurr.Enabled = !instance.AutoBuyEnable;
                    label9.Enabled = !instance.AutoBuyEnable;
                    label10.Enabled = !instance.AutoBuyEnable;
                    label2.Enabled = !instance.AutoBuyEnable;
                    label3.Enabled = !instance.AutoBuyEnable;
                    label4.Enabled = !instance.AutoBuyEnable;
                    label7.Enabled = !instance.AutoBuyEnable;
                    label11.Enabled = !instance.AutoBuyEnable;
                    label6.Enabled = !instance.AutoBuyEnable;
                    label5.Enabled = !instance.AutoBuyEnable;
                    label1.Enabled = !instance.AutoBuyEnable;
                    label8.Enabled = !instance.AutoBuyEnable;
                    label12.Enabled = !instance.AutoBuyEnable;
                    label14.Enabled = !instance.AutoBuyEnable;
                    label13.Enabled = !instance.AutoBuyEnable;
                }));
            }
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Init();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            instance.Dispose();
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
            try
            {
                if (chartValues.Any(item => item.Epoch == quote.Epoch)) return;
                Quote quoteBefore = chartValues.LastOrDefault(item => item.Epoch < quote.Epoch);
                if (chartValues.Contains(quoteBefore))
                {
                    chartValues.Insert(chartValues.IndexOf(quoteBefore) + 1, quote);
                }
                else
                {
                    chartValues.Add(quote);
                }
                if (!checkBoxFreeze.Checked) while (chartValues.Count > instance.Size) chartValues.RemoveAt(0);
                Invoke(new MethodInvoker(delegate
                {
                    headAxisSection.Value = instance.LastEpoch - instance.LRHeadSize * instance.TickEpochDuration;
                    headAxisSection.SectionWidth = instance.LRHeadSize * instance.TickEpochDuration;
                    tailAxisSection.Value = instance.LastEpoch - instance.LRTailSize * instance.TickEpochDuration - instance.LROffset * instance.TickEpochDuration;
                    tailAxisSection.SectionWidth = instance.LRTailSize * instance.TickEpochDuration;
                }));
            }
            catch { }
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

        private void CheckBoxReverse_CheckedChanged(object sender, EventArgs e)
        {
            instance.ReverseLogic = checkBoxReverse.Checked;
        }

        private void TextBoxHeadR2Thres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxHeadR2Thres.Text.Equals(instance.HeadR2Barrier.ToString())) return;
            if (double.TryParse(textBoxHeadR2Thres.Text, out double result))
            {
                instance.HeadR2Barrier = result;
            }
        }

        private void TextBoxHeadR2Thres_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxHeadR2Thres.Text, out double result))
            {
                instance.HeadR2Barrier = result;
            }
            else
            {
                textBoxHeadR2Thres.Text = instance.HeadR2Barrier.ToString();
            }
        }

        private void TextBoxHeadSlopeThres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxHeadSlopeThres.Text.Equals(instance.HeadSlopeBarrier.ToString())) return;
            if (double.TryParse(textBoxHeadSlopeThres.Text, out double result))
            {
                instance.HeadSlopeBarrier = result;
            }
        }

        private void TextBoxHeadSlopeThres_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxHeadSlopeThres.Text, out double result))
            {
                instance.HeadSlopeBarrier = result;
            }
            else
            {
                textBoxHeadSlopeThres.Text = instance.HeadSlopeBarrier.ToString();
            }
        }

        private void TextBoxTailR2Thres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTailR2Thres.Text.Equals(instance.TailR2Barrier.ToString())) return;
            if (double.TryParse(textBoxTailR2Thres.Text, out double result))
            {
                instance.TailR2Barrier = result;
            }
        }

        private void TextBoxTailR2Thres_Leave(object sender, EventArgs e)
        {
            if (double.TryParse(textBoxTailR2Thres.Text, out double result))
            {
                instance.TailR2Barrier = result;
            }
            else
            {
                textBoxTailR2Thres.Text = instance.TailR2Barrier.ToString();
            }
        }

        private void TextBoxTailSlopeThres_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTailSlopeThres.Text.Equals(instance.TailSlopeBarrier.ToString())) return;
            if (double.TryParse(textBoxTailSlopeThres.Text, out double result))
            {
                instance.TailSlopeBarrier = result;
            }
        }

        private void TextBoxTailSlopeThres_Leave(object sender, EventArgs e)
        {
            if (textBoxTailSlopeThres.Text.Equals(instance.TailSlopeBarrier.ToString())) return;
            if (double.TryParse(textBoxTailSlopeThres.Text, out double result))
            {
                instance.TailSlopeBarrier = result;
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

        private void CheckBoxBuyBan_CheckedChanged(object sender, EventArgs e)
        {
            instance.BuyBanEnable = checkBoxBuyBan.Checked;
        }

        private void NumericUpDownBuyBanLose_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownBuyBanLose.Value == instance.BuyBanLose) return;
            numericUpDownBuyBanLose.Value = (int)numericUpDownBuyBanLose.Value;
            instance.BuyBanLose = (int)numericUpDownBuyBanLose.Value;
        }

        private void NumericUpDownBuyBanDuration_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownBuyBanDuration.Value == instance.BuyBanDuration) return;
            numericUpDownBuyBanDuration.Value = (int)numericUpDownBuyBanDuration.Value;
            instance.BuyBanDuration = (int)numericUpDownBuyBanDuration.Value;
        }

        private void NumericUpDownHeadLRSize_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownHeadLRSize.Value == instance.LRHeadSize) return;
            numericUpDownHeadLRSize.Value = (int)numericUpDownHeadLRSize.Value;
            instance.LRHeadSize = (int)numericUpDownHeadLRSize.Value;
        }

        private void NumericUpDownTailLRSize_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownTailLRSize.Value == instance.LRTailSize) return;
            numericUpDownTailLRSize.Value = (int)numericUpDownTailLRSize.Value;
            instance.LRTailSize = (int)numericUpDownTailLRSize.Value;
        }

        private void NumericUpDownOffset_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownOffset.Value == instance.LROffset) return;
            numericUpDownOffset.Value = (int)numericUpDownOffset.Value;
            instance.LROffset = (int)numericUpDownOffset.Value;
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
