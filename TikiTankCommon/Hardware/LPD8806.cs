using System;
using System.Drawing;
using System.IO;
using TikiTankCommon;

namespace TikiTankHardware
{
    public class LPD8806 : IDisplayDevice
    {
        public LPD8806(int numberOfLEDs, string fileName)
        {
            // Color calculator
            Gama = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                // http://learn.adafruit.com/light-painting-with-raspberry-pi
                Gama[i] = (byte)(0x80 | (int)(Math.Pow((float)i / 255.0F, 2.5F) * 127.0F + 0.5F));
            }

            Length = numberOfLEDs;

            Console.WriteLine("Opening SPI: {0}", fileName);
            _fstream = File.OpenWrite(fileName);

            // Crate latch bytes array
            _latchBytes = new byte[((Length + 31) / 32)];
            Array.Clear(_latchBytes, 0, _latchBytes.Length);
            Console.WriteLine("LPD8806: Latch length: {0}", _latchBytes.Length);

            // Create buffer used to show all pixels
            _showBytes = new byte[(3 * Length) + _latchBytes.Length];

            // Put the latch at the end of the show buffer
            Buffer.BlockCopy(_latchBytes, 0, _showBytes, 3 * Length, _latchBytes.Length);

            InitSPI();
        }

        private void InitSPI()
        {
            _fstream.WriteByte(0x00);
            _fstream.Write(_latchBytes, 0, _latchBytes.Length);
            _fstream.Flush();
        }

        public void Show(Color[] pixels)
        {
            if (pixels.Length < Length)
                return;

            for(int i=0; i<pixels.Length; i++)
            {
                // Move to GRB with gamma taken into account
                _showBytes[3 * i + 0] = Gama[pixels[i].G];
                _showBytes[3 * i + 1] = Gama[pixels[i].R];
                _showBytes[3 * i + 2] = Gama[pixels[i].B];
            }

            _fstream.Write(_showBytes, 0, _showBytes.Length);
            _fstream.Flush();
        }

        public void Dispose()
        {
            _fstream.Close();
        }

        public int Length
        {
            get;
            private set;
        }

        byte[] Gama;
        private byte[] _latchBytes;
        private byte[] _showBytes;
        private FileStream _fstream;
    }
}
