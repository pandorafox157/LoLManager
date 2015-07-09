using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace LoLManager
{
    public class UpdateCheck
    {
        string Path;
        CFGFile ManagerINI;
        public UpdateCheck(string _Path) 
        {
            Path = _Path;
            ManagerINI = new CFGFile(Directory.GetCurrentDirectory() + "\\LoLManager.ini");
        }
        public void SetSaveVersion()
        {
            StreamReader StreamReader = new StreamReader(Path + "\\lol.version");
            int LoLVersion = Int32.Parse(StreamReader.ReadToEnd());
            ManagerINI.SetValue("Version", "Save", LoLVersion.ToString());
            StreamReader.Close();
        }
        public void SetMainVersion()
        {
            StreamReader StreamReader = new StreamReader(Path + "\\lol.version");
            int LoLVersion = Int32.Parse(StreamReader.ReadToEnd());
            ManagerINI.SetValue("Version", "Main", LoLVersion.ToString());
            StreamReader.Close();
        }
        public int GetSaveVersion()
        {
            return Int32.Parse(ManagerINI.GetValue("Version", "Save"));
        }
        public int GetLoLVersion()
        {
            StreamReader StreamReader = new StreamReader(Path + "\\lol.version");
            int LoLVersion = Int32.Parse(StreamReader.ReadToEnd());
            StreamReader.Close();
            return LoLVersion; 
        }
        public bool CheckBackupExVersion() 
        {
            StreamReader StreamReader = new StreamReader(Path + "\\lol.version");

            int LoLVersion = Int32.Parse(StreamReader.ReadToEnd());
            int MainVersion = Int32.Parse(ManagerINI.GetValue("Version" ,"Main"));
            StreamReader.Close();
            if (LoLVersion > MainVersion)
            {
                return true;
            }
            return false;
        }
        public bool CheakLoLVersion() 
        {
            StreamReader StreamReader = new StreamReader(Path + "\\lol.version");

            int LoLVersion = Int32.Parse(StreamReader.ReadToEnd());
            int SaveVersion = Int32.Parse(ManagerINI.GetValue("Version" ,"Save"));
            StreamReader.Close();
            if (Int32.Parse(ManagerINI.GetValue("Option", "CheckSaveVersion")) == 1)
            {
                if (LoLVersion > SaveVersion && SaveVersion != 0)
                {
                    return true;
                }
            }
            return false;   
        }
    }
}
