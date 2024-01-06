using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace InControl
{
    public abstract class PlayerActionSet
    {
        protected PlayerActionSet()
        {
            this.Enabled = true;
            this.PreventInputWhileListeningForBinding = true;
            this.Device = null;
            this.IncludeDevices = new List<InputDevice>();
            this.ExcludeDevices = new List<InputDevice>();
            this.Actions = new ReadOnlyCollection<PlayerAction>(this.actions);
            InputManager.AttachPlayerActionSet(this);
        }

        public InputDevice Device { get; set; }

        public List<InputDevice> IncludeDevices { get; private set; }

        public List<InputDevice> ExcludeDevices { get; private set; }

        public ReadOnlyCollection<PlayerAction> Actions { get; private set; }

        public ulong UpdateTick { get; protected set; }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action<BindingSourceType> OnLastInputTypeChanged;

        public bool Enabled { get; set; }

        public bool PreventInputWhileListeningForBinding { get; set; }

        public object UserData { get; set; }

        public void Destroy()
        {
            this.OnLastInputTypeChanged = null;
            InputManager.DetachPlayerActionSet(this);
        }

        protected PlayerAction CreatePlayerAction(string name)
        {
            return new PlayerAction(name, this);
        }

        internal void AddPlayerAction(PlayerAction action)
        {
            action.Device = this.FindActiveDevice();
            if (this.actionsByName.ContainsKey(action.Name))
            {
                throw new InControlException("Action '" + action.Name + "' already exists in this set.");
            }
            this.actions.Add(action);
            this.actionsByName.Add(action.Name, action);
        }

        protected PlayerOneAxisAction CreateOneAxisPlayerAction(PlayerAction negativeAction, PlayerAction positiveAction)
        {
            PlayerOneAxisAction playerOneAxisAction = new PlayerOneAxisAction(negativeAction, positiveAction);
            this.oneAxisActions.Add(playerOneAxisAction);
            return playerOneAxisAction;
        }

        protected PlayerTwoAxisAction CreateTwoAxisPlayerAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
        {
            PlayerTwoAxisAction playerTwoAxisAction = new PlayerTwoAxisAction(negativeXAction, positiveXAction, negativeYAction, positiveYAction);
            this.twoAxisActions.Add(playerTwoAxisAction);
            return playerTwoAxisAction;
        }

        public PlayerAction this[string actionName]
        {
            get
            {
                PlayerAction result;
                if (this.actionsByName.TryGetValue(actionName, out result))
                {
                    return result;
                }
                throw new KeyNotFoundException("Action '" + actionName + "' does not exist in this action set.");
            }
        }

        public PlayerAction GetPlayerActionByName(string actionName)
        {
            PlayerAction result;
            if (this.actionsByName.TryGetValue(actionName, out result))
            {
                return result;
            }
            return null;
        }

        internal void Update(ulong updateTick, float deltaTime)
        {
            InputDevice device = this.Device ?? this.FindActiveDevice();
            BindingSourceType lastInputType = this.LastInputType;
            ulong lastInputTypeChangedTick = this.LastInputTypeChangedTick;
            InputDeviceClass lastDeviceClass = this.LastDeviceClass;
            InputDeviceStyle lastDeviceStyle = this.LastDeviceStyle;
            int count = this.actions.Count;
            for (int i = 0; i < count; i++)
            {
                PlayerAction playerAction = this.actions[i];
                playerAction.Update(updateTick, deltaTime, device);
                if (playerAction.UpdateTick > this.UpdateTick)
                {
                    this.UpdateTick = playerAction.UpdateTick;
                    this.activeDevice = playerAction.ActiveDevice;
                }
                if (playerAction.LastInputTypeChangedTick > lastInputTypeChangedTick)
                {
                    lastInputType = playerAction.LastInputType;
                    lastInputTypeChangedTick = playerAction.LastInputTypeChangedTick;
                    lastDeviceClass = playerAction.LastDeviceClass;
                    lastDeviceStyle = playerAction.LastDeviceStyle;
                }
            }
            int count2 = this.oneAxisActions.Count;
            for (int j = 0; j < count2; j++)
            {
                this.oneAxisActions[j].Update(updateTick, deltaTime);
            }
            int count3 = this.twoAxisActions.Count;
            for (int k = 0; k < count3; k++)
            {
                this.twoAxisActions[k].Update(updateTick, deltaTime);
            }
            if (lastInputTypeChangedTick > this.LastInputTypeChangedTick)
            {
                bool flag = lastInputType != this.LastInputType;
                this.LastInputType = lastInputType;
                this.LastInputTypeChangedTick = lastInputTypeChangedTick;
                this.LastDeviceClass = lastDeviceClass;
                this.LastDeviceStyle = lastDeviceStyle;
                if (this.OnLastInputTypeChanged != null && flag)
                {
                    this.OnLastInputTypeChanged(lastInputType);
                }
            }
        }

        public void Reset()
        {
            int count = this.actions.Count;
            for (int i = 0; i < count; i++)
            {
                this.actions[i].ResetBindings();
            }
        }

        private InputDevice FindActiveDevice()
        {
            bool flag = this.IncludeDevices.Count > 0;
            bool flag2 = this.ExcludeDevices.Count > 0;
            if (flag || flag2)
            {
                InputDevice inputDevice = InputDevice.Null;
                int count = InputManager.Devices.Count;
                for (int i = 0; i < count; i++)
                {
                    InputDevice inputDevice2 = InputManager.Devices[i];
                    if (inputDevice2 != inputDevice && inputDevice2.LastChangedAfter(inputDevice))
                    {
                        if (!flag2 || !this.ExcludeDevices.Contains(inputDevice2))
                        {
                            if (!flag || this.IncludeDevices.Contains(inputDevice2))
                            {
                                inputDevice = inputDevice2;
                            }
                        }
                    }
                }
                return inputDevice;
            }
            return InputManager.ActiveDevice;
        }

        public void ClearInputState()
        {
            int count = this.actions.Count;
            for (int i = 0; i < count; i++)
            {
                this.actions[i].ClearInputState();
            }
            int count2 = this.oneAxisActions.Count;
            for (int j = 0; j < count2; j++)
            {
                this.oneAxisActions[j].ClearInputState();
            }
            int count3 = this.twoAxisActions.Count;
            for (int k = 0; k < count3; k++)
            {
                this.twoAxisActions[k].ClearInputState();
            }
        }

        public bool HasBinding(BindingSource binding)
        {
            if (binding == null)
            {
                return false;
            }
            int count = this.actions.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.actions[i].HasBinding(binding))
                {
                    return true;
                }
            }
            return false;
        }

        public void RemoveBinding(BindingSource binding)
        {
            if (binding == null)
            {
                return;
            }
            int count = this.actions.Count;
            for (int i = 0; i < count; i++)
            {
                this.actions[i].RemoveBinding(binding);
            }
        }

        public bool IsListeningForBinding
        {
            get
            {
                return this.listenWithAction != null;
            }
        }

        public BindingListenOptions ListenOptions
        {
            get
            {
                return this.listenOptions;
            }
            set
            {
                this.listenOptions = (value ?? new BindingListenOptions());
            }
        }

        public InputDevice ActiveDevice
        {
            get
            {
                return (this.activeDevice != null) ? this.activeDevice : InputDevice.Null;
            }
        }

        public string Save()
        {
            string result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
                {
                    binaryWriter.Write(66);
                    binaryWriter.Write(73);
                    binaryWriter.Write(78);
                    binaryWriter.Write(68);
                    binaryWriter.Write(2);
                    int count = this.actions.Count;
                    binaryWriter.Write(count);
                    for (int i = 0; i < count; i++)
                    {
                        this.actions[i].Save(binaryWriter);
                    }
                }
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            return result;
        }

        public void Load(string data)
        {
            if (data == null)
            {
                return;
            }
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(data)))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        if (binaryReader.ReadUInt32() != 1145981250U)
                        {
                            throw new Exception("Unknown data format.");
                        }
                        ushort num = binaryReader.ReadUInt16();
                        if (num < 1 || num > 2)
                        {
                            throw new Exception("Unknown data format version: " + num);
                        }
                        int num2 = binaryReader.ReadInt32();
                        for (int i = 0; i < num2; i++)
                        {
                            PlayerAction playerAction;
                            if (this.actionsByName.TryGetValue(binaryReader.ReadString(), out playerAction))
                            {
                                playerAction.Load(binaryReader, num);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Provided state could not be loaded:\n" + ex.Message);
                this.Reset();
            }
        }

        public BindingSourceType LastInputType;

        public ulong LastInputTypeChangedTick;

        public InputDeviceClass LastDeviceClass;

        public InputDeviceStyle LastDeviceStyle;

        private List<PlayerAction> actions = new List<PlayerAction>();

        private List<PlayerOneAxisAction> oneAxisActions = new List<PlayerOneAxisAction>();

        private List<PlayerTwoAxisAction> twoAxisActions = new List<PlayerTwoAxisAction>();

        private Dictionary<string, PlayerAction> actionsByName = new Dictionary<string, PlayerAction>();

        private BindingListenOptions listenOptions = new BindingListenOptions();

        internal PlayerAction listenWithAction;

        private InputDevice activeDevice;

        private const ushort currentDataFormatVersion = 2;
    }
}
