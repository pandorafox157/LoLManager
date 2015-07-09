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
    public partial class LoLManager : Form
    {
        string RootPath = "";
        FontControl FontControl;
        UpdateCheck UpdateCheck;
        FontData FontData;
        CFGFile ManagerINI;
        ChatControl ChatControl;
        //BarSetting BarSetting;
        public LoLManager()
        {
            InitializeComponent();
        }

        private void LoLManager_Load(object sender, EventArgs e)
        {
            ManagerINI = new CFGFile(Directory.GetCurrentDirectory() + "\\LoLManager.ini");
            RootPath = ManagerINI.GetValue("Init", "Path");
            if (RootPath != "" && !Directory.Exists(RootPath))
            {
                RootPath = "";
                ManagerINI.SetValue("Init", "Path", "");
            }
            if (RootPath == "")
            {
                if (HaveReg("SOFTWARE\\Wow6432Node\\Garena\\LoLTW"))
                {
                    RootPath = ReadReg("SOFTWARE\\Wow6432Node\\Garena\\LoLTW", "Path");
                }
            }
            if (RootPath == "")
            {
                if (HaveReg("SOFTWARE\\Wow6432Node\\Riot Games\\League of Legends"))
                {
                    RootPath = ReadReg("SOFTWARE\\Wow6432Node\\Riot Games\\League of Legends", "Path");
                }
            }
            if (RootPath == "")
            {
                if (HaveReg("SOFTWARE\\Garena\\LoLTW"))
                {
                    RootPath = ReadReg("SOFTWARE\\Garena\\LoLTW", "Path");
                }
            }
            if (RootPath == "")
            {
                if (HaveReg("SOFTWARE\\Riot Games\\League of Legends"))
                {
                    RootPath = ReadReg("SOFTWARE\\Riot Games\\League of Legends", "Path");
                }
            }
            if (RootPath == "")
            {
                MessageBox.Show("手動設定\n請選擇您安裝的路徑\nGarena/GameData/Apps/LoLTW", "無法抓取LOL安裝位置");
                FindLOL();
            }
            if (RootPath != "")
            {
                if (Directory.Exists(RootPath + "\\Game\\DATA\\") == false)
                {
                    MessageBox.Show("重新設定LOL位置", "遺失LOL位置");
                    FindLOL();
                }

                UpdateCheck = new UpdateCheck(RootPath);

                // 開啟時必定進入Pack自動比對
                BackupEx.Pack(RootPath, UpdateCheck.GetLoLVersion().ToString());
                UpdateCheck.SetMainVersion();
                foreach (string it in BackupEx.GetListString())
                {
                    BackupExListBox.Items.Add(it);
                }

                if (UpdateCheck.CheakLoLVersion())
                {
                    if (MessageBox.Show("偵測到LoL版本有更新\n是否需要匯入上次匯出的資料?", "匯入系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Import();
                    }
                }
                FontControl = new FontControl(RootPath + "\\Game\\DATA\\");
                ChatControl = new ChatControl(RootPath + "\\Game\\DATA\\");
                //BarSetting = new BarSetting(RootPath + "\\Game\\DATA\\");
                LoLVersionLabel.Text = UpdateCheck.GetLoLVersion().ToString();
                SaveVersionLabel.Text = UpdateCheck.GetSaveVersion().ToString();

                CheckVersionCheckBox.Checked = Int32.Parse(ManagerINI.GetValue("Option", "CheckSaveVersion")) == 0 ? false : true;
                FontNameComboBox.SelectedIndex = 0;
                ReflashFontTypeList();
                NewFontTypeComboBox.SelectedIndex = 0;
                FontSettingComboBox.SelectedIndex = 0;
                ChatSettingComboBox.SelectedIndex = 0;
                HealthBarComboBox.SelectedIndex = 0;
                MpBarLoag();
                LOLPathLabel.Text = RootPath;

                linkLabel1.Links.Add(0, linkLabel1.Text.Length, linkLabel1.Text);
                linkLabel2.Links.Add(0, linkLabel2.Text.Length, linkLabel2.Text);
                linkLabel5.Links.Add(0, linkLabel5.Text.Length, linkLabel5.Text);
                linkLabel6.Links.Add(0, linkLabel6.Text.Length, linkLabel6.Text);
            }
        }
        private void FindLOL() 
        {
            if (FolderBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {
                string p = FolderBrowserDialog.SelectedPath.Substring(FolderBrowserDialog.SelectedPath.Length - 19);
                if (p == "GameData\\Apps\\LoLTW")
                {
                    CFGFile CFGFile = new CFGFile(Directory.GetCurrentDirectory() + "\\LoLManager.ini");
                    CFGFile.SetValue("Init", "Path", FolderBrowserDialog.SelectedPath);
                    RootPath = FolderBrowserDialog.SelectedPath;
                }
                else
                {
                    MessageBox.Show("選擇的路徑無效\n請重新啟動程式.", "無法抓取LOL安裝位置");
                }
            }
            else
            {
                MessageBox.Show("請重新啟動程式.", "無法抓取LOL安裝位置");
            }
        }
        private void FontNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), FontNameComboBox.SelectedIndex).ToString();
            FontData = FontControl.GetFont(FontName);
            ExampleLabel.ForeColor = FontData.FontColor;
            ExampleLabel.BackColor = Color.FromArgb(255 - FontData.FontColor.R, 255 - FontData.FontColor.G, 255 - FontData.FontColor.B);
            FontSizeTextBox.Text = ((int)FontData.Size).ToString();
            OutlineSizeTextBox.Text = ((int)FontData.OutlineSize).ToString();
            FontColorLabel.Text = ExampleLabel.ForeColor.ToString().Substring(6);
            OutlineColorLabel.Text = ExampleLabel.BorderColor.ToString().Substring(6);
            BoldCheckBox.Checked = FontData.Bold;
            UpdateExampleLabel();
            ErrorProvider.SetError(SaveFontButton, "");
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
        private void UpdateExampleLabel() 
        {
            FontControl.GetFontList();
            ExampleLabel.Font = new Font(
                FontControl.FontMap["Regular"].Families[0],
                (float)(FontData.Size * FontData.Scale),
                FontData.Bold ? FontStyle.Bold : FontStyle.Regular,
                GraphicsUnit.Pixel);
            ((BorderLabel)ExampleLabel).BorderSize = (float)FontData.OutlineSize;
            ((BorderLabel)ExampleLabel).BorderColor = FontData.OutlineColor;
            // ((BorderLabel)ExampleLabel);
        }
        private void FontSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            int Result;
            if (FontSizeTextBox.Text != "" && Int32.TryParse(FontSizeTextBox.Text, out Result))
            {
                FontData.Size = Result;
                UpdateExampleLabel();
                ErrorProvider.SetError(SaveFontButton, "數值被修改,請記得點選保存");
            }
        }

        private void ChangeColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = FontData.FontColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                FontData.FontColor = Color.FromArgb(
                    FontData.FontColor.A,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                ExampleLabel.ForeColor = FontData.FontColor;
                ExampleLabel.BackColor = Color.FromArgb(255 - FontData.FontColor.R, 255 - FontData.FontColor.G, 255 - FontData.FontColor.B);
                FontColorLabel.Text = ExampleLabel.ForeColor.ToString().Substring(6);
                ErrorProvider.SetError(SaveFontButton, "數值被修改,請記得點選保存");
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), FontNameComboBox.SelectedIndex).ToString();
            FontControl.SetFontSize(FontName, (int)FontData.Size, (int)FontData.OutlineSize);
            FontControl.SetFontColorBold(FontName, FontData.FontColor, FontData.Bold, FontData.OutlineColor);
            ErrorProvider.SetError(SaveFontButton, "");
        }

        private void BackupButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否啟動字體還原?", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                FontControl.Backup();
                FontNameComboBox.SelectedIndex = 1;
                FontNameComboBox.SelectedIndex = 0;
                MessageBox.Show("已還原.");
            }
        }

        private void OutlineSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            int Result;
            if (OutlineSizeTextBox.Text != "" && Int32.TryParse(OutlineSizeTextBox.Text, out Result))
            {
                FontData.OutlineSize = Result;
                UpdateExampleLabel();
                ErrorProvider.SetError(SaveFontButton, "數值被修改,請記得點選保存");
            }
        }

        private void BoldCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FontData.Bold = BoldCheckBox.Checked;
            UpdateExampleLabel();
            ErrorProvider.SetError(SaveFontButton, "數值被修改,請記得點選保存");
        }

        private void OutlineColorChangeButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = FontData.OutlineColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                FontData.OutlineColor = Color.FromArgb(
                    FontData.OutlineColor.A,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                ExampleLabel.BorderColor = FontData.OutlineColor;
                OutlineColorLabel.Text = ExampleLabel.BorderColor.ToString().Substring(6);
                ErrorProvider.SetError(SaveFontButton, "數值被修改,請記得點選保存");
            }
        }

        private void FontTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void UpdateExampleFontLabel()
        {

        }

        private void BackupFontButton_Click(object sender, EventArgs e)
        {
            FileInfo File;
            /*
            File = new FileInfo("FZLTCH.TTF");
            File.CopyTo(RootPath + "\\Game\\DATA\\Fonts\\FZLTCH.TTF", true);

            File = new FileInfo("FZXHYSZK.TTF");
            File.CopyTo(RootPath + "\\Game\\DATA\\Fonts\\FZXHYSZK.TTF", true);
            */
            if (MessageBox.Show("是否啟動舊版的字型還原?\n*本字型還原將清除新版的字型列表\n*啟動還原將會使用小黑窗進行還原\n 並自動重開本程式", "舊版字型還原", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File = new FileInfo("FontMappings.txt");
                File.CopyTo(RootPath + "\\Game\\DATA\\Fonts\\FontMappings.txt", true);

                FontControl.BackupFont();
                System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
                StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                StartInfo.Verb = "runas";
                StartInfo.FileName = "LoLMConsole.exe";
                StartInfo.Arguments = "BackupFont \"" + RootPath + "\"";
                //System.Diagnostics.Process.Start("LoLManagerBackup.exe", "BackupFont " + RootPath);
                System.Diagnostics.Process.Start(StartInfo);

                this.Close();
                Environment.Exit(Environment.ExitCode);
                InitializeComponent();
            }
        }

        private void NewLoadFontFileButton_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                FileInfo File = new FileInfo(OpenFileDialog.FileName);
                if (HasChinese(File.Name))
                {
                    MessageBox.Show("字型檔案必須以英文或數字命名", "警告");
                }
                else
                {
                    FontControl.AddFontToMapping(File.FullName, File.Name.Substring(0, File.Name.Length - 4).Replace(" ", ""));
                    ReflashFontTypeList();
                    NewFontTypeComboBox.SelectedIndex = 1;
                    NewFontTypeComboBox.SelectedIndex = 0;
                }
            }
        }

        private bool HasChinese(string Value)
        {
            foreach (char it in Value)
            {
                if (it > 128)
                {
                    return true;
                }
            }
            return false;
        }

        private void ReflashFontTypeList()
        {
            List<string> FontList = FontControl.GetFontList();
            NewFontSelectComboBox.Items.Clear();
            foreach (string it in FontList)
            {
                NewFontSelectComboBox.Items.Add(it);
                if (it == "Regular")
                {
                    ChatSettingFontTypeComboBox.Items.Add("FZXHYSZK");
                }
                else 
                {
                    ChatSettingFontTypeComboBox.Items.Add(it);
                }
            }
        }

        private void NewFontTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), NewFontTypeComboBox.SelectedIndex).ToString();
            FontData FontData = FontControl.GetFont(FontName);
            string FontDataTypeA = FontControl.SetBold(FontData.Type, false);
            string FontDataTypeB = FontControl.SetBold(FontData.Type, true);

            if (!NewFontSelectComboBox.Items.Contains(FontDataTypeA) && !NewFontSelectComboBox.Items.Contains(FontDataTypeB))
            {
                MessageBox.Show("字型 " + FontData.Type + " 沒有對應在FontTypes.xml內\n已將本樣板字型初始化為 Regular\n如需使用該字型請再匯入一次", "警告");
                FontData.Type = "Regular";
                FontControl.SetFontType(FontName, "RegularBold");
            }
            if (NewFontSelectComboBox.Items.Contains(FontDataTypeA))
                NewFontSelectComboBox.SelectedIndex = NewFontSelectComboBox.FindString(FontDataTypeA);
            else if (NewFontSelectComboBox.Items.Contains(FontDataTypeB))
                NewFontSelectComboBox.SelectedIndex = NewFontSelectComboBox.FindString(FontDataTypeB);
            else
                NewFontSelectComboBox.SelectedIndex = NewFontSelectComboBox.FindString("Regular");

            ErrorProvider.SetError(SaveFontTypeButton, "");
        }

        private void NewFontSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FontName = NewFontSelectComboBox.SelectedItem.ToString();
            NewExampleFontLable.Font = new Font(
                FontControl.FontMap[FontName].Families[0],
                42,
                FontStyle.Bold,
                GraphicsUnit.Pixel);
            ErrorProvider.SetError(SaveFontTypeButton, "數值被修改,請記得點選保存");
        }

        private void SaveFontTypeButton_Click(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), NewFontTypeComboBox.SelectedIndex).ToString();
            string TypeName = NewFontSelectComboBox.SelectedItem.ToString();
            FontControl.SetFontType(FontName, TypeName);
            ErrorProvider.SetError(SaveFontTypeButton, "");
        }

        private void NewBackupFontButton_Click(object sender, EventArgs e)
        {
            string FontName;
            FontData FontData;
            if (MessageBox.Show("是否啟動字型還原?", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                for (int i = 0; i < NewFontTypeComboBox.Items.Count; i++)
                {
                    FontName = Enum.ToObject(typeof(FontNameHandle), i).ToString();
                    FontData = FontControl.GetFont(FontName);
                    FontControl.SetFontType(FontName, "Regular");
                }
                MessageBox.Show("已還原.");
                NewFontTypeComboBox.SelectedIndex = 1;
                NewFontTypeComboBox.SelectedIndex = 0;
            }
        }

        private void FontSettingBackupButton_Click(object sender, EventArgs e)
        {
            FileInfo File;
            if (MessageBox.Show("是否啟動浮動設定還原?", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File = new FileInfo("GamePermanent_zh_TW.cfg");
                File.CopyTo(RootPath + "\\Game\\DATA\\CFG\\defaults\\GamePermanent_zh_TW.cfg", true);

                MessageBox.Show("已還原.");
                FontSettingComboBox.SelectedIndex = 0;
                FontSettingComboBox.SelectedIndex = 1;
            }
        }
        private void FontSettingChange(object sender, EventArgs e)
        {
            ErrorProvider.SetError(SaveFontSettingButton, "數值被修改,請記得點選保存");
        }

        private void SaveFontSettingButton_Click(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), FontSettingComboBox.SelectedIndex).ToString();
            FontSetting FontSetting = new FontSetting();

            FontSetting.EnableList[0] = MinYVelocityTextBox.Enabled;
            FontSetting.EnableList[1] = MaxYVelocityTextBox.Enabled;
            FontSetting.EnableList[2] = ContinualForceYTextBox.Enabled;
            FontSetting.EnableList[3] = DecayTextBox.Enabled;
            FontSetting.EnableList[4] = ShrinkTimeTextBox.Enabled;
            FontSetting.EnableList[5] = ShrinkScaleTextBox.Enabled;

            FontSetting.MinYVelocity = double.Parse(MinYVelocityTextBox.Text);
            FontSetting.MaxYVelocity = double.Parse(MaxYVelocityTextBox.Text);
            FontSetting.ContinualForceY = double.Parse(ContinualForceYTextBox.Text);
            FontSetting.Decay = double.Parse(DecayTextBox.Text);
            FontSetting.ShrinkTime = double.Parse(ShrinkTimeTextBox.Text);
            FontSetting.ShrinkScale = double.Parse(ShrinkScaleTextBox.Text);

            FontControl.SetFontSetting(FontName, FontSetting);
            ErrorProvider.SetError(SaveFontSettingButton, "");
        }

        private void FontSettingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(FontNameHandle), FontSettingComboBox.SelectedIndex).ToString();
            FontSetting FontSetting = FontControl.GetFontSetting(FontName);

            MinYVelocityTextBox.Enabled = FontSetting.EnableList[0];
            MaxYVelocityTextBox.Enabled = FontSetting.EnableList[1];
            ContinualForceYTextBox.Enabled = FontSetting.EnableList[2];
            DecayTextBox.Enabled = FontSetting.EnableList[3];
            ShrinkTimeTextBox.Enabled = FontSetting.EnableList[4];
            ShrinkScaleTextBox.Enabled = FontSetting.EnableList[5];

            MinYVelocityTextBox.Text = FontSetting.MinYVelocity.ToString();
            MaxYVelocityTextBox.Text = FontSetting.MaxYVelocity.ToString();
            ContinualForceYTextBox.Text = FontSetting.ContinualForceY.ToString();
            DecayTextBox.Text = FontSetting.Decay.ToString();
            ShrinkTimeTextBox.Text = FontSetting.ShrinkTime.ToString();
            ShrinkScaleTextBox.Text = FontSetting.ShrinkScale.ToString();

            ErrorProvider.SetError(SaveFontSettingButton, "");
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("匯出會將之前匯出的檔案覆蓋! \n 是否將目前設定匯出? ", "匯出系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Export.Start(RootPath + "\\Game\\DATA\\");
                UpdateCheck.SetSaveVersion();
                MessageBox.Show("以匯出.");
            }
        }
        private void Import() 
        {
            if (File.Exists("backup.zip"))
            {
                UpdateCheck.SetSaveVersion();

                FontControl.BackupFont();
                System.Diagnostics.ProcessStartInfo StartInfo = new System.Diagnostics.ProcessStartInfo();
                StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                StartInfo.Verb = "runas";
                StartInfo.FileName = "LoLMConsole.exe";
                StartInfo.Arguments = "ImportData \"" + RootPath + "\"";
                System.Diagnostics.Process.Start(StartInfo);

                this.Close();
                Environment.Exit(Environment.ExitCode);
                InitializeComponent();
            }
            else
            {
                MessageBox.Show("找不到先前匯出的資訊\n請確認LoLManager資料夾底下有backup.zip", "匯入系統");
            }
        }
        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("匯入將會覆蓋目前的設定值\n是否匯入先前匯出的版本?", "匯入系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Import();
            }
        }

        public void UnPack()
        {
            FontControl.Release();

            BackupEx.UnPackOriginal(RootPath, UpdateCheck.GetLoLVersion().ToString());

            this.Close();
            Environment.Exit(Environment.ExitCode);
            InitializeComponent();
        }

        public void UnPackLatest()
        {
            FontControl.Release();

            BackupEx.UnPackLatest(RootPath, UpdateCheck.GetLoLVersion().ToString());

            this.Close();
            Environment.Exit(Environment.ExitCode);
            InitializeComponent();
        }

        private void CheckVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ManagerINI.SetValue("Option", "CheckSaveVersion", CheckVersionCheckBox.Checked ? "1" : "0");
        }

        private void LinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        private void SaveChatSettingButton_Click(object sender, EventArgs e)
        {
            string FontName = Enum.ToObject(typeof(ChatTypeNameHandle), ChatSettingComboBox.SelectedIndex).ToString();
            ChatControl.SetChatSize(FontName, (int)ChatSettingBorderLabel.Font.Size, (int)ChatSettingBorderLabel.BorderSize, ChatSettingBorderLabel.GlowSize);
            ChatControl.SetChatColorBold(FontName, ChatSettingBorderLabel.ForeColor, ChatSettingBorderLabel.Font.Bold, ChatSettingBorderLabel.BorderColor, ChatSettingBorderLabel.BackColor);
            ChatControl.SetFontName(FontName, ChatSettingFontTypeComboBox.SelectedItem.ToString());
            ErrorProvider.SetError(SaveChatSettingButton, "");
        }

        private void ChatSettingBackuputton_Click(object sender, EventArgs e)
        {
            FileInfo File;
            if (MessageBox.Show("是否啟動文字設定還原?", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File = new FileInfo("GamePermanent_zh_TW.cfg");
                File.CopyTo(RootPath + "\\Game\\DATA\\CFG\\defaults\\GamePermanent_zh_TW.cfg", true);

                MessageBox.Show("已還原.");
                ChatSettingComboBox.SelectedIndex = 1;
                ChatSettingComboBox.SelectedIndex = 0;
            }
        }

        private void ChatSetting_TextChanged(object sender, EventArgs e)
        {
            int Result;
            if (Int32.TryParse(ChatSettingFontSizeTextBox.Text, out Result)
                && Int32.TryParse(ChatSettingOutlineSizeTextBox.Text, out Result)
                && Int32.TryParse(ChatSettingGlowSizeTextBox.Text, out Result))
            {
                int Size = Int32.Parse(ChatSettingFontSizeTextBox.Text);
                int OutlineSize = Int32.Parse(ChatSettingOutlineSizeTextBox.Text);
                int GlowSize = Int32.Parse(ChatSettingGlowSizeTextBox.Text);

                string FontName = ChatSettingFontTypeComboBox.SelectedItem.ToString();
                if (FontName == "FZXHYSZK") FontName = "Regular";

                FontControl.GetFontList();
                ChatSettingBorderLabel.Font = new Font(
                    FontControl.FontMap[FontName].Families[0],
                    (float)(Size),
                    ChatSettingBoldCheckBox.Checked ? FontStyle.Bold : FontStyle.Regular,
                    GraphicsUnit.Pixel);
                ChatSettingBorderLabel.BorderSize = (float)OutlineSize;
                ChatSettingBorderLabel.GlowSize = GlowSize;

                ErrorProvider.SetError(SaveChatSettingButton, "數值被修改,請記得點選保存");
            }
        }

        private void ChatSettingFontColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ChatSettingBorderLabel.ForeColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                ChatSettingBorderLabel.ForeColor = Color.FromArgb(
                    255,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                ChatSettingFontColorLabel.Text = ChatSettingBorderLabel.ForeColor.ToString().Substring(6);
                ErrorProvider.SetError(SaveChatSettingButton, "數值被修改,請記得點選保存");
            }
        }

        private void ChatSettingOutlineColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ChatSettingBorderLabel.BorderColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                ChatSettingBorderLabel.BorderColor = Color.FromArgb(
                    255,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                ChatSettingOutlineColorLabel.Text = ChatSettingBorderLabel.BorderColor.ToString().Substring(6);
                ErrorProvider.SetError(SaveChatSettingButton, "數值被修改,請記得點選保存");
            }
        }

        private void ChatSettingGlowColorButton_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ChatSettingBorderLabel.BackColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                ChatSettingBorderLabel.BackColor = Color.FromArgb(
                    255,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                ChatSettingGlowColorLabel.Text = ChatSettingBorderLabel.BackColor.ToString().Substring(6);
                ErrorProvider.SetError(SaveChatSettingButton, "數值被修改,請記得點選保存");
            }
        }

        private void ChatSettingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Name = Enum.ToObject(typeof(ChatTypeNameHandle), ChatSettingComboBox.SelectedIndex).ToString();

            FontControl.GetFontList();

            string FontName = ChatControl.GetData(Name, "FontName");
            if (!ChatSettingFontTypeComboBox.Items.Contains(FontName))
            {
                MessageBox.Show("字型 " + FontName + " 沒有對應在FontTypes.xml內\n已將本樣板字型初始化為 FZXHYSZK\n如需使用該字型請再匯入一次", "警告");
                FontName = "FZXHYSZK";
                ChatControl.SetFontName(Name, "FZXHYSZK"); 
            }
            ChatSettingFontTypeComboBox.SelectedIndex = ChatSettingFontTypeComboBox.FindString(FontName);
            if (FontName == "FZXHYSZK") FontName = "Regular";

            ChatSettingFontSizeTextBox.Text = ChatControl.GetData(Name, "FontSize");
            ChatSettingOutlineSizeTextBox.Text = ChatControl.GetData(Name, "OutlineSize");
            ChatSettingGlowSizeTextBox.Text = ChatControl.GetData(Name, "Glow");

            ChatSettingBorderLabel.Font = new Font(
                FontControl.FontMap[FontName].Families[0],
                (float)Int32.Parse(ChatSettingFontSizeTextBox.Text),
                ChatControl.GetData(Name, "Bold") == "1" ? FontStyle.Bold : FontStyle.Regular,
                GraphicsUnit.Pixel);
            ChatSettingBorderLabel.BorderSize = Int32.Parse(ChatSettingOutlineSizeTextBox.Text);
            ChatSettingBorderLabel.GlowSize = Int32.Parse(ChatSettingGlowSizeTextBox.Text);

            ChatSettingBorderLabel.ForeColor = ChatControl.StringToColor(ChatControl.GetData(Name, "Color"));
            ChatSettingBorderLabel.BorderColor = ChatControl.StringToColor(ChatControl.GetData(Name, "OutlineColor"));
            ChatSettingBorderLabel.BackColor = ChatControl.StringToColor(ChatControl.GetData(Name, "GlowColor"));

            ChatSettingFontColorLabel.Text = ChatSettingBorderLabel.ForeColor.ToString().Substring(6);
            ChatSettingOutlineColorLabel.Text = ChatSettingBorderLabel.BorderColor.ToString().Substring(6);
            ChatSettingGlowColorLabel.Text = ChatSettingBorderLabel.BackColor.ToString().Substring(6);

            ChatSettingBoldCheckBox.Checked = ChatControl.GetData(Name, "Bold") == "1";

            ErrorProvider.SetError(SaveChatSettingButton, "");
        }

        private void HealthBarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Name = Enum.ToObject(typeof(GeneralCharacterDataTagNameHandle), HealthBarComboBox.SelectedIndex).ToString();
            /*HealthBarColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData(Name, "HealthBarColor"));
            HealthFadeColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData(Name, "HealthFadeColor"));
            AllShieldColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData(Name, "AllShieldColor"));
            PhysShieldColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData(Name, "PhysShieldColor"));
            MagicShieldColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData(Name, "MagicShieldColor"));

            HealthBarColorLabel.Text = HealthBarColorPictureBox.BackColor.ToString().Substring(6);
            HealthFadeColorLabel.Text = HealthFadeColorPictureBox.BackColor.ToString().Substring(6);
            AllShieldColorLabel.Text = AllShieldColorPictureBox.BackColor.ToString().Substring(6);
            PhysShieldColorLabel.Text = PhysShieldColorPictureBox.BackColor.ToString().Substring(6);
            MagicShieldColorLabel.Text = MagicShieldColorPictureBox.BackColor.ToString().Substring(6);*/

            ErrorProvider.SetError(HealthBarSaveButton, "");
        }

        private void HealthBarSaveButton_Click(object sender, EventArgs e)
        {
            string Name = Enum.ToObject(typeof(GeneralCharacterDataTagNameHandle), HealthBarComboBox.SelectedIndex).ToString();

            /*BarSetting.SetHealthBar(
                Name,
                HealthBarColorPictureBox.BackColor,
                HealthFadeColorPictureBox.BackColor,
                AllShieldColorPictureBox.BackColor,
                PhysShieldColorPictureBox.BackColor,
                MagicShieldColorPictureBox.BackColor);
            */
            ErrorProvider.SetError(HealthBarSaveButton, "");
        }

        private void HealthBarColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(HealthBarColorPictureBox, HealthBarColorLabel, HealthBarSaveButton);
        }

        private void HealthFadeColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(HealthFadeColorPictureBox, HealthFadeColorLabel, HealthBarSaveButton);
        }

        private void AllShieldColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(AllShieldColorPictureBox, AllShieldColorLabel, HealthBarSaveButton);
        }

        private void PhysShieldColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(PhysShieldColorPictureBox, PhysShieldColorLabel, HealthBarSaveButton);
        }

        private void MagicShieldColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(MagicShieldColorPictureBox, MagicShieldColorLabel, HealthBarSaveButton);
        }

        private void MpBarLoag()
        {/*
            MPBarColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "MPBarColor"));
            EnergyBarColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "EnergyBarColor"));
            ShieldBarColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "ShieldBarColor"));
            OtherBarColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "OtherBarColor"));

            MPFadeColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "MPFadeColor"));
            EnergyFadeColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "EnergyFadeColor"));
            ShieldFadeColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "ShieldFadeColor"));
            OtherFadeColorPictureBox.BackColor = BarSetting.StringToColor(BarSetting.GetData("GeneralDataHero", "OtherFadeColor"));

            MPBarColorLabel.Text = MPBarColorPictureBox.BackColor.ToString().Substring(6);
            EnergyBarColorLabel.Text = EnergyBarColorPictureBox.BackColor.ToString().Substring(6);
            ShieldBarColorLabel.Text = ShieldBarColorPictureBox.BackColor.ToString().Substring(6);
            OtherBarColorLabel.Text = OtherBarColorPictureBox.BackColor.ToString().Substring(6);

            MPFadeColorLabel.Text = MPFadeColorPictureBox.BackColor.ToString().Substring(6);
            EnergyFadeColorLabel.Text = EnergyFadeColorPictureBox.BackColor.ToString().Substring(6);
            ShieldFadeColorLabel.Text = ShieldFadeColorPictureBox.BackColor.ToString().Substring(6);
            OtherFadeColorLabel.Text = OtherFadeColorPictureBox.BackColor.ToString().Substring(6);
            */
            ErrorProvider.SetError(HealthBarSaveButton, "");
        }

        public void ChangePictureBoxBackColor(PictureBox PictureBox, Label Label, Button Button) 
        {
            ColorDialog.Color = PictureBox.BackColor;
            ColorDialog.FullOpen = true;
            if (ColorDialog.ShowDialog() != DialogResult.Cancel)
            {
                PictureBox.BackColor = Color.FromArgb(
                    255,
                    ColorDialog.Color.R,
                    ColorDialog.Color.G,
                    ColorDialog.Color.B);
                Label.Text = PictureBox.BackColor.ToString().Substring(6);
                ErrorProvider.SetError(Button, "數值被修改,請記得點選保存");
            }
        }

        private void MPBarSaveButton_Click(object sender, EventArgs e)
        {/*
            BarSetting.SetBar(
                MPBarColorPictureBox.BackColor,
                EnergyBarColorPictureBox.BackColor,
                ShieldBarColorPictureBox.BackColor,
                OtherBarColorPictureBox.BackColor,
                MPFadeColorPictureBox.BackColor,
                EnergyFadeColorPictureBox.BackColor,
                ShieldFadeColorPictureBox.BackColor,
                OtherFadeColorPictureBox.BackColor);
            */
            ErrorProvider.SetError(MPBarSaveButton, "");
        }

        private void MPBarColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(MPBarColorPictureBox, MPBarColorLabel, MPBarSaveButton);
        }

        private void EnergyBarColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(EnergyBarColorPictureBox, EnergyBarColorLabel, MPBarSaveButton);
        }

        private void ShieldBarColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(ShieldBarColorPictureBox, ShieldBarColorLabel, MPBarSaveButton);
        }

        private void OtherBarColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(OtherBarColorPictureBox, OtherBarColorLabel, MPBarSaveButton);
        }

        private void MPFadeColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(MPFadeColorPictureBox, MPFadeColorLabel, MPBarSaveButton);
        }

        private void EnergyFadeColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(EnergyFadeColorPictureBox, EnergyFadeColorLabel, MPBarSaveButton);
        }

        private void ShieldFadeColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(ShieldFadeColorPictureBox, ShieldFadeColorLabel, MPBarSaveButton);
        }

        private void OtherFadeColorButton_Click(object sender, EventArgs e)
        {
            ChangePictureBoxBackColor(OtherFadeColorPictureBox, OtherFadeColorLabel, MPBarSaveButton);
        }

        private void BarBackupButton_Click(object sender, EventArgs e)
        {/*
            FileInfo File;
            if (MessageBox.Show("是否啟動血魔條設定還原?", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                File = new FileInfo("GeneralCharacterData_Legacy.ini");
                File.CopyTo(RootPath + "\\Game\\DATA\\Menu\\HUD\\GeneralCharacterData_Legacy.ini", true);

                MessageBox.Show("已還原.");
                MpBarLoag();
                HealthBarComboBox.SelectedIndex = 1;
                HealthBarComboBox.SelectedIndex = 0;
            }*/
        }

        private void UnPackLatestButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("本系統將會把所有檔案更新到最新\n但是最新不一定是官方版本\n有可能因版號對不上導致遊戲崩潰\n確定要進行最新化還原嗎?\n\n*還原之後將不自動啟動LoLManager\n以確保還原完整.", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                UnPackLatest();
                MessageBox.Show("已還原.");
            }
        }

        private void BackupExButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("本系統將會把所有檔案還原到官方最新版\n確定執行嗎?\n\n*還原之後將不自動啟動LoLManager\n以確保還原完整.", "還原系統", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                UnPack();
                MessageBox.Show("已還原.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
