using AedernSpoofer.Classes;
using AedernSpoofer.Classes.Spoofing;
using AedernSpoofer.Classes.Spoofing.Drive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AedernSpoofer
{
    class Program
    {
        private static readonly string[] blacklist = new string[] { "VolumeID", "Load", "LSpoofer", "FACEIT", "Origin", "Battle.net", "Steam", "vgc", "OriginWebHelperService", "RiotClientServices", "Agent" };

        static void Main(string[] args)
        {
            Console.Title = "Aedern Spoofer";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" - AEDERN SPOOFER ->");
            Console.WriteLine(" - 1) Spoof Hardware");
            Console.WriteLine(" - 2) Close");
            Console.WriteLine(" - Your Option : ");
            int option = int.Parse(Console.ReadLine());
            Console.Clear();

            if(option == 1)
            {
                #region Kill blacklisted programs
                foreach (Process process in Process.GetProcesses())
                {
                    for (int i = 0; i < blacklist.Count(); i++)
                    {
                        if (process.ProcessName == blacklist[i])
                        {
                            process.Kill();
                        }
                    }
                }
                #endregion

                #region Trace Files
                File.WriteAllText("vanguard.bat", Properties.Resources.vanguard_tracking_files);
                Process vanguard = new Process();
                vanguard.StartInfo.RedirectStandardInput = true;
                vanguard.StartInfo.UseShellExecute = false;
                vanguard.StartInfo.CreateNoWindow = true;
                vanguard.StartInfo.Arguments = "runas";
                vanguard.StartInfo.FileName = "vanguard.bat";
                vanguard.Start();
                Console.WriteLine("Trace files removing!");
                new Traces();
                #endregion

                VolumeId.SpoofDrives();
                Console.WriteLine("Volume Id - Spoofed!");
                Hostname.SetMachineName(string.Format("DESKTOP-{0}", Utils.RandomString(6)));
                Console.WriteLine("Host Name - Spoofed!");
                foreach (string deviceId in MAC.GetDeviceIDs())
                {
                    new MAC(deviceId).Spoof();
                }
                Console.WriteLine("Mac Address - Spoofed!");
                new NtwSpoof();

                vanguard.WaitForExit();
                File.Delete("vanguard.bat");
                Console.WriteLine("Valorant - Trace files succesfully removed!");
                MessageBox.Show("This process requires a reboot.", "Aedern Spoofer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                new SystemBIOS();

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The reboot process has been started.");
            }
            else if (option == 2)
            {
                Environment.Exit(0);
            }
        }
    }
}
