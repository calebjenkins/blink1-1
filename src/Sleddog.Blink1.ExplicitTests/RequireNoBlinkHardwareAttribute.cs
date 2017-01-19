﻿using System.Linq;

namespace Sleddog.Blink1.ExplicitTests
{
    public sealed class RequireNoBlinkHardwareAttribute : BlinkHardwareScannerAttribute
    {
        public RequireNoBlinkHardwareAttribute()
        {
            if (Devices.Any())
            {
                Skip = "Blink1 devices connected";
            }
        }
    }
}