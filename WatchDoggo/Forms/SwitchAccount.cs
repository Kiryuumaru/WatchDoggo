using System;
using System.Windows.Forms;
using DoggoWire.Models;
using DoggoWire.Services;

namespace WatchDoggo.Forms
{
    public partial class SwitchAccount : Form
    {
        private Account selectedAccount;

        public SwitchAccount()
        {
            InitializeComponent();
            UpdateUi();
        }

        private void UpdateUi()
        {
            comboBoxAccounts.DataSource = Session.Accounts;
            if (Session.LoggedIn)
            {
                if (Session.Current.Exist)
                {
                    comboBoxAccounts.SelectedIndex = comboBoxAccounts.FindString(Session.Current.LoginId);
                }
                comboBoxAccounts.Enabled = true;
                buttonSelect.Enabled = true;
                buttonLogin.Text = "Logout";
            }
            else
            {
                comboBoxAccounts.Enabled = false;
                buttonSelect.Enabled = false;
                buttonLogin.Text = "Login";
            }
        }

        private void ComboBoxAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAccount = (Account)comboBoxAccounts.SelectedItem;
            if (selectedAccount != null)
            {
                labelAcct.Text = "Name: " + selectedAccount.Name;
                labelCurr.Text = "Currency: " + selectedAccount.Currency;
                labelVrtl.Text = "Virtual: " + (selectedAccount.Virtual ? "Yes" : "No");
            }
            else
            {
                labelAcct.Text = "Name: N/A";
                labelCurr.Text = "Currency: N/A";
                labelVrtl.Text = "Virtual: N/A";
            }
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            Session.Current.Set(selectedAccount);
            Session.Current.Initialize();
            Close();
        }

        private void ButtonLogout_Click(object sender, EventArgs e)
        {
            if (Session.LoggedIn)
            {
                Session.Logout();
                UpdateUi();
            }
            else
            {
                if (Extension.IsAdmin())
                {
                    Session.Login(delegate
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            UpdateUi();
                            this.Restore();
                            Activate();
                        }));
                    });
                }
                else
                {
                    MessageBox.Show("Application needs to open with admin privilege to login", "Need Admin Privilege");
                }
            }
        }
    }
}
