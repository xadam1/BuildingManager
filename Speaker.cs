using System;

namespace BuildingManager
{
    public class Speaker : Device
    {
        private float _volume = 50f;

        public Speaker(string name) : base(DeviceType.Speaker, name ?? "Speaker")
        {
        }

        public SoundOptions Sound { get; private set; } = SoundOptions.None;

        public float Volume
        {
            get => _volume;
            set
            {
                if (value < 0 || value > 100)
                {
                    // TODO event based
                    Helper.PrintError("Volume of the speaker must be between 0 and 100.");
                    return;
                }
                _volume = value;
                OnDeviceModified();
            }
        }

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