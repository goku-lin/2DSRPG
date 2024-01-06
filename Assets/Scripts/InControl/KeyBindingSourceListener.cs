using System;

namespace InControl
{
    public class KeyBindingSourceListener : BindingSourceListener
    {
        public void Reset()
        {
            this.detectFound.Clear();
            this.detectPhase = 0;
        }

        public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
        {
            if (!listenOptions.IncludeKeys)
            {
                return null;
            }
            if (this.detectFound.IncludeCount > 0 && !this.detectFound.IsPressed && this.detectPhase == 2)
            {
                KeyBindingSource result = new KeyBindingSource(this.detectFound);
                this.Reset();
                return result;
            }
            KeyCombo keyCombo = KeyCombo.Detect(listenOptions.IncludeModifiersAsFirstClassKeys);
            if (keyCombo.IncludeCount > 0)
            {
                if (this.detectPhase == 1)
                {
                    this.detectFound = keyCombo;
                    this.detectPhase = 2;
                }
            }
            else if (this.detectPhase == 0)
            {
                this.detectPhase = 1;
            }
            return null;
        }

        private KeyCombo detectFound;

        private int detectPhase;
    }
}
