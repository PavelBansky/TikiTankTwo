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
            _fstream.Write(base._leds, 0, base._leds.Length);
            _fstream.Write(_latchBytes, 0, _latchBytes.Length);
            _fstream.Flush();
        }

        public void Dispose()
        {
            _fstream.Close();
        }

        private byte[] _latchBytes;
        private FileStream _fstream;
    }
}
