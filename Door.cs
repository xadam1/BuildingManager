using System;

namespace BuildingManager
{
    public class Door : Device
    {
        public Door(string name) : base(DeviceType.Door, name ?? "Door")
        {
        }

        public event OnDeviceModifiedEventHandler DoorModified;


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
            OnDoorModified();
        }

        public void LockDoor()
        {
            State = PossibleState.Locked;
            OnDoorModified();
        }

        public void SetOpenForTooLong()
        {
            State = PossibleState.OpenForTooLong;
            OnDoorModified();
        }

        public void SetOpenedForcibly()
        {
            State = PossibleState.OpenedForcibly;
            OnDoorModified();
        }


        public enum PossibleState
        {
            Locked,
            Open,
            OpenForTooLong,
            OpenedForcibly
        }

        protected virtual void OnDoorModified()
        {
            DoorModified?.Invoke(this, EventArgs.Empty);
        }
    }
}