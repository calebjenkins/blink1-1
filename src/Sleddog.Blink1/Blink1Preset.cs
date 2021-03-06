﻿using System;
using System.Drawing;
using Sleddog.Blink1.Internal;

namespace Sleddog.Blink1
{
    public class Blink1Preset
    {
        public Blink1Preset(Color color, TimeSpan duration)
        {
            Color = color;
            PresetDuration = duration;
        }

        public Color Color { get; }

        public TimeSpan Duration => PresetDuration;

        internal Blink1Duration PresetDuration { get; }

        protected bool Equals(Blink1Preset other)
        {
            var equal = Color.R.Equals(other.Color.R) &&
                        Color.G.Equals(other.Color.G) &&
                        Color.B.Equals(other.Color.B) &&
                        PresetDuration.Equals(other.PresetDuration);

            return equal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((Blink1Preset) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode()*397) ^ (PresetDuration != null ? PresetDuration.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Blink1Preset left, Blink1Preset right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Blink1Preset left, Blink1Preset right)
        {
            return !Equals(left, right);
        }
    }
}