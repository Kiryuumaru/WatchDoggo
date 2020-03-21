using DoggoWire.Models;
using DoggoWire.Services;
using LiveCharts;
using LiveCharts.Configurations;
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
        #region HelperClass

        public class QuoteMapper
        {
            public Quote Quote { get; private set; }
            public string Label { get; set; }
            public SolidColorBrush Stroke { get; set; }
            public SolidColorBrush Fill { get; set; }
            public QuoteMapper(Quote quote)
            {
                Quote = quote;
                Label = "";
                Stroke = Brushes.Transparent;
                Fill = Brushes.Transparent;
            }
        }

        #endregion

        #region Properties

        private readonly Session.TradingInstance instance;
        private readonly List<Purchase> purchasesToDraw = new List<Purchase>();
        private bool justBuy = false;
        public ChartValues<QuoteMapper> ChartValues { get; set; }

        #endregion

        public Trading(Session.TradingInstance tradingInstance)
        {
            InitializeComponent();
            Session.OnConnectionChanges += OnConnectionChanges;
            instance = tradingInstance;
            instance.SetTradingInstanceUI(this);
            Text = instance.ActiveSymbol.DisplayName;
            labelCurrencyTrade.Text = Session.Current.Currency;
            textBoxBuyAmount.Text = instance.BuyAmount.ToString();
            numericUpDownDuration.Value = instance.BuyDuration;
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

            Charting.For<QuoteMapper>(Mappers.Xy<QuoteMapper>()
                .X(model => model.Quote.Epoch)
                .Y(model => model.Quote.Value)
                .Stroke(model => model.Stroke)
                .Fill(model => model.Fill));

            ChartValues = new ChartValues<QuoteMapper>();

            cartesianChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = ChartValues,
                    PointGeometrySize = 15,
                    StrokeThickness = 2,
                    DataLabels = true,
                    LabelPoint = point =>
                    {
                        QuoteMapper mapper = point.Instance as QuoteMapper;
                        return mapper.Label;
                    }
                }
            };
            cartesianChart.AxisX.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => Helpers.ConvertEpoch((long)value).ToString("mm:ss"),
                Separator = new Separator
                {
                    Step = 4
                }
            });
            cartesianChart.AxisY.Add(new Axis
            {
                DisableAnimations = true,
                LabelFormatter = value => value.ToString("#.##")
            });
        }

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

        public void OnQuoteHistoryReady(Session.TradingInstance.QuoteHistoryEventArgs args)
        {
            foreach (Quote quote in args.Quotes)
            {
                if (!ChartValues.Any(item => item.Quote.Epoch == quote.Epoch)) ChartValues.Add(new QuoteMapper(quote));
            }
            Invoke(new MethodInvoker(delegate
            {
                Text = instance.ActiveSymbol.DisplayName;
                Enabled = true;
            }));
        }

        public void OnQuote(Session.TradingInstance.QuoteEventArgs args)
        {
            ChartValues.Add(new QuoteMapper(args.Quote));
            if (ChartValues.Count > instance.Size) ChartValues.RemoveAt(0);
            if (justBuy)
            {
                justBuy = false;
                Invoke(new MethodInvoker(delegate
                {
                    buttonUp.Enabled = true;
                    buttonDown.Enabled = true;
                }));
            }
            while (purchasesToDraw.Count > 0)
            {
                Purchase pur = purchasesToDraw.First();
                purchasesToDraw.Remove(pur);
                QuoteMapper quoteMapperRequest = null;
                QuoteMapper quoteMapperSell = null;
                foreach (QuoteMapper mapper in ChartValues)
                {
                    if (pur.SellTransaction != null)
                    {
                        if (quoteMapperSell == null) quoteMapperSell = mapper;
                        if (Math.Abs(mapper.Quote.Epoch - pur.SellTransaction.DateExpiry) <
                            Math.Abs(quoteMapperSell.Quote.Epoch - pur.SellTransaction.DateExpiry))
                        {
                            quoteMapperSell = mapper;
                        }
                    }
                    else if (pur.BuyResponse != null)
                    {
                        if (quoteMapperRequest == null) quoteMapperRequest = mapper;
                        if (Math.Abs(mapper.Quote.Epoch - pur.BuyResponse.Buy.StartTime) <
                            Math.Abs(quoteMapperRequest.Quote.Epoch - pur.BuyResponse.Buy.StartTime))
                        {
                            quoteMapperRequest = mapper;
                        }
                    }
                }
                if (quoteMapperRequest != null)
                {
                    quoteMapperRequest.Fill = Brushes.SkyBlue;
                    if (pur.BuyResponse.Request.Parameters.ContractType == ContractType.Call)
                    {
                        quoteMapperRequest.Label = "Call↗";
                    }
                    else
                    {
                        quoteMapperRequest.Label = "Put↘";
                    }

                }
                if (quoteMapperSell != null)
                {
                    if (pur.PurchaseType == PurchaseType.Win)
                    {
                        quoteMapperSell.Fill = Brushes.Green;
                        quoteMapperSell.Label = Session.Current.Currency + " " + (pur.SellTransaction.Amount + pur.BuyTransaction.Amount).ToString("#.##");
                    }
                    else if (pur.PurchaseType == PurchaseType.Lose)
                    {
                        quoteMapperSell.Fill = Brushes.Red;
                        quoteMapperSell.Label = Session.Current.Currency + " " + (pur.SellTransaction.Amount + pur.BuyTransaction.Amount).ToString("#.##");
                    }
                }
                Invoke(new MethodInvoker(delegate
                {
                    cartesianChart.Refresh();
                }));
            }
        }

        public void OnTransaction(Session.TradingInstance.PurchaseEventArgs args)
        {
            purchasesToDraw.Add(args.Purchase);
        }

        #region UIHandler

        private void ButtonUp_Click(object sender, EventArgs e)
        {
            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            justBuy = true;
            instance.Buy(ContractType.Call);
        }

        private void ButtonDown_Click(object sender, EventArgs e)
        {
            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            justBuy = true;
            instance.Buy(ContractType.Put);
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

        #endregion
    }
}
