namespace BuildingManager.Devices
{
    public class LedPanel : Device
    {
        private string _message;

        public LedPanel(string name) : base(DeviceTypes.LedPanel, name ?? "LedPanel")
        {
        }
        
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnDeviceModified();
            }
        }

        public override string GetCurrentState() => 
            base.GetCurrentState() + $"\nMessage: {Message}";
    }
}