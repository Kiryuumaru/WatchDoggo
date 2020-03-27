using DoggoWire.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WatchDoggo.Forms
{
    public partial class Cutoff : Form
    {
        private readonly Session.TradingInstance instance;
        private readonly Action followups;
        public Cutoff(Session.TradingInstance tradingInstance, Action resetFollowups)
        {
            InitializeComponent();
            instance = tradingInstance;
            followups = resetFollowups;
            if (instance.TotalSessionAmount < 0)
            {
                labelResult.Text = "lose";
                labelResult.ForeColor = Color.Red;
                labelTotalSession.ForeColor = Color.Red;
            }
            else
            {
                labelResult.Text = "win";
                labelResult.ForeColor = Color.Green;
                labelTotalSession.ForeColor = Color.Green;
            }
            labelTotalSession.Text = Session.Current.Currency + " " + instance.TotalSessionAmount.ToString();
            labelWin.Text = instance.WinCount.ToString();
            labelLose.Text = instance.LoseCount.ToString();
            labelAccuracy.Text = (instance.Accuracy * 100).ToString("0.##") + "%";
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            followups?.Invoke();
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
