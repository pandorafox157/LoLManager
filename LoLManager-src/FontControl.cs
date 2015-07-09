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
    public class FontControl
    {
        XmlDocument FontResolutionXML = new XmlDocument();
        XmlDocument FontDescriptionXML = new XmlDocument();
        XmlDocument FontTypeXML = new XmlDocument();
        string FontMapping;
        string FilePath;
        CFGFile GamePermanentCFG;
        public Dictionary<string, PrivateFontCollection> FontMap = new Dictionary<string,PrivateFontCollection>();
        List<string> Blind = new List<string>();
        
        public FontControl(string path) 
        {
            Blind.Add("Critical");
            Blind.Add("PhysicalDamage");
            Blind.Add("EnemyPhysicalDamage");
            Blind.Add("EnemyCritical");

            FilePath = path;

            GamePermanentCFG = new CFGFile(FilePath + "CFG\\defaults\\GamePermanent_zh_TW.cfg");
            StreamReader StreamReader = new StreamReader(FilePath + "Fonts\\FontMappings.txt");
            FontMapping = StreamReader.ReadToEnd();
            StreamReader.Close();
            StreamWriter StreamWriter;
            try
            {
                FontResolutionXML.Load(FilePath + "CFG\\defaults\\FontResolutions.xml");
            }
            catch (Exception)
            {
                StreamReader = new StreamReader(FilePath + "CFG\\defaults\\FontResolutions.xml");
                string FixData = StreamReader.ReadToEnd();
                StreamReader.Close();

                FixData = FixData.Replace("\"ShadowColor", "\" ShadowColor");
                StreamWriter = new StreamWriter(FilePath + "CFG\\defaults\\FontResolutions.xml");
                StreamWriter.Write(FixData);
                StreamWriter.Close();

                FontResolutionXML.Load(FilePath + "CFG\\defaults\\FontResolutions.xml");
                MessageBox.Show("FontResolutions.xml有損,簡易修正完成.");
            }
            try
            {
                FontDescriptionXML.Load(FilePath + "CFG\\defaults\\FontDescriptions.xml");
            }
            catch (Exception)
            {
                StreamReader = new StreamReader(FilePath + "CFG\\defaults\\FontDescriptions.xml");
                string FixData = StreamReader.ReadToEnd();
                StreamReader.Close();

                FixData = FixData.Replace("\"ShadowColor", "\" ShadowColor");
                StreamWriter = new StreamWriter(FilePath + "CFG\\defaults\\FontDescriptions.xml");
                StreamWriter.Write(FixData);
                StreamWriter.Close();

                FontDescriptionXML.Load(FilePath + "CFG\\defaults\\FontDescriptions.xml");
                MessageBox.Show("FontDescriptions.xml有損,簡易修正完成.");
            }
            try
            {
                FontTypeXML.Load(FilePath + "CFG\\defaults\\FontTypes.xml");
            }
            catch (Exception)
            {
                StreamReader = new StreamReader(FilePath + "CFG\\defaults\\FontTypes.xml");
                string FixData = StreamReader.ReadToEnd();
                StreamReader.Close();

                FixData = FixData.Replace("\"ShadowColor", "\" ShadowColor");
                StreamWriter = new StreamWriter(FilePath + "CFG\\defaults\\FontTypes.xml");
                StreamWriter.Write(FixData);
                StreamWriter.Close();

                FontTypeXML.Save(FilePath + "CFG\\defaults\\FontTypes.xml");
                MessageBox.Show("FontTypes.xml有損,簡易修正完成.");
            }
        }
        public void Release()
        {
            FontResolutionXML.Load("Release.xml");
            FontDescriptionXML.Load("Release.xml");
            FontTypeXML.Load("Release.xml");
        }
        public FontSetting GetFontSetting(string Key) 
        {
            FontSetting FontSetting = new FontSetting();
            FontSetting.EnableList[0] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_MinYVelocity"), out FontSetting.MinYVelocity);
            FontSetting.EnableList[1] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_MaxYVelocity"), out FontSetting.MaxYVelocity);
            FontSetting.EnableList[2] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_ContinualForceY"), out FontSetting.ContinualForceY);
            FontSetting.EnableList[3] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_Decay"), out FontSetting.Decay);
            FontSetting.EnableList[4] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_ShrinkTime"), out FontSetting.ShrinkTime);
            FontSetting.EnableList[5] = Double.TryParse(GamePermanentCFG.GetValue("FloatingText", Key + "_ShrinkScale"), out FontSetting.ShrinkScale);
            return FontSetting;
        }
        public void SetFontSetting(string Key, FontSetting FontSetting)
        {
            if (FontSetting.EnableList[0])
                GamePermanentCFG.SetValue("FloatingText", Key + "_MinYVelocity", FontSetting.MinYVelocity.ToString());
            if (FontSetting.EnableList[1])
                GamePermanentCFG.SetValue("FloatingText", Key + "_MaxYVelocity", FontSetting.MaxYVelocity.ToString());
            if (FontSetting.EnableList[2])
                GamePermanentCFG.SetValue("FloatingText", Key + "_ContinualForceY", FontSetting.ContinualForceY.ToString());
            if (FontSetting.EnableList[3])
                GamePermanentCFG.SetValue("FloatingText", Key + "_Decay", FontSetting.Decay.ToString());
            if (FontSetting.EnableList[4])
                GamePermanentCFG.SetValue("FloatingText", Key + "_ShrinkTime", FontSetting.ShrinkTime.ToString());
            if (FontSetting.EnableList[5]) 
                GamePermanentCFG.SetValue("FloatingText", Key + "_ShrinkScale", FontSetting.ShrinkScale.ToString());
        }
        public void Backup() 
        {
            FontResolutionXML.Load("FontResolutions.xml");
            FontDescriptionXML.Load("FontDescriptions.xml");
            FontResolutionXML.Save(FilePath + "CFG\\defaults\\FontResolutions.xml");
            FontDescriptionXML.Save(FilePath + "CFG\\defaults\\FontDescriptions.xml");
        }
        public void BackupFont()
        {
            FontTypeXML.Load("FontTypes.xml");
            FontTypeXML.Save(FilePath + "CFG\\defaults\\FontTypes.xml");
            XmlNodeList DesNodes = FontDescriptionXML["FontDescriptions"].ChildNodes;
            foreach (XmlNode it in DesNodes)
            {
                if (it.Attributes != null)
                {
                    it.Attributes["Type"].Value = IsBold(it.Attributes["Type"].Value) ? "RegularBold" : "Regular";
                }
            }
            FontDescriptionXML.Save(FilePath + "CFG\\defaults\\FontDescriptions.xml");
        }

        public void SetGamePermanent(string Section, string Key, string Value)
        {
            GamePermanentCFG.SetValue(Section, Key, Value);
        }

        public FontData GetFont(string Name)
        {
            XmlNodeList ResNodes = FontResolutionXML["FontResolutions"].ChildNodes;
            XmlNodeList DesNodes = FontDescriptionXML["FontDescriptions"].ChildNodes;
            string Resolution = "";
            string Type = "";
            string Color = "";
            string Scale = "1";
            string OutlineColor = "";
            double Size = 0;
            double OutlineSize = 0;
            foreach (XmlNode it in DesNodes)
            {
                if (it.Attributes["Name"].Value == Name)
                {
                    Resolution = it.Attributes["Resolution"].Value;
                    Type = it.Attributes["Type"].Value == "RegularBoldBold" ? "RegularBold" : it.Attributes["Type"].Value;
                    Color = it["Display"].Attributes["Color"].Value;
                    OutlineColor = it["Display"].Attributes["OutlineColor"] != null ? it["Display"].Attributes["OutlineColor"].Value : "0,0,0,255";
                }
            }
            foreach (XmlNode it in ResNodes)
            {
                if (it.Attributes != null) 
                {
                    if (it.Attributes["Name"].Value == Resolution)
                    {
                        if (it["locale"] == null)
                        {
                            Double.TryParse(Resolution.Substring(Resolution.IndexOf("_") + 1, Resolution.IndexOf("_", Resolution.IndexOf("_") + 1) - Resolution.IndexOf("_") - 1), out OutlineSize);
                            Double.TryParse(Resolution.Substring(0, Resolution.IndexOf("_")), out Size);
                            continue;
                        }
                        if (it["locale"]["Resolution"] == null)
                        {
                            Double.TryParse(Resolution.Substring(Resolution.IndexOf("_") + 1, Resolution.IndexOf("_", Resolution.IndexOf("_") + 1) - Resolution.IndexOf("_") - 1), out OutlineSize);
                            Double.TryParse(Resolution.Substring(0, Resolution.IndexOf("_")), out Size);
                            continue;
                        }

                        if (it["locale"]["Resolution"].Attributes["FontSize"] != null)
                        {
                            if (!Double.TryParse(it["locale"]["Resolution"].Attributes["FontSize"].Value, out Size))
                            {
                                Double.TryParse(Resolution.Substring(0, Resolution.IndexOf("_")), out Size);
                            }
                        }
                        else 
                        {
                            Double.TryParse(Resolution.Substring(0, Resolution.IndexOf("_")), out Size);
                        }
                        if (it["locale"]["Resolution"].Attributes["OutlineSize"] != null)
                        {
                            if (!Double.TryParse(it["locale"]["Resolution"].Attributes["OutlineSize"].Value, out OutlineSize))
                            {
                                Double.TryParse(Resolution.Substring(Resolution.IndexOf("_") + 1, Resolution.IndexOf("_", Resolution.IndexOf("_") + 1) - Resolution.IndexOf("_") - 1), out OutlineSize);
                            }
                        }
                        else
                        {
                            Double.TryParse(Resolution.Substring(Resolution.IndexOf("_") + 1, Resolution.IndexOf("_", Resolution.IndexOf("_") + 1) - Resolution.IndexOf("_") - 1), out OutlineSize);
                        }
                    }
                }
            }
            Scale = GamePermanentCFG.GetValue("FloatingText", Name + "_ShrinkScale");
            if (Scale == "")
            {
                Scale = "1";
            }
            return new FontData(Name, Type, Resolution, Size, OutlineSize, Color, Double.Parse(Scale), OutlineColor);
        }
        public void SetFontSize(string Name, int FontSize, int OutlineSize)
        {
            XmlNodeList ResNodes = FontResolutionXML["FontResolutions"].ChildNodes;
            XmlNodeList DesNodes = FontDescriptionXML["FontDescriptions"].ChildNodes;
            XmlNode ResTarget = null;
            XmlNode DesTarget = null;
            string OldResolution = "";
            string NewResolution = "";

            foreach (XmlNode it in DesNodes)
            {
                if (it.Attributes != null)
                {
                    if (it.Attributes["Name"].Value == Name)
                    {
                        DesTarget = it;
                        OldResolution = it.Attributes["Resolution"].Value;
                    }
                }
            }
            NewResolution = FontSize.ToString() + "_" + OutlineSize.ToString() + OldResolution.Substring(OldResolution.IndexOf("_", OldResolution.IndexOf("_") + 1));
            foreach (XmlNode it in ResNodes)
            {
                if (it.Attributes != null)
                {
                    if (it.Attributes["Name"].Value == NewResolution)
                    {
                        ResTarget = it;
                    }
                }
            }
            if (ResTarget == null)
            {
                XmlNode Node = FontResolutionXML.SelectSingleNode("FontResolutions");

                XmlElement FontResolutionElement = FontResolutionXML.CreateElement("FontResolution");
                FontResolutionElement.SetAttribute("Name", NewResolution);
                FontResolutionElement.SetAttribute("AutoScale", "True");
                Node.AppendChild(FontResolutionElement);

                XmlElement LocaleElement = FontResolutionXML.CreateElement("locale");
                LocaleElement.SetAttribute("Name", "en_us");
                FontResolutionElement.AppendChild(LocaleElement);

                XmlElement ResolutionElement = FontResolutionXML.CreateElement("Resolution");
                ResolutionElement.SetAttribute("ScreenHeight", "1080");
                ResolutionElement.SetAttribute("FontSize", FontSize.ToString());
                ResolutionElement.SetAttribute("OutlineSize", OutlineSize.ToString());
                LocaleElement.AppendChild(ResolutionElement);
            }
            DesTarget.Attributes["Resolution"].Value = NewResolution;

            FontResolutionXML.Save(FilePath + "CFG\\defaults\\FontResolutions.xml");
            FontDescriptionXML.Save(FilePath + "CFG\\defaults\\FontDescriptions.xml");
        }
        public void SetFontColorBold(string Name, Color Color, bool Bold, Color OutlineColor)
        {
            XmlNodeList DesNodes = FontDescriptionXML["FontDescriptions"].ChildNodes;
            
            foreach (XmlNode it in DesNodes)
            {
                if (it.Attributes["Name"].Value == Name)
                {
                    it["Display"].Attributes["Color"].Value = Color.R + "," + Color.G + "," + Color.B + "," + Color.A;
                    it["Display"].Attributes["OutlineColor"].Value = OutlineColor.R + "," + OutlineColor.G + "," + OutlineColor.B + "," + OutlineColor.A;
                    it.Attributes["Type"].Value = SetBold(it.Attributes["Type"].Value, Bold);
                    if (Blind.Contains(Name))
                    {
                        it["ColorBlindDisplay"].Attributes["Color"].Value = Color.R + "," + Color.G + "," + Color.B + "," + Color.A;
                        it["ColorBlindDisplay"].Attributes["OutlineColor"].Value = OutlineColor.R + "," + OutlineColor.G + "," + OutlineColor.B + "," + OutlineColor.A;
                    }
                }
            }
            FontDescriptionXML.Save(FilePath + "CFG\\defaults\\FontDescriptions.xml");
        }
        public void SetFontType(string FontName, string TypeName)
        {
            XmlNodeList DesNodes = FontDescriptionXML["FontDescriptions"].ChildNodes;
            foreach (XmlNode it in DesNodes)
            {
                if (it.Attributes != null)
                {
                    if (it.Attributes["Name"].Value == FontName)
                    {
                        it.Attributes["Type"].Value = IsBold(it.Attributes["Type"].Value) ? TypeName + "Bold" : TypeName;
                    }
                }
            }
            FontDescriptionXML.Save(FilePath + "CFG\\defaults\\FontDescriptions.xml");
        }
        public List<string> GetFontList() 
        {
            XmlNodeList TypNodes = FontTypeXML["FontTypes"].ChildNodes;
            List<string> NameList = new List<string>();
            List<XmlNode> RemoveNode = new List<XmlNode>();
            foreach (XmlNode it in TypNodes)
            {
                if (it.Attributes != null)
                {
                    NameList.Add(it.Attributes["Name"].Value);
                    if (!FontMap.ContainsKey(it.Attributes["Name"].Value))
                    {
                        FontMap.Add(it.Attributes["Name"].Value, new PrivateFontCollection());
                        foreach (XmlNode it2 in it)
                        {
                            if (it2.Attributes != null)
                            {
                                if (it2.Attributes["Locale"].Value == "zh_tw")
                                {
                                    if (File.Exists(FilePath + it2.Attributes["Resource"].Value.Substring(5)))
                                    {
                                        FontMap[it.Attributes["Name"].Value].AddFontFile(FilePath + it2.Attributes["Resource"].Value.Substring(5));
                                    }
                                    else 
                                    {
                                        MessageBox.Show("先前匯入的字型檔案遺失\n" + it.Attributes["Name"].Value + " 無法被抓取\n如要使用該字型請重新匯入", "警告");
                                        FontMap.Remove(it.Attributes["Name"].Value);
                                        NameList.Remove(it.Attributes["Name"].Value);
                                        RemoveNode.Add(it);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            foreach (XmlNode it in RemoveNode)
            {
                FontTypeXML["FontTypes"].RemoveChild(it);
                FontTypeXML.Save(FilePath + "CFG\\defaults\\FontTypes.xml");
            }
            return NameList;
        }
        public void AddFontToMapping(string FontPath,string FontName)
        {
            FileInfo File = new FileInfo(FontPath);
            File.CopyTo(FilePath + "Fonts\\" + FontName + ".TTF", true);
            FontMapping += "\n\"" + FontName + "\" \"DATA\\Fonts\\" + FontName + ".ttf\" \n";
            StreamWriter StreamWriter = new StreamWriter(FilePath + "Fonts\\FontMappings.txt");
            StreamWriter.Write(FontMapping);
            StreamWriter.Close();
            
            XmlNode Node = FontTypeXML.SelectSingleNode("FontTypes");

            XmlElement FontTypeElement = FontTypeXML.CreateElement("FontType");
            FontTypeElement.SetAttribute("Name", FontName);
            Node.AppendChild(FontTypeElement);

            XmlElement LocaleElement = FontTypeXML.CreateElement("locale");
            LocaleElement.SetAttribute("Locale", "en_us");
            LocaleElement.SetAttribute("Type", FontName);
            LocaleElement.SetAttribute("Resource", "data/fonts/" + FontName + ".TTF");
            LocaleElement.SetAttribute("Bold", "false");
            LocaleElement.SetAttribute("Italic", "false");
            LocaleElement.SetAttribute("Mipped", "true");
            FontTypeElement.AppendChild(LocaleElement);

            LocaleElement = FontTypeXML.CreateElement("locale");
            LocaleElement.SetAttribute("Locale", "zh_tw");
            LocaleElement.SetAttribute("Type", FontName);
            LocaleElement.SetAttribute("Resource", "data/fonts/" + FontName + ".TTF");
            LocaleElement.SetAttribute("Bold", "false");
            LocaleElement.SetAttribute("Italic", "false");
            LocaleElement.SetAttribute("Mipped", "true");
            FontTypeElement.AppendChild(LocaleElement);

            FontTypeElement = FontTypeXML.CreateElement("FontType");
            FontTypeElement.SetAttribute("Name", FontName + "Bold");
            Node.AppendChild(FontTypeElement);

            LocaleElement = FontTypeXML.CreateElement("locale");
            LocaleElement.SetAttribute("Locale", "en_us");
            LocaleElement.SetAttribute("Type", FontName);
            LocaleElement.SetAttribute("Resource", "data/fonts/" + FontName + ".TTF");
            LocaleElement.SetAttribute("Bold", "true");
            LocaleElement.SetAttribute("Italic", "false");
            LocaleElement.SetAttribute("Mipped", "true");
            FontTypeElement.AppendChild(LocaleElement);

            LocaleElement = FontTypeXML.CreateElement("locale");
            LocaleElement.SetAttribute("Locale", "zh_tw");
            LocaleElement.SetAttribute("Type", FontName);
            LocaleElement.SetAttribute("Resource", "data/fonts/" + FontName + ".TTF");
            LocaleElement.SetAttribute("Bold", "true");
            LocaleElement.SetAttribute("Italic", "false");
            LocaleElement.SetAttribute("Mipped", "true");
            FontTypeElement.AppendChild(LocaleElement);

            FontTypeXML.Save(FilePath + "CFG\\defaults\\FontTypes.xml");
        }

        public string SetBold(string Value, bool Bold)
        {
            if (Bold && !IsBold(Value))
            {
                Value += "Bold";
            }
            else if (!Bold && IsBold(Value))
            {
                Value = Value.Substring(0, Value.Length - 4);
            }
            return Value;
        }

        public bool IsBold(string Value)
        {
            if (Value.Length <= 4)
            {
                return false;
            }
            else
            {
                if (Value.Substring(Value.Length - 4) == "Bold")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
