using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace LoLMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo File;
            System.Diagnostics.ProcessStartInfo StartInfo;
            System.Diagnostics.Process Process;
            FileStream FileStream;
            BinaryReader BinaryReader;
            BinaryWriter BinaryWriter;
            if (args.Count() > 0)
            {
                Console.Write("Loading LoL Manager Console.");
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
                System.Threading.Thread.Sleep(500);
                Console.Write(".\n\n");
                System.Threading.Thread.Sleep(500);

                switch(args[0])
                {
                    case "UnPack":
                        Console.WriteLine("### 匯入開始 ###");
                        Console.WriteLine("");

                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "7z.exe";
                        StartInfo.Arguments = "x Original-v" + args[1] + ".zip -y -o\"" + args[2] + "\\Game\\DATA\\\"";
                        Process = System.Diagnostics.Process.Start(StartInfo);
                        Process.WaitForExit();

                        Console.WriteLine("### 匯入完成 ###");
                        Console.WriteLine("");
                        break;
                    case "BackupFont":
                        Console.WriteLine("### 還原開始 ###");
                        Console.WriteLine("");
                        File = new FileInfo("FZLTCH.TTF");
                        File.CopyTo(args[1] + "\\Game\\DATA\\Fonts\\FZLTCH.TTF", true);
                        Console.WriteLine("Backup file: FZLTCH.TTF => " + args[1] + @"\Game\DATA\Fonts\FZLTCH.TTF");

                        File = new FileInfo("FZXHYSZK.TTF");
                        File.CopyTo(args[1] + "\\Game\\DATA\\Fonts\\FZXHYSZK.TTF", true);
                        Console.WriteLine("Backup file: FZXHYSZK.TTF => " + args[1] + @"\Game\DATA\Fonts\FZXHYSZK.TTF");
                        Console.WriteLine("");
                        Console.WriteLine("### 還原完成 ###");
                        Console.WriteLine("");
                        Console.WriteLine("重新啟動LoLManager...");
                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "LoLManager.exe";
                        System.Diagnostics.Process.Start(StartInfo);
                        break;
                    case "ImportData":
                        Console.WriteLine("### 匯入開始 ###");
                        Console.WriteLine("");

                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "7z.exe";
                        StartInfo.Arguments = "x backup.zip -y -o\"" + args[1] + "\\Game\\DATA\\\"";
                        Process = System.Diagnostics.Process.Start(StartInfo);
                        Process.WaitForExit();

                        Console.WriteLine("### 匯入完成 ###");
                        Console.WriteLine("");
                        Console.WriteLine("重新啟動LoLManager...");
                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "LoLManager.exe";
                        System.Diagnostics.Process.Start(StartInfo);
                        break;
                    case "AddFontToGFX":
                        byte[] TempData;
                        int Lenght;
                        //Console.WriteLine("Transform fonts_tw.gfx to swf file ...\n");
                        //File = new FileInfo(args[1] + "\\Game\\DATA\\Menu\\fonts_tw.gfx");
                        //File.CopyTo("temp.swf", true);
                        //
                        //mpData = new byte[536870912];
                        //FileStream = new FileStream("temp.swf", FileMode.Open);
                        //BinaryReader = new BinaryReader(FileStream);
                        //Lenght = BinaryReader.Read(TempData, 0, 536870912);
                        //BinaryReader.Close();
                        //FileStream.Close();
                        //
                        //TempData[0] = (byte)'C';
                        //TempData[1] = (byte)'W';
                        //TempData[2] = (byte)'S';
                        //
                        //FileStream = new FileStream("temp.swf", FileMode.Create);
                        //BinaryWriter = new BinaryWriter(FileStream);
                        //BinaryWriter.Write(TempData, 0, Lenght);
                        //BinaryWriter.Close();
                        //FileStream.Close();
                        //
                        //Console.WriteLine("Transform swf to xml file ...\n");
                        //
                        //StartInfo = new System.Diagnostics.ProcessStartInfo();
                        //StartInfo.Verb = "runas";
                        //StartInfo.FileName = "SwiXConsole.exe";
                        //StartInfo.Arguments = "-i temp.swf";
                        //Process = System.Diagnostics.Process.Start(StartInfo);
                        //Process.WaitForExit();

                        Console.WriteLine("Transform font to swf file ...\n");

                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "font2swf.exe";
                        StartInfo.Arguments = args[2];

                        Process = System.Diagnostics.Process.Start(StartInfo);
                        Process.WaitForExit();

                        Console.WriteLine("Decode font ...\n");

                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "SwiXConsole.exe";
                        StartInfo.Arguments = "-i output.swf";
                        Process = System.Diagnostics.Process.Start(StartInfo);
                        Process.WaitForExit();

                        Console.WriteLine("Combine xml file ...\n");

                        StreamReader mainInput = new StreamReader("output.xml");
                        StreamReader topInput = new StreamReader("fonts_tw_top.xml");
                        StreamReader downInput = new StreamReader("fonts_tw_down.xml");
                        StreamWriter output = new StreamWriter("temp.xml");

                        output.Write(topInput.ReadToEnd());

                        string Data;
                        bool start_flag = false;
                        bool end_flag = false;
                        do
                        {
                            Data = mainInput.ReadLine();
                            
                            if(start_flag && !end_flag)
                            {
                                if (Data.IndexOf("DefineFont3") > 0)
                                {
                                    output.WriteLine(Data);
                                    end_flag = true;
                                }
                                else 
                                {
                                    output.WriteLine(Data);
                                }
                            }

                            if (!start_flag)
                            {
                                if (Data.IndexOf("DefineFont3") > 0)
                                {
                                    
                                    output.WriteLine("<DefineFont3 FontId=\"1\" LanguageCode=\"4\" FontName=\"FZXHYSZK\" " + Data.Substring(Data.IndexOf("Ascent")));
                                    
                                    start_flag = true;
                                }
                            }
                        }
                        while (!mainInput.EndOfStream);

                        output.Write(downInput.ReadToEnd());

                        mainInput.Close();
                        topInput.Close();
                        downInput.Close();
                        output.Close();


                        Console.WriteLine("Transform temp.xml to temp.swf ...\n");

                        StartInfo = new System.Diagnostics.ProcessStartInfo();
                        StartInfo.WorkingDirectory = System.Environment.CurrentDirectory;
                        StartInfo.Verb = "runas";
                        StartInfo.FileName = "SwiXConsole.exe";
                        StartInfo.Arguments = "-i temp.xml";
                        Process = System.Diagnostics.Process.Start(StartInfo);
                        Process.WaitForExit();

                        Console.WriteLine("Transform swf to gfx ...\n");

                        //TempData = new byte[536870912];
                        //FileStream = new FileStream("temp.swf", FileMode.Open);
                        //BinaryReader = new BinaryReader(FileStream);
                        //Lenght = BinaryReader.Read(TempData, 0, 536870912);
                        //BinaryReader.Close();
                        //FileStream.Close();
                        //
                        //TempData[0] = (byte)'C';
                        //TempData[1] = (byte)'F';
                        //TempData[2] = (byte)'X';
                        //
                        //FileStream = new FileStream("temp.swf", FileMode.Create);
                        //BinaryWriter = new BinaryWriter(FileStream);
                        //BinaryWriter.Write(TempData, 0, Lenght);
                        //BinaryWriter.Close();
                        //FileStream.Close();

                        Console.WriteLine("Copy fonts_tw.gfx to " + args[1] + "\\Game\\DATA\\Menu\\ ...\n");
                        File = new FileInfo("temp.swf");
                        File.CopyTo(args[1] + "\\Game\\DATA\\Menu\\fonts_tw.swf", true);

                        Console.WriteLine("Delete temp file...\n");

                        File = new FileInfo("temp.swf");
                        File.Delete();
                        File = new FileInfo("output.swf");
                        File.Delete();
                        File = new FileInfo("temp.xml");
                        File.Delete();
                        File = new FileInfo("input.swf");
                        File.Delete();
                        File = new FileInfo("output.xml");
                        File.Delete();

                        break;
                }
            }
            else
            {
                Console.WriteLine("LoL Manager Console無法獨自執行.");
            }
            Console.WriteLine("");
            Console.Write("2秒後離開本系統");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
        }
    }
}
