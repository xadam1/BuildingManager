using System;

namespace BuildingManager
{
    public class Door : Device
    {
        public Door(string name) : base(DeviceType.Door, name ?? "Door")
        {
        }
        
        public override string GetCurrentState()
        {
            return base.GetCurrentState() + $"\nState: {State}";
        }

        public PossibleState State { get; private set; } = PossibleState.Locked;

        public bool Locked => State == PossibleState.Locked;

        public bool Open => State == PossibleState.Open;

        public bool OpenForTooLong => State == PossibleState.OpenForTooLong;

        public bool OpenedForcibly => State == PossibleState.OpenedForcibly;


        // Methods to change Door's state
        public void OpenDoor()
        {
            State = PossibleState.Open;
            OnDeviceModified();
        }

        public void LockDoor()
        {
            State = PossibleState.Locked;
            OnDeviceModified();
        }

        public void SetOpenForTooLong()
        {
            State = PossibleState.OpenForTooLong;
            OnDeviceModified();
        }

        public void SetOpenedForcibly()
        {
            State = PossibleState.OpenedForcibly;
            OnDeviceModified();
        }


        public enum PossibleState
        {
            Locked,
            Open,
            OpenForTooLong,
            OpenedForcibly
        }
    }
}