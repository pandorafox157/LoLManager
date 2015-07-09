using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoLManager
{
    public class FontSetting
    {
        public double MinYVelocity;
        public double MaxYVelocity;
        public double ContinualForceY;
        public double Decay;
        public double ShrinkTime;
        public double ShrinkScale;
        public bool[] EnableList = new bool[6];
    }
}
