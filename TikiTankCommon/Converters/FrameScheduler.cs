using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace TikiTankCommon.Converters
{
    public class FrameScheduler
    {
        public FrameScheduler(TimeSpan interframe)
        {
            frame = 0;
            index = 0;
            Delay = interframe;
            devices = new Dictionary<string, DoubleBufferShowable>();
            indices = new Dictionary<int, DoubleBufferShowable>();
        }

        public TimeSpan Delay { get; private set; }

        public void AddDevice(IShowable device)
        {
            DoubleBufferShowable show = new DoubleBufferShowable(
                    device.Name, device, device.Pixels);
            devices.Add(device.Name, show);
            indices.Add(index, show);
            index++;
        }

        public Color[] GetDrawFrame(string device)
        {
            return devices[device].DrawFrame;
        }

        public void Show(List<IPattern> patterns)
        {
            DateTime end = DateTime.Now + Delay;
            frame++;
            foreach (IPattern pat in patterns)
            {
                //Console.WriteLine(pat.OutputDevice);
                if (devices.ContainsKey(pat.OutputDevice))
                {
                    DoubleBufferShowable output = devices[pat.OutputDevice];
                    if (pat.WouldUpdate(frame))
                    {
                        pat.Update(frame, output.DrawFrame);
                    }
                    output.Show();
                }
            }

            if (end > DateTime.Now)
                Thread.Sleep(end - DateTime.Now);
        }

        private int index;
        private int frame;
        private Dictionary<string, DoubleBufferShowable> devices;
        private Dictionary<int, DoubleBufferShowable> indices;
    }
}
