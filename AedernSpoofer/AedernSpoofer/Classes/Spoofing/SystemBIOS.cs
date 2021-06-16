using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes.Spoofing
{
    class SystemBIOS
    {
        private string SerialId = string.Empty;
        public SystemBIOS()
        {
            SerialId = Utils.RandomString(Utils.RandomInt(6, 12));
            Spoof();
        }

        public void DownloadRequirments()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            webClient.Proxy = null;
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854452065764900894/AMIDEWINx64.exe", "C:\\Windows\\AMIDEWINx64.exe");
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854452061299146792/amide.sys", "C:\\Windows\\amide.sys");
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854452063706808390/AMIDEWIN.EXE", "C:\\Windows\\AMIDEWIN.exe");
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854452067434889266/amifldrv64.sys", "C:\\Windows\\amifldrv64.sys");
        }

        public void DeleteRequirments()
        {
            if (File.Exists("C:\\Windows\\AMIDEWINx64.exe"))
            {
                File.Delete("C:\\Windows\\AMIDEWINx64.exe");
            }
            if (File.Exists("C:\\Windows\\amide.sys"))
            {
                File.Delete("C:\\Windows\\amide.sys");
            }
            if (File.Exists("C:\\Windows\\AMIDEWIN.exe"))
            {
                File.Delete("C:\\Windows\\AMIDEWIN.exe");
            }
            if (File.Exists("C:\\Windows\\amifldrv64.sys"))
            {
                File.Delete("C:\\Windows\\amifldrv64.sys");
            }
        }

        public void Spoof() // Needs Restart
        {
            DownloadRequirments();

            #region BIOS Spoof
            Process biosSpoof = new Process();
            biosSpoof.StartInfo.RedirectStandardInput = true;
            biosSpoof.StartInfo.UseShellExecute = false;
            biosSpoof.StartInfo.CreateNoWindow = true;
            biosSpoof.StartInfo.FileName = "cmd.exe";
            biosSpoof.Start();
            biosSpoof.StandardInput.WriteLine("cd C:\\Windows");
            biosSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /BS " + SerialId);
            biosSpoof.StandardInput.WriteLine("exit");
            biosSpoof.WaitForExit();
            #endregion

            Utils.ExecuteCommand("net stop IPHLPSVC");
            Utils.ExecuteCommand("net stop winmgmt");
            Utils.ExecuteCommand("net start winmgmt");
            Utils.ExecuteCommand("sc stop winmgmt");
            Utils.ExecuteCommand("sc start winmgmt");
            Utils.ExecuteCommand("net start IPHLPSVC");

            #region BIOS Id Spoof
            Process idSpoof = new Process();
            idSpoof.StartInfo.RedirectStandardInput = true;
            idSpoof.StartInfo.UseShellExecute = false;
            idSpoof.StartInfo.CreateNoWindow = true;
            idSpoof.StartInfo.FileName = "cmd.exe";
            idSpoof.Start();
            idSpoof.StandardInput.WriteLine("cd C:\\Windows");
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /BS " + SerialId);
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /SS " + SerialId);
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /SU auto");
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /SK " + SerialId);
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /SF " + SerialId);
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /CS " + SerialId);
            idSpoof.StandardInput.WriteLine("AMIDEWINx64.exe /PSN " + SerialId);
            idSpoof.StandardInput.WriteLine("exit");
            idSpoof.WaitForExit();
            #endregion

            DeleteRequirments();

            Process.Start("shutdown.exe", "-r -t 15");
        }
    }
}
