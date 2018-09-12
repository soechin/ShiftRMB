using System;
using System.Runtime.InteropServices;

namespace ShiftRMB
{
    public class Winmm
    {
        public delegate void TIMECALLBACK(int uTimerID, int uMsg, IntPtr dwUser, IntPtr dw1, IntPtr dw2);

        [DllImport("winmm.dll")]
        public static extern int timeSetEvent(int uDelay, int uResolution, TIMECALLBACK lpTimeProc, IntPtr dwUser, int fuEvent);

        [DllImport("winmm.dll")]
        public static extern int timeKillEvent(int uTimerID);
    }
}
