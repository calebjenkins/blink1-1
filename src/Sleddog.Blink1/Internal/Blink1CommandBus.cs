﻿using System;
using System.Collections.Generic;
using System.Linq;
using HidLibrary;
using Sleddog.Blink1.Internal.Interfaces;

namespace Sleddog.Blink1.Internal
{
    internal class Blink1CommandBus : IDisposable
    {
        private readonly HidDevice hidDevice;

        public bool IsConnected => hidDevice.IsOpen;

        public Blink1CommandBus(HidDevice hidDevice)
        {
            this.hidDevice = hidDevice;
        }

        internal string ReadSerial()
        {
            byte[] output;
         
            hidDevice.ReadSerialNumber(out output);

            var chars = (from o in output where o != 0 select (char) o).ToArray();

            return $"0x{string.Join(string.Empty, chars)}";
        }

        internal bool SendCommand(IBlink1MultiCommand multiCommand)
        {
            if (!IsConnected)
            {
                Connect();
            }

            var commandResults = (from hc in multiCommand.ToHidCommands()
                                  select WriteData(hc))
                .ToList();

            return commandResults.Any(cr => cr == false);
        }

        internal T SendQuery<T>(IBlink1MultiQuery<T> query) where T : class
        {
            if (!IsConnected)
            {
                Connect();
            }

            var responseSegments = new List<byte[]>();

            var hidCommands = query.ToHidCommands().ToList();

            foreach (var hidCommand in hidCommands)
            {
                var commandSend = WriteData(hidCommand);

                if (commandSend)
                {
                    byte[] responseData;

                    var readData = hidDevice.ReadFeatureData(out responseData, Convert.ToByte(1));

                    if (readData)
                    {
                        responseSegments.Add(responseData);
                    }
                }
            }

            if (responseSegments.Count == hidCommands.Count)
            {
                return query.ToResponseType(responseSegments);
            }

            return default;
        }

        internal bool SendCommand(IBlink1Command command)
        {
            if (!IsConnected)
            {
                Connect();
            }

            var commandSend = WriteData(command.ToHidCommand());

            return commandSend;
        }

        internal T SendQuery<T>(IBlink1Query<T> query) where T : class
        {
            if (!IsConnected)
            {
                Connect();
            }

            var commandSend = WriteData(query.ToHidCommand());

            if (commandSend)
            {
                byte[] responseData;

                var readData = hidDevice.ReadFeatureData(out responseData, Convert.ToByte(1));

                if (readData)
                {
                    return query.ToResponseType(responseData);
                }
            }

            return default;
        }

        private bool WriteData(byte[] data)
        {
            var writeData = new byte[8];

            writeData[0] = Convert.ToByte(1);

            var length = Math.Min(data.Length, writeData.Length - 1);

            Array.Copy(data, 0, writeData, 1, length);

            return hidDevice.WriteFeatureData(writeData);
        }

        public void Connect()
        {
            hidDevice.OpenDevice();
        }

        public void Dispose()
        {
            if (hidDevice != null && hidDevice.IsOpen)
            {
                hidDevice.CloseDevice();
            }
        }
    }
}