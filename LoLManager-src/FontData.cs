using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LoLManager
{
    public class FontData
    {
        public string Name;
        public string Type;
        public string Resolution;
        public Color FontColor;
        public Color OutlineColor;
        public double Scale;
        public double Size;
        public double OutlineSize;
        public bool Bold;

        public FontData(string _Name, string _Type, string _Resolution, double _Size, double _OutlineSize, string _Color, double _Scale, string _OutlineColor) 
        {
            Name = _Name;
            Type = _Type;
            Resolution = _Resolution;
            FontColor = StringToColor(_Color);
            OutlineColor = StringToColor(_OutlineColor);
            Scale = _Scale;
            Size = _Size;
            OutlineSize = _OutlineSize;
            Bold = Type.Length > 7;
            if (Scale <= 0)
            {
                Scale = 1;
            }
        }
        public Color StringToColor(string _Color)
        {
            string R = _Color.Substring(0, _Color.IndexOf(","));
            _Color = _Color.Substring(_Color.IndexOf(",") + 1);
            string G = _Color.Substring(0, _Color.IndexOf(","));
            _Color = _Color.Substring(_Color.IndexOf(",") + 1);
            string B = _Color.Substring(0, _Color.IndexOf(","));
            _Color = _Color.Substring(_Color.IndexOf(",") + 1);
            string A = _Color;
            return Color.FromArgb(Int32.Parse(A), Int32.Parse(R), Int32.Parse(G), Int32.Parse(B));
        }
    }
}
