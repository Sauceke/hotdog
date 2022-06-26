using System;

namespace HotdogServer
{
    internal class DepthReadingEventArgs : EventArgs
    {
        public float Depth { get; set; }
    }
}
