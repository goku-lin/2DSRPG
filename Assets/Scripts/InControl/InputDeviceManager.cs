using System;
using System.Collections.Generic;
using UnityEngine.XR;

namespace InControl
{
    public abstract class InputDeviceManager
    {
        public abstract void Update(ulong updateTick, float deltaTime);

        public virtual void Destroy()
        {
        }

        protected List<InputDevice> devices = new List<InputDevice>();
    }
}
