using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace InControl
{
    public class XInputDeviceManager : InputDeviceManager
    {
        public XInputDeviceManager()
        {
            if (InputManager.XInputUpdateRate == 0U)
            {
                this.timeStep = Mathf.FloorToInt(Time.fixedDeltaTime * 1000f);
            }
            else
            {
                this.timeStep = Mathf.FloorToInt(1f / InputManager.XInputUpdateRate * 1000f);
            }
            this.bufferSize = (int)Math.Max(InputManager.XInputBufferSize, 1U);
            for (int i = 0; i < 4; i++)
            {
                //this.gamePadState[i] = new RingBuffer<GamePadState>(this.bufferSize);
            }
            this.StartWorker();
            for (int j = 0; j < 4; j++)
            {
                this.devices.Add(new XInputDevice(j, this));
            }
            this.Update(0UL, 0f);
        }

        private void StartWorker()
        {
            if (this.thread == null)
            {
                this.thread = new Thread(new ThreadStart(this.Worker));
                this.thread.IsBackground = true;
                this.thread.Start();
            }
        }

        private void StopWorker()
        {
            if (this.thread != null)
            {
                this.thread.Abort();
                this.thread.Join();
                this.thread = null;
            }
        }

        private void Worker()
        {
            for (; ; )
            {
                for (int i = 0; i < 4; i++)
                {
                    //this.gamePadState[i].Enqueue(GamePad.GetState((PlayerIndex)i));
                }
                Thread.Sleep(this.timeStep);
            }
        }

        //internal GamePadState GetState(int deviceIndex)
        //{
        //    return this.gamePadState[deviceIndex].Dequeue();
        //}

        public override void Update(ulong updateTick, float deltaTime)
        {
            for (int i = 0; i < 4; i++)
            {
                XInputDevice xinputDevice = this.devices[i] as XInputDevice;
                //if (!xinputDevice.IsConnected)
                //{
                //    xinputDevice.GetState();
                //}
                //if (xinputDevice.IsConnected != this.deviceConnected[i])
                //{
                //    if (xinputDevice.IsConnected)
                //    {
                //        InputManager.AttachDevice(xinputDevice);
                //    }
                //    else
                //    {
                //        InputManager.DetachDevice(xinputDevice);
                //    }
                //    this.deviceConnected[i] = xinputDevice.IsConnected;
                //}
            }
        }

        public override void Destroy()
        {
            this.StopWorker();
        }

        public static bool CheckPlatformSupport(ICollection<string> errors)
        {
            if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
            {
                return false;
            }
            try
            {
                //GamePad.GetState(PlayerIndex.One);
            }
            catch (DllNotFoundException ex)
            {
                if (errors != null)
                {
                    errors.Add(ex.Message + ".dll could not be found or is missing a dependency.");
                }
                return false;
            }
            return true;
        }

        internal static void Enable()
        {
            List<string> list = new List<string>();
            if (XInputDeviceManager.CheckPlatformSupport(list))
            {
                //InputManager.HideDevicesWithProfile(typeof(Xbox360WinProfile));
                //InputManager.HideDevicesWithProfile(typeof(XboxOneWinProfile));
                //InputManager.HideDevicesWithProfile(typeof(XboxOneWin10Profile));
                //InputManager.HideDevicesWithProfile(typeof(XboxOneWin10AEProfile));
                //InputManager.HideDevicesWithProfile(typeof(LogitechF310ModeXWinProfile));
                //InputManager.HideDevicesWithProfile(typeof(LogitechF510ModeXWinProfile));
                //InputManager.HideDevicesWithProfile(typeof(LogitechF710ModeXWinProfile));
                InputManager.AddDeviceManager<XInputDeviceManager>();
            }
            else
            {
                foreach (string text in list)
                {
                    //Logger.LogError(text);
                }
            }
        }

        private bool[] deviceConnected = new bool[4];

        private const int maxDevices = 4;

        //private RingBuffer<GamePadState>[] gamePadState = new RingBuffer<GamePadState>[4];

        private Thread thread;

        private int timeStep;

        private int bufferSize;
    }
}
