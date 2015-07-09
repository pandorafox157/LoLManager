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
    public class BarSetting
    {
        //HealthBarColor = 0 180 0 255
        //HealthFadeColor = 255 0 0 255
        //AllShieldColor = 170 170 170 255 
        //PhysShieldColor = 255 103 0 255 
        //MagicShieldColor = 153 0 255 255
        CFGFile GamePermanentCFG;
        public BarSetting(string path)
        {
            FileInfo iniFile;
            if (!File.Exists(path + "Menu\\HUD\\GeneralCharacterData_Legacy.ini"))
            {
                iniFile = new FileInfo("GeneralCharacterData_Legacy.ini");
                iniFile.CopyTo(path + "Menu\\HUD\\GeneralCharacterData_Legacy.ini", true);
            }
            GamePermanentCFG = new CFGFile(path + "Menu\\HUD\\GeneralCharacterData_Legacy.ini");
        }

        public void SetHealthBar(
            string Target, 
            Color HealthBarColor, 
            Color HealthFadeColor,
            Color AllShieldColor,
            Color PhysShieldColor,
            Color MagicShieldColor) 
        {
            GamePermanentCFG.SetValue(Target, "HealthBarColor", " " + HealthBarColor.R + " " + HealthBarColor.G + " " + HealthBarColor.B + " " + HealthBarColor.A);
            GamePermanentCFG.SetValue(Target, "HealthFadeColor", " " + HealthFadeColor.R + " " + HealthFadeColor.G + " " + HealthFadeColor.B + " " + HealthFadeColor.A);
            GamePermanentCFG.SetValue(Target, "AllShieldColor", " " + AllShieldColor.R + " " + AllShieldColor.G + " " + AllShieldColor.B + " " + AllShieldColor.A);
            GamePermanentCFG.SetValue(Target, "PhysShieldColor", " " + PhysShieldColor.R + " " + PhysShieldColor.G + " " + PhysShieldColor.B + " " + PhysShieldColor.A);
            GamePermanentCFG.SetValue(Target, "MagicShieldColor", " " + MagicShieldColor.R + " " + MagicShieldColor.G + " " + MagicShieldColor.B + " " + MagicShieldColor.A);
        }

        public void SetBar(
            Color MPBarColor,
            Color EnergyBarColor,
            Color ShieldBarColor,
            Color OtherBarColor,
            Color MPFadeColor,
            Color EnergyFadeColor,
            Color ShieldFadeColor,
            Color OtherFadeColor)
        {
            GamePermanentCFG.SetValue("GeneralDataHero", "MPBarColor", " " + MPBarColor.R + " " + MPBarColor.G + " " + MPBarColor.B + " " + MPBarColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "EnergyBarColor", " " + EnergyBarColor.R + " " + EnergyBarColor.G + " " + EnergyBarColor.B + " " + EnergyBarColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "ShieldBarColor", " " + ShieldBarColor.R + " " + ShieldBarColor.G + " " + ShieldBarColor.B + " " + ShieldBarColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "OtherBarColor", " " + OtherBarColor.R + " " + OtherBarColor.G + " " + OtherBarColor.B + " " + OtherBarColor.A);

            GamePermanentCFG.SetValue("GeneralDataHero", "MPFadeColor", " " + MPFadeColor.R + " " + MPFadeColor.G + " " + MPFadeColor.B + " " + MPFadeColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "EnergyFadeColor", " " + EnergyFadeColor.R + " " + EnergyFadeColor.G + " " + EnergyFadeColor.B + " " + EnergyFadeColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "ShieldFadeColor", " " + ShieldFadeColor.R + " " + ShieldFadeColor.G + " " + ShieldFadeColor.B + " " + ShieldFadeColor.A);
            GamePermanentCFG.SetValue("GeneralDataHero", "OtherFadeColor", " " + OtherFadeColor.R + " " + OtherFadeColor.G + " " + OtherFadeColor.B + " " + OtherFadeColor.A);
        }

        public string GetData(string Section, string Tag)
        {
            return GamePermanentCFG.GetValue(Section, Tag);
        }

        public Color StringToColor(string _Color)
        {
            string R = _Color.Substring(0, _Color.IndexOf(" "));
            _Color = _Color.Substring(_Color.IndexOf(" ") + 1);
            string G = _Color.Substring(0, _Color.IndexOf(" "));
            _Color = _Color.Substring(_Color.IndexOf(" ") + 1);
            string B = _Color.Substring(0, _Color.IndexOf(" "));
            _Color = _Color.Substring(_Color.IndexOf(" ") + 1);
            string A = _Color;
            return Color.FromArgb(Int32.Parse(A), Int32.Parse(R), Int32.Parse(G), Int32.Parse(B));
        }
    }
}
