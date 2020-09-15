﻿namespace BuildingManager.Devices
{
    public class Speaker : Device
    {
        private float _volume = 50f;
        private SoundOptions _sound;

        public Speaker(string name) : base(DeviceTypes.Speaker, name ?? "Speaker")
        {
            Sound = SoundOptions.None;
        }

        public SoundOptions Sound
        {
            get => _sound;
            set
            {
                _sound = value;
                OnDeviceModified();
            }
        }

        public float Volume
        {
            get => _volume;
            set
            {
                if (value < 0 || value > 100)
                {
                    OnDeviceError("Volume of the speaker must be between 0 and 100.");
                    return;
                }
                _volume = value;
                OnDeviceModified();
            }
        }

        /*
        // Methods to change Sound
        public void PlayMusic()
        {
            Sound = SoundOptions.Music;
            OnDeviceModified();
        }

        public void PlayAlarm()
        {
            Sound = SoundOptions.Alarm;
            OnDeviceModified();
        }

        public void StopPlaying()
        {
            Sound = SoundOptions.None;
            OnDeviceModified();
        }
        */

        public override string GetCurrentState()
            => base.GetCurrentState() + $"\nPlaying: {Sound}\nVolume: {Volume}";
    
        public enum SoundOptions
        {
            None,
            Music,
            Alarm
        }
    }
}