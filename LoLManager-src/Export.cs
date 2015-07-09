using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace LoLManager
{
    static public class Export
    {
        static public void Start(string LoLpath) 
        {
            FileInfo File;
            System.Diagnostics.Process Process;
            XmlDocument FontTypeXML = new XmlDocument();
            FontTypeXML.Load(LoLpath + "CFG\\defaults\\FontTypes.xml");

            Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\CFG\\defaults\\");
            Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\Fonts\\");

            File = new FileInfo(LoLpath + "CFG\\defaults\\FontTypes.xml");
            File.CopyTo("CFG\\defaults\\FontTypes.xml", true);
            File = new FileInfo(LoLpath + "CFG\\defaults\\FontDescriptions.xml");
            File.CopyTo("CFG\\defaults\\FontDescriptions.xml", true);
            File = new FileInfo(LoLpath + "CFG\\defaults\\FontResolutions.xml");
            File.CopyTo("CFG\\defaults\\FontResolutions.xml", true);
            File = new FileInfo(LoLpath + "CFG\\defaults\\GamePermanent_zh_TW.cfg");
            File.CopyTo("CFG\\defaults\\GamePermanent_zh_TW.cfg", true);
            File = new FileInfo(LoLpath + "Fonts\\FontMappings.txt");
            File.CopyTo("Fonts\\FontMappings.txt", true);

            XmlNodeList TypNodes = FontTypeXML["FontTypes"].ChildNodes;
            List<string> NameList = new List<string>();
            foreach (XmlNode it in TypNodes)
            {
                if (it.Attributes != null)
                {
                    if (!IsBold(it.Attributes["Name"].Value) && it.Attributes["Name"].Value != "Regular")
                    {
                        foreach (XmlNode it2 in it)
                        {
                            if (it2.Attributes != null)
                            {
                                if (it2.Attributes["Locale"].Value == "zh_tw")
                                {
                                    File = new FileInfo(LoLpath + it2.Attributes["Resource"].Value.Substring(5));
                                    File.CopyTo("Fonts\\" + it2.Attributes["Resource"].Value.Substring(11), true);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
            StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
            StartInfo.Verb = "runas";
            StartInfo.FileName = "7z.exe";
            StartInfo.Arguments = "a -tzip backup.zip CFG/ Fonts/";
            Process = System.Diagnostics.Process.Start(StartInfo);
            Process.WaitForExit();

            Directory.Delete(System.Environment.CurrentDirectory + "\\CFG\\", true);
            Directory.Delete(System.Environment.CurrentDirectory + "\\Fonts\\", true);
        }
        static bool IsBold(string Value)
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
