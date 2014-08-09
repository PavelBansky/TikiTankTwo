using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TikiTankHardware
{
    public class HardwareHelper
    {
        public const string SPI0_FIRMWARE = "BBB-SPI0";
        public const string SPI1_FIRMWARE = "BBB-SPI1";
        public const string UART5_FIRMWARE = "BBB-UART1";
        public const string OVERLAY_FILE = "/sys/devices/bone_capemgr.9/slots";

        public static void SetupOverlays()
        {
            HardwareHelper hw = new HardwareHelper();            
            hw.EnableOverlays();                       
        }

        private void EnableOverlays()
        {
            using (FileStream fileStream = File.Open(OVERLAY_FILE, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                StreamReader sr = new StreamReader(fileStream);
                _overlays = sr.ReadToEnd();
                sr.Close();
                fileStream.Close();
            }

            DisableHDMI();
            EnableSerial();
        }

        private void DisableHDMI()
        {
            string[] lines = _overlays.Split('\n');
            foreach(string line in lines)
            {
                if (line.Contains("HDMI"))
                {
                    string lineNumber = GetLineNumber(line);
                    Console.WriteLine("Overlays: HDMI is enabled");
                    Console.WriteLine("Overlays: Disabling HDMI (line {0})", lineNumber);
                    EnableOverlay(string.Format("-{0}", lineNumber));
                }
            }
        }

        private void EnableSerial()
        {
            if (_overlays.Contains(SPI0_FIRMWARE))
            {
                Console.WriteLine("Overlays: SPI0 is alrady enabled");
            }
            else
            {
                Console.WriteLine("Overlays: Enabling SPI0");
                EnableOverlay(SPI0_FIRMWARE);
            }

            if (_overlays.Contains(SPI1_FIRMWARE))
            {
                Console.WriteLine("Overlays: SPI1 is alrady enabled");
            }
            else
            {
                Console.WriteLine("Overlays: Enabling SPI1");
                EnableOverlay(SPI1_FIRMWARE);
            }

            if (_overlays.Contains(UART5_FIRMWARE))
            {
                Console.WriteLine("Overlays: UART is alrady enabled");
            }
            else
            {
                Console.WriteLine("Overlays: Enabling UART");
                EnableOverlay(UART5_FIRMWARE);
            }
        }

        private void EnableOverlay(string overlayInput)
        {
           /* string command = string.Format("-c \"echo {0} > {1}\"", overlayInput, OVERLAY_FILE);
            Console.WriteLine("Overlays: Executing {0} ", command);
            Process.Start("/bin/bash", command);      */ 
            
            using (FileStream fileStream = File.Open(OVERLAY_FILE, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(overlayInput);                
                streamWriter.Close();                
                fileStream.Close();
                Thread.Sleep(200);
            }          
        }

        private string GetLineNumber(string line)
        {
            return line.Substring(0, line.IndexOf(':')).Trim();
        }

        private string _overlays;

    }
}
