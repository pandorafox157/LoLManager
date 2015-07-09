using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace LoLManager
{
    public class ChatControl
    {
        CFGFile GamePermanentCFG;
        public ChatControl(string path)
        {
            GamePermanentCFG = new CFGFile(path + "CFG\\defaults\\GamePermanent_zh_TW.cfg");
        }

        public string GetData(string Section, string Tag)
        {
            return GamePermanentCFG.GetValue(Section, Tag);
        }
        public void SetChatSize(string Name, int FontSize, int OutlineSize, int GlowSize)
        {
            GamePermanentCFG.SetValue(Name, "FontSize", " " + FontSize.ToString());
            GamePermanentCFG.SetValue(Name, "OutlineSize", " " + OutlineSize.ToString());
            GamePermanentCFG.SetValue(Name, "Glow", " " + GlowSize.ToString());
        }
        public void SetChatColorBold(string Name, Color Color, bool Bold, Color OutlineColor, Color GlowColor)
        {
            GamePermanentCFG.SetValue(Name, "Color", " " + Color.R + " " + Color.G + " " + Color.B);
            GamePermanentCFG.SetValue(Name, "OutlineColor", " " + OutlineColor.R + " " + OutlineColor.G + " " + OutlineColor.B);
            GamePermanentCFG.SetValue(Name, "GlowColor", " " + GlowColor.R + " " + GlowColor.G + " " + GlowColor.B);
            GamePermanentCFG.SetValue(Name, "Bold", " " + (Bold ? "1" : "0"));
        }
        public void SetFontName(string Name, string FontName)
        {
            GamePermanentCFG.SetValue(Name, "FontName", " " + FontName);
        }
        public Color StringToColor(string _Color)
        {
            string R = _Color.Substring(0, _Color.IndexOf(" "));
            _Color = _Color.Substring(_Color.IndexOf(" ") + 1);
            string G = _Color.Substring(0, _Color.IndexOf(" "));
            _Color = _Color.Substring(_Color.IndexOf(" ") + 1);
            string B = _Color;
            return Color.FromArgb(255, Int32.Parse(R), Int32.Parse(G), Int32.Parse(B));
        }
    }
}
