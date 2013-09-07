﻿using System;
using System.Linq;
using Sleddog.Blink1.Interfaces;

namespace Sleddog.Blink1.Commands
{
	public class SetPresetCommand : IBlink1Command
	{
		private readonly Blink1Preset preset;
		private readonly byte position;

		public SetPresetCommand(Blink1Preset preset, ushort position)
		{
			if (!Enumerable.Range(0, Blink1.NumberOfPresets).Contains(position))
				throw new ArgumentOutOfRangeException("position");

			this.preset = preset;
			this.position = Convert.ToByte(position);
		}

		public byte[] ToHidCommand()
		{
			var presetDuration = preset.Duration;
			var presetColor = preset.Color;

			return new[]
			       {
				       Convert.ToByte(1),
				       (byte) Blink1Commands.SavePreset,
				       presetColor.R,
				       presetColor.G,
				       presetColor.B,
				       presetDuration.High,
				       presetDuration.Low,
				       position
			       };
		}
	}
}