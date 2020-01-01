using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoggoWire.Models
{
    public enum PurchaseType
    {
        Win,
        Lose,
        Ongoing
    }

    public class Purchase
    {
        public ActiveSymbol ActiveSymbol { get; private set; }
        public Transaction BuyTransaction { get; private set; }
        public Transaction SellTransaction { get; private set; }
        public PurchaseType PurchaseType
        {
            get
            {
                if (SellTransaction == null) return PurchaseType.Ongoing;
                return Amount < 0 ? PurchaseType.Lose : PurchaseType.Win;
            }
        }
        public decimal Amount
        {
            get
            {
                if (SellTransaction == null) return BuyTransaction.Amount;
                return BuyTransaction.Amount + SellTransaction.Amount;
            }
        }

        public Purchase(ActiveSymbol activeSymbol, Transaction buyTransaction, Transaction sellTransaction = null)
        {
            ActiveSymbol = activeSymbol;
            BuyTransaction = buyTransaction;
            SellTransaction = sellTransaction;
        }
    }
}
