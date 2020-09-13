using System;
using System.Text.RegularExpressions;

namespace BuildingManager
{
    public class CardReader : Default
    {
        private string _accessCardNumber;

        public CardReader(string name) : base(Device.CardReader, name ?? "CardReader")
        {
        }

        public event OnDeviceModifiedEventHandler CardReaderModified;

        public string AccessCardNumber
        {
            get => _accessCardNumber;
            set
            {
                if (!ValidateCardNumber(value))
                {
                    Helper.PrintError("Card Number is not valid!");
                    return;
                }

                _accessCardNumber = ReverseBytesAndPad(value);
                OnCardReaderModified();
            }
        }

        // Checks if condition for CardNumber are met
        private bool ValidateCardNumber(string cardNumber)
        {
            return (cardNumber.Length % 2 == 0 && cardNumber.Length < 16)
                   && Regex.IsMatch(cardNumber.ToUpper(), @"^[0-9A-F]+$");
        }


        private static string ReverseBytesAndPad(string input)
        {
            var reversed = "";

            for (var i = input.Length - 1; i > 0; i -= 2)
            {
                reversed += input[i - 1];
                reversed += input[i];
            }

            return reversed.PadLeft(16, '0');
        }

        public override string GetCurrentState() => base.GetCurrentState() + $"\nAccess Number: {AccessCardNumber}";

        protected virtual void OnCardReaderModified()
        {
            CardReaderModified?.Invoke(this, EventArgs.Empty);
        }
    }
}