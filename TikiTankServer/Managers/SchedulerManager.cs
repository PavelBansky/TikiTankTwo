using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using TikiTankCommon;
using TikiTankCommon.Converters;

namespace TikiTankServer.Managers
{
    public enum DeviceType
    {
        Treads,
        Barrel,
        Panels,
    }
    public class SchedulerManager
    {
        private const int THREAD_JOIN_WAIT = 2000;
        private const string DEVICES_FILE = "Json/devices.json";
        private const string PATTERNS_FILE = "Json/patterns.json";

        private const int TREAD_INDEX = 0;
        private const int BARREL_INDEX = 1;
        private const int PANELS_INDEX = 2;

        public SchedulerManager()
        {
           scheduler = new FrameScheduler(TimeSpan.FromMilliseconds(50));

        }

        public void Start()
        {
            _isRunning = true;
            _thread = new Thread(DoWork);
            _thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _thread.Join(THREAD_JOIN_WAIT);
        }

        public void DoWork()
        {
            LoadDevices();
            LoadPatterns();

            _patterns = new List<IPattern>();
            _patterns.Add(treadPatterns[0]);
            _patterns.Add(barrelPatterns[0]);
            _patterns.Add(panelsPatterns[0]);

            while(_isRunning)
            {
                scheduler.Show(_patterns); 
                
                if (_changeRequest)
                {
                    _patterns = new List<IPattern>();
                    _patterns.Add(_tempPatterns[0]);
                    _patterns.Add(_tempPatterns[1]);
                    _patterns.Add(_tempPatterns[2]);
                    _changeRequest = false;
                }
            }

        }

        private void LoadDevices()
        {
            Console.WriteLine("Scheduler Manager: Loading {0}", DEVICES_FILE);

            using (StreamReader srDev = new StreamReader(DEVICES_FILE))
            {
                // json device file, parse once
                 devices = JsonConvert.DeserializeObject<List<IShowable>>(
                    srDev.ReadToEnd(),
                    new DeviceConverter());

                foreach (IShowable show in devices)
                {
                    show.Init();
                    scheduler.AddDevice(show);
                }
            }
        }

        private void LoadPatterns()
        {
            Console.WriteLine("Scheduler Manager: Loading {0}", PATTERNS_FILE);

            List<IPattern> patterns = new List<IPattern>();
            using (StreamReader srPat = new StreamReader(PATTERNS_FILE))
            {
                // json parse patterns file
                patterns = JsonConvert.DeserializeObject<List<IPattern>>(
                    srPat.ReadToEnd(),
                    new PatternConverter());
            }

            treadPatterns = new List<IPattern>();
            barrelPatterns = new List<IPattern>();
            panelsPatterns = new List<IPattern>();

            foreach(IPattern pattern in patterns)
            {                
                if (pattern.OutputDevice == "Tread")
                    treadPatterns.Add(pattern);
                else if (pattern.OutputDevice == "Barrel")
                    barrelPatterns.Add(pattern);
                else if (pattern.OutputDevice == "Panels")
                    panelsPatterns.Add(pattern);
            }
        }


        public List<EffectInformation> GetEffectsInformation(DeviceType deviceType)
        {
            List<IPattern> effectList;
            switch (deviceType)
            {
                case DeviceType.Barrel:
                    effectList = barrelPatterns;
                    break;
                case DeviceType.Panels:
                    effectList = panelsPatterns;
                    break;
                default:
                    effectList = treadPatterns;
                    break;
            }

            List<EffectInformation> result = new List<EffectInformation>();
            for (int i = 0; i < effectList.Count; i++)
            {
                EffectInformation info = new EffectInformation(effectList[i].Name);
                info.Id = i;
                result.Add(info);
            }

            return result;
        }

        public void SelectEffect(DeviceType deviceType, int index)
        {
            List<IPattern> effectList;
            int i = 0;
            switch (deviceType)
            {
                case DeviceType.Barrel:
                    effectList = barrelPatterns;
                    i = BARREL_INDEX;
                    break;
                case DeviceType.Panels:
                    effectList = panelsPatterns;
                    i = PANELS_INDEX;
                    break;
                default:
                    effectList = treadPatterns;
                    i = TREAD_INDEX;
                    break;
            }

            _tempPatterns = new List<IPattern>(_patterns);            
            _tempPatterns[i] = effectList[index];            
            _changeRequest = true;

        }
       
        private List<IPattern> treadPatterns, barrelPatterns, panelsPatterns;
        private List<IShowable> devices;
        private List<IPattern> _patterns,_tempPatterns;
        private FrameScheduler scheduler;
        private Thread _thread;
        private bool _isRunning, _changeRequest;
    }
}
