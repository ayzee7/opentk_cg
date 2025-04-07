using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace opentk_cg.Common
{
    class MyTimer
    {
        System.Timers.Timer timer;
        static float exp;
        static float initExp;
        public MyTimer(string renderer)
        {
            if (renderer.Contains("Intel"))
            {
                exp = 10;
                initExp = 10;
            }
            else
            {
                exp = 1;
                initExp = 1;
            }
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 10000;
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            exp *= 1.1f;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            exp = initExp;
            timer.Start();
        }

        public float Exp()
        {
            return exp;
        }
    }
}
