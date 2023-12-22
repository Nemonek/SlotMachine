using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlotMachineLibrary
{
    internal class Slot
    {
        private bool _isLocked;
        public bool IsLocked { get => _isLocked; internal set => _isLocked = value; }

        public Slot()
        {
            this._isLocked = false;
        }
    }
}
