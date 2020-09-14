using System.Text;
using System.Text.RegularExpressions;

namespace BuildingManager.Devices
{
    public class CardReader : Device
    {
        private string _accessCardNumber;

        public CardReader(string name) : base(DeviceTypes.CardReader, name ?? "CardReader")
        {
        }

        public string AccessCardNumber
        {
            get => _accessCardNumber;
            set
            {
                if (!ValidateCardNumber(value))
                {
                    OnDeviceError("Card Number is not valid!");
                    return;
                }

                _accessCardNumber = ReverseBytesAndPad(value);
                OnDeviceModified();
            }
        }

        // Checks if condition for CardNumber are met
        private static bool ValidateCardNumber(string cardNumber)
        {
            return (cardNumber.Length % 2 == 0 && cardNumber.Length < 16)
                   && Regex.IsMatch(cardNumber.ToUpper(), @"^[0-9A-F]+$");
        }

        private static string ReverseBytesAndPad(string input)
        {
            var sb = new StringBuilder();
            for (var i = input.Length - 1; i > 0; i -= 2)
            {
                sb.Append(input[i - 1]);
                sb.Append(input[i]);
            }

            return sb.ToString().PadLeft(16, '0');
        }


        public override string GetCurrentState() =>
            base.GetCurrentState() + $"\nAccess Number: {AccessCardNumber}";
    }
}