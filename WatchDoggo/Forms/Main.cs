﻿using System;
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
        #region HelperClass

        public class TradingInstanceWire
        {
            public Action OnClose;
            public void Close()
            {
                OnClose?.Invoke();
            }
        }

        #endregion

        #region Properties

        private readonly TradingInstanceWire tradingInstanceWire = new TradingInstanceWire();

        #endregion

        #region Initializers

        public Main()
        {
            InitializeComponent();
            Session.OnConnectionChanges += OnConnectionChanges;
            Session.Current.OnStateChanges += OnStateChanges;
            Session.Current.OnActiveSymbolAvailable += OnActiveSymbolAvailable;
            Session.Current.OnBalanceChanges += OnBalanceChanges;
            Session.Current.OnProfitTableResponse += OnProfitTableResponse;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Session.Init(new Storage());
            if (!Session.Current.Exist)
            {
                Hide();
                using SwitchAccount switchAccount = new SwitchAccount();
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

        protected override void OnClosing(CancelEventArgs e)
        {
            Session.Current.Stop();
            base.OnClosing(e);
        }

        #endregion

        #region EventHandlers

        private void OnConnectionChanges(Session.ConnectionChangesEventArgs args)
        {
            if (args.Connected)
            {
                Session.Current.AuthorizeRequest();
            }
        }

        private void OnStateChanges(Session.Current.CurrentStateChangesEventArgs args)
        {
            if (args.LoggedIn)
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    Text = Session.Current.Email;
                    labelAccount.Text = Session.Current.LoginId + (Session.Current.Virtual ? " (virtual)" : "");
                    labelBalance.Text = Session.Current.Balance.ToString("0.################") + " " + Session.Current.Currency;
                }));
                Session.Current.BalanceRequest();
                Session.Current.ActiveSymbolsRequest();
                Session.Current.ProfitTableRequest();
                Session.Current.TransactionStreamRequest();
            }
            else
            {
                Invoke(new MethodInvoker(delegate
                {
                    if (Disposing) return;
                    labelAccount.Text = "N/A";
                    labelBalance.Text = "N/A";
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
        }

        private void OnActiveSymbolAvailable(Session.Current.ActiveSymbolAvaliableEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                dataGridViewMarket.Rows.Clear();
                foreach (ActiveSymbol symbol in args.ActiveSymbols)
                {
                    DataGridViewRow row = new DataGridViewRow { Tag = symbol.Symbol };
                    row.CreateCells(
                        dataGridViewMarket,
                        symbol.MarketDisplayName + " - " + symbol.DisplayName,
                        "Open");
                    dataGridViewMarket.Rows.Add(row);
                }
            }));
        }

        private void OnProfitTableResponse(Session.Current.ProfitTableResponseEventArgs args)
        {
            Invoke(new MethodInvoker(delegate
            {
                if (Disposing) return;
                dataGridViewProfitTable.Rows.Clear();
                foreach (ProfitTableTransaction transaction in args.ProfitTable.Transactions)
                {
                    DataGridViewRow row = new DataGridViewRow { Tag = transaction.ContractId };
                    row.CreateCells(
                        dataGridViewProfitTable,
                        transaction.PurchaseTime,
                        transaction.Longcode,
                        (transaction.SellPrice - transaction.BuyPrice).ToString("0.################") + " " + Session.Current.Currency);
                    if (transaction.SellPrice - transaction.BuyPrice < 0)
                    {
                        row.Cells[2].Style.ForeColor = Color.Red;
                    }
                    else if (transaction.SellPrice - transaction.BuyPrice > 0)
                    {
                        row.Cells[2].Style.ForeColor = Color.Green;
                    }
                    row.MinimumHeight = 50;
                    dataGridViewProfitTable.Rows.Add(row);
                }
            }));
        }

        private void ButtonSwitch_Click(object sender, EventArgs e)
        {
            using SwitchAccount switchAccount = new SwitchAccount();
            switchAccount.ShowDialog();
            if (!Session.Current.Exist)
            {
                Close();
            }
            Session.Current.AuthorizeRequest();
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to reset current session?", "Reset Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                dataGridViewMarket.Rows.Clear();
                dataGridViewProfitTable.Rows.Clear();
                tradingInstanceWire.Close();
                Session.Current.AuthorizeRequest();
            }
        }
 
        private void DataGridViewMarket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Session.TradingInstance tradingInstance = Session.Current.StartTrading((string)dataGridViewMarket.Rows[e.RowIndex].Tag);
                if (tradingInstance == null) return;
                Trading trading = new Trading(tradingInstance);
                tradingInstanceWire.OnClose += delegate { trading.Close(); };
                trading.Show();
                WindowState = FormWindowState.Minimized;
            }
        }

        #endregion
    }
}
