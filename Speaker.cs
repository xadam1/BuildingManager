using System;

namespace BuildingManager
{
    public class Speaker : Device
    {
        private float _volume = 50f;

        public event OnDeviceModifiedEventHandler SpeakerModified; 

        
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
                    Helper.PrintError("Volume of the speaker must be between 0 and 100.");
                    return;
                }
                _volume = value;
                OnSpeakerModified();
            }
        }

        // Methods to change Sound
        public void PlayMusic()
        {
            Sound = SoundOptions.Music;
            OnSpeakerModified();
        }

        public void PlayAlarm()
        {
            Sound = SoundOptions.Alarm;
            OnSpeakerModified();
        }

        public void StopPlaying()
        {
            Sound = SoundOptions.None;
            OnSpeakerModified();
        }

        public override string GetCurrentState()
        {
            return base.GetCurrentState() + $"\nPlaying: {Sound}\nVolume: {Volume}";
        }

        public enum SoundOptions
        {
            None,
            Music,
            Alarm
        }

        protected virtual void OnSpeakerModified()
        {
            SpeakerModified?.Invoke(this, EventArgs.Empty);
        }
    }
}