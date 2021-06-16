using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes.Spoofing.Drive
{
    class VolumeId
    {
        public static void SpoofDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    ChangeSerialNumber(d.Name[0], Convert.ToUInt32(Utils.RandomNumber(6)));
                }
            }
        }

        static void ChangeSerialNumber(char volume, uint newSerial)
        {
            var fsInfo = new[]
            {
                new { Name = "FAT32", NameOffs = 0x52, SerialOffs = 0x43 },
                new { Name = "FAT", NameOffs = 0x36, SerialOffs = 0x27 },
                new { Name = "NTFS", NameOffs = 0x03, SerialOffs = 0x48 }
            };

            using (var disk = new Disk(volume))
            {
                var sector = new byte[512];
                disk.ReadSector(0, sector);

                var fs = fsInfo.FirstOrDefault(
                        f => Utils.Strncmp(f.Name, sector, f.NameOffs)
                    );
                if (fs == null) throw new NotSupportedException("This file system is not supported");

                var s = newSerial;
                for (int i = 0; i < 4; ++i, s >>= 8) sector[fs.SerialOffs + i] = (byte)(s & 0xFF);

                disk.WriteSector(0, sector);
            }
        }
    }
}
