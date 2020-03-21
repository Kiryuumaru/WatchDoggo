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
        Request,
        Ongoing
    }

    public class Purchase
    {
        public ActiveSymbol ActiveSymbol { get; private set; }
        public BuyResponse BuyResponse { get; set; }
        public Transaction BuyTransaction { get; set; }
        public Transaction SellTransaction { get; set; }
        public PurchaseType PurchaseType
        {
            get
            {
                if (BuyTransaction == null) return PurchaseType.Request;
                if (SellTransaction == null) return PurchaseType.Ongoing;
                return Amount < 0 ? PurchaseType.Lose : PurchaseType.Win;
            }
        }
        public decimal Amount
        {
            get
            {
                if (BuyTransaction == null) return BuyResponse.Buy.BuyPrice;
                if (SellTransaction == null) return BuyTransaction.Amount;
                return BuyTransaction.Amount + SellTransaction.Amount;
            }
        }

        public Purchase(ActiveSymbol activeSymbol, BuyResponse buyResponse = null,  Transaction buyTransaction = null, Transaction sellTransaction = null)
        {
            ActiveSymbol = activeSymbol;
            BuyResponse = buyResponse;
            BuyTransaction = buyTransaction;
            SellTransaction = sellTransaction;
        }
    }
}
