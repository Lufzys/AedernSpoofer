using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes.Spoofing
{
    public class MAC
    {
        static RegistryKey NetworkClass = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\");
        RegistryKey NetworkInterface;
        ManagementObject NetworkAdapter;
        String RegPath = @"Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\";
        String Device;
        String DriverDesc;


        // Generate a Valid Adapter ID
        private static String GenerateID(int i)
        {
            return i.ToString().PadLeft(4, '0');
        }

        // Generate a random MAC address
        public static String GenerateRandomMAC()
        {
            Random r = new Random((int)DateTime.Now.ToFileTimeUtc());
            String abc = "0123456789ABCDEF";
            String MAC = "";
            for (int i = 1; i < 12; i++)
            {
                MAC += abc[r.Next(0, 15)];
            }
            return MAC;
        }

        private bool DisableNetworkDriver()
        {

            try
            {
                if ((uint)NetworkAdapter.InvokeMethod("Disable", null) == 0)
                    return true;
            }
            catch (Exception ex) { }
            return false;
        }
        private bool EnableNetworkDriver()
        {

            try
            {
                if ((uint)NetworkAdapter.InvokeMethod("Enable", null) == 0)
                    return true;
            }
            catch (Exception ex) { }
            return false;
        }
        public static List<String> GetDeviceIDs()
        {
            List<String> IDs = new List<String>();
            for (int i = 0; i <= 9999; i++)
            {
                var ID = GenerateID(i);
                var regKey = NetworkClass.OpenSubKey(ID);
                if (regKey != null)
                    IDs.Add(ID);
                else
                    break;
            }
            return IDs;
        }
        public static String GetDriverDescByID(String id)
        {
            return NetworkClass.OpenSubKey(id).GetValue("DriverDesc").ToString();
        }

        public MAC(String DeviceID)
        {
            DriverDesc = GetDriverDescByID(DeviceID);
            Device = DeviceID;
            NetworkInterface = NetworkClass.OpenSubKey(DeviceID, true);
            NetworkAdapter = new ManagementObjectSearcher("select * from win32_networkadapter where Name='" + DriverDesc + "'").Get().Cast<ManagementObject>().FirstOrDefault();
        }

        public bool Spoof(String MAC)
        {
            if (DisableNetworkDriver())
                return false;
            NetworkInterface.SetValue("NetworkAddress", MAC, RegistryValueKind.String);
            if (EnableNetworkDriver())
                return false;
            return true;
        }

        public bool Spoof()
        {
            if (!DisableNetworkDriver())
                return false;

            NetworkInterface.SetValue("NetworkAddress", GenerateRandomMAC(), RegistryValueKind.String);

            if (!EnableNetworkDriver())
                return false;
            return true;
        }

        public bool Reset()
        {
            if (!DisableNetworkDriver())
                return false;

            NetworkInterface.DeleteValue("NetworkAddress");

            if (!EnableNetworkDriver())
                return false;
            return true;
        }

    }
}
