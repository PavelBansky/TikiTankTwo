using System;
using System.IO;
using TikiTankCommon;

namespace TikiTankHardware
{
    public class LPD8806 : LEDStrip, IDisposable
    {
        public LPD8806(int numberOfLEDs, string fileName)
            : base(numberOfLEDs)
        {
            // Color calculator
            Gama = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                // http://learn.adafruit.com/light-painting-with-raspberry-pi
                base.Gama[i] = (byte)(0x80 | (int)(Math.Pow((float)i / 255.0F, 2.5F) * 127.0F + 0.5F));
            }

            Console.WriteLine("Opening SPI: {0}", fileName);
            _fstream = File.OpenWrite(fileName);

            // Crate latch bytes array
            _latchBytes = new byte[((this.Length + 31) / 32)];
            Array.Clear(_latchBytes, 0, _latchBytes.Length);
            Console.WriteLine("LPD8806: Latch length: {0}", _latchBytes.Length);

            InitSPI();
        }

        private void InitSPI()
        {
            _fstream.WriteByte(0x00);
            _fstream.Write(_latchBytes, 0, _latchBytes.Length);
            _fstream.Flush();
        }

        public override void Show()
        {
            _fstream.Write(base.Channels, 0, base.Channels.Length);
            _fstream.Write(_latchBytes, 0, _latchBytes.Length);
            _fstream.Flush();
        }

        public override void SetPixelRGB(int pixel, byte r, byte g, byte b)
        {
            if (pixel < Length)
            {
                // GRB
                pixel *= 3;
                Channels[pixel] = (byte)(g | 0x80);
                Channels[pixel + 1] = (byte)(r | 0x80);
                Channels[pixel + 2] = (byte)(b | 0x80);
            }
        }

        public override void SetPixelRGB(int pixel, byte r, byte g, byte b, byte brightness)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                Channels[pixel] = Gama[(byte)(g * ((brightness / 100F) * brightness))];
                Channels[pixel + 1] = Gama[(byte)(r * ((brightness / 100F) * brightness))];
                Channels[pixel + 2] = Gama[(byte)(b * ((brightness / 100F) * brightness))];
            }
        }

        public override void SetPixelColor(int pixel, int color)
        {
            if (pixel < Length)
            {
                pixel *= 3;
                Channels[pixel] = (byte)((color >> 8) | 0x80);
                Channels[pixel + 1] = (byte)((color >> 16) | 0x80);
                Channels[pixel + 2] = (byte)(color | 0x80);
            }
        }

        public override void FillRGB(int start, int count, byte r, byte g, byte b)
        {
            for (int i = start; i < start + count; i++)
            {
                SetPixelRGB(i, r, g, b);
            }
        }

        public override int GetPixelColor(int pixel)
        {
            pixel *= 3;
            return ((int)(Channels[pixel+1] & 0x7f) << 16) |
                    ((int)(Channels[pixel] & 0x7f) << 8) |
                    ((int)(Channels[pixel + 2] & 0x7f));
        }
        public void Dispose()
        {
            _fstream.Close();
        }

        private byte[] _latchBytes;
        private FileStream _fstream;
    }
}
