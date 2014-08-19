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

            //Gama = new byte[256] 
            //{	
            //    // from math found at 
            //    // http://learn.adafruit.com/light-painting-with-raspberry-pi
            //    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
            //    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
            //    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
            //    128, 128, 128, 128, 128, 128, 129, 129, 129, 129,
            //    129, 129, 129, 129, 129, 129, 129, 129, 130, 130,
            //    130, 130, 130, 130, 130, 130, 131, 131, 131, 131,
            //    131, 131, 131, 132, 132, 132, 132, 132, 132, 133,
            //    133, 133, 133, 133, 133, 134, 134, 134, 134, 135,
            //    135, 135, 135, 135, 136, 136, 136, 136, 137, 137,
            //    137, 137, 138, 138, 138, 139, 139, 139, 139, 140,
            //    140, 140, 141, 141, 141, 142, 142, 142, 143, 143,
            //    143, 144, 144, 144, 145, 145, 146, 146, 146, 147,
            //    147, 148, 148, 148, 149, 149, 150, 150, 151, 151,
            //    152, 152, 152, 153, 153, 154, 154, 155, 155, 156,
            //    156, 157, 157, 158, 158, 159, 160, 160, 161, 161,
            //    162, 162, 163, 163, 164, 165, 165, 166, 166, 167,
            //    168, 168, 169, 170, 170, 171, 172, 172, 173, 174,
            //    174, 175, 176, 176, 177, 178, 178, 179, 180, 181,
            //    181, 182, 183, 184, 184, 185, 186, 187, 188, 188,
            //    189, 190, 191, 192, 192, 193, 194, 195, 196, 197,
            //    198, 198, 199, 200, 201, 202, 203, 204, 205, 206,
            //    207, 208, 208, 209, 210, 211, 212, 213, 214, 215,
            //    216, 217, 218, 219, 220, 221, 222, 224, 225, 226,
            //    227, 228, 229, 230, 231, 232, 233, 234, 236, 237,
            //    238, 239, 240, 241, 242, 244, 245, 246, 247, 248,
            //    250, 251, 252, 253, 254, 255
            //};

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
