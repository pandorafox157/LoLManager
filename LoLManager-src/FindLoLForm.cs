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
    public partial class FindLoLForm : Form
    {
        string RootPath;
        public FindLoLForm()
        {
            InitializeComponent();
        }

        private void FindLoLForm_Load(object sender, EventArgs e)
        {
            string Path = "";

            if (HaveReg("SOFTWARE\\Wow6432Node\\Garena\\LoLTW"))
            {
                Path = ReadReg("SOFTWARE\\Wow6432Node\\Garena\\LoLTW", "Path");
            }
            if (Path == "")
            {
                if (HaveReg("SOFTWARE\\Wow6432Node\\Riot Games\\League of Legends"))
                {
                    Path = ReadReg("SOFTWARE\\Wow6432Node\\Riot Games\\League of Legends", "Path");
                }
            }
            if (Path == "")
            {
                if (HaveReg("SOFTWARE\\Garena\\LoLTW"))
                {
                    Path = ReadReg("SOFTWARE\\Garena\\LoLTW", "Path");
                }
            }
            if (Path == "")
            {
                if (HaveReg("SOFTWARE\\Riot Games\\League of Legends"))
                {
                    Path = ReadReg("SOFTWARE\\Riot Games\\League of Legends", "Path");
                }
            }
            if (Path == "")
            {
                if (HaveReg("SOFTWARE\\LOLManager\\LoLTW"))
                {
                    Path = ReadReg("SOFTWARE\\LOLManager\\LoLTW", "Path");
                }
            }

            if (Path == "")
            {
                MessageBox.Show("系統無法利用Regedit讀取LOL安裝位置", "無法抓取LOL安裝位置");
            }
            if (Path != "")
            {
                if (Directory.Exists(Path + "\\Game\\DATA\\") == false)
                {
                    MessageBox.Show("Regedit註冊的LOL安裝位置無效", "遺失LOL位置");
                }
                else 
                {
                    PathListBox.Items.Add(Path);
                }
            }

        }
        private void FindLOL()
        {
            if (FolderBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {

            }
        }
        public static bool WriteReg(string SubKeyPath, string keyName, string keyValue)
        {
            bool bolReturnValue = false;
            try
            {
                RegistryKey rootKey = Registry.LocalMachine;
                RegistryKey subKey = rootKey.OpenSubKey(@SubKeyPath);
                if (subKey == null)
                {
                    rootKey.CreateSubKey(SubKeyPath);
                }
                subKey = rootKey.OpenSubKey(SubKeyPath, true);
                subKey.SetValue(keyName, keyValue);
                bolReturnValue = true;
                subKey.Close();
                rootKey.Close();
            }
            catch (Exception)
            {

            }
            return bolReturnValue;
        }

        public string ReadReg(string SubKeyPath, string keyName)
        {
            string strkeyValue = "";

            try
            {
                RegistryKey rootkey = Registry.LocalMachine;
                RegistryKey subKey = rootkey.OpenSubKey(SubKeyPath);
                strkeyValue = subKey.GetValue(keyName).ToString();
                subKey.Close();
                rootkey.Close();
            }
            catch (Exception)
            {

            }
            return strkeyValue;
        }

        public bool HaveReg(string SubKeyPath)
        {
            bool bolReturnValue = false;
            try
            {
                RegistryKey rootKey = Registry.LocalMachine;
                RegistryKey subKey = rootKey.OpenSubKey(@SubKeyPath);
                if (subKey == null)
                {
                    bolReturnValue = false;
                }
                else
                {
                    bolReturnValue = true;
                }
                subKey.Close();
                rootKey.Close();
            }
            catch (Exception)
            {

            }
            return bolReturnValue;
        }
    }
}
