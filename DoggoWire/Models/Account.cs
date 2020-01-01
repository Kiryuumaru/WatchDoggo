namespace DoggoWire.Models
{
    public class Account
    {
        #region Properties

        public string Name { get; private set; }
        public string Token { get; private set; }
        public string Currency { get; private set; }
        public bool Virtual { get; private set; }

        #endregion

        #region Initializers

        public Account(string name, string token, string currency, bool isVirtual)
        {
            Name = name;
            Token = token;
            Currency = currency;
            Virtual = isVirtual;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
