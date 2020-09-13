﻿using System;
using System.Runtime.CompilerServices;

namespace BuildingManager
{
    public class LedPanel : Device
    {
        private string _message;

        public LedPanel(string name) : base(DeviceType.LedPanel, name ?? "LedPanel")
        {
        }

        public event OnDeviceModifiedEventHandler LedPanelModified;


        public override string GetCurrentState()
        {
            return base.GetCurrentState() + $"\nMessage: {Message}";
        }


        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnLedPanelModified();
            }
        }

        protected virtual void OnLedPanelModified()
        {
            LedPanelModified?.Invoke(this, EventArgs.Empty);
        }
    }
}