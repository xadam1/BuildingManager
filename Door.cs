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


        public PossibleState State { get; set; } = PossibleState.Locked;

        public bool Open
        {
            get => State == PossibleState.Open;
            set
            {
                if (value)
                {
                    State |= PossibleState.Open;
                }
            }
        }

        public bool OpenForTooLong
        {
            get => State == PossibleState.OpenForTooLong;
            set
            {
                if (value)
                {
                    State |= PossibleState.OpenForTooLong;
                }
            }
        }

        public bool OpenedForcibly
        {
            get => State == PossibleState.OpenedForcibly;
            set
            {
                if (value)
                {
                    State |= PossibleState.OpenedForcibly;
                }
            }
        }

        public bool Locked
        {
            get => State == PossibleState.Locked;
            set
            {
                if (value)
                {
                    State |= PossibleState.Locked;
                }
            }
        }
        

        [Flags]
        public enum PossibleState
        {
            Locked = 1,
            Open = 2,
            OpenForTooLong = 4,
            OpenedForcibly = 8
        }
    }
}