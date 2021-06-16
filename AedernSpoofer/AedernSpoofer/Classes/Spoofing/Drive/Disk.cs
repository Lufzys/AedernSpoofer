using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes.Spoofing.Drive
{
    class Disk : IDisposable
    {
        private SafeFileHandle handle;

        public Disk(char volume)
        {
            var ptr = CreateFile(
                String.Format("\\\\.\\{0}:", volume),
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                IntPtr.Zero,
                FileMode.Open,
                0,
                IntPtr.Zero
                );

            handle = new SafeFileHandle(ptr, true);

            if (handle.IsInvalid) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        public void ReadSector(uint sector, byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (SetFilePointer(handle, sector, IntPtr.Zero, EMoveMethod.Begin) == INVALID_SET_FILE_POINTER) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            uint read;
            if (!ReadFile(handle, buffer, buffer.Length, out read, IntPtr.Zero)) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            if (read != buffer.Length) throw new IOException();
        }

        public void WriteSector(uint sector, byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (SetFilePointer(handle, sector, IntPtr.Zero, EMoveMethod.Begin) == INVALID_SET_FILE_POINTER) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());

            uint written;
            if (!WriteFile(handle, buffer, buffer.Length, out written, IntPtr.Zero)) Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            if (written != buffer.Length) throw new IOException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (handle != null) handle.Dispose();
            }
        }

        enum EMoveMethod : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        const uint INVALID_SET_FILE_POINTER = 0xFFFFFFFF;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            int flags,
            IntPtr template);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint SetFilePointer(
             [In] SafeFileHandle hFile,
             [In] uint lDistanceToMove,
             [In] IntPtr lpDistanceToMoveHigh,
             [In] EMoveMethod dwMoveMethod);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadFile(SafeFileHandle hFile, [Out] byte[] lpBuffer,
            int nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);

        [DllImport("kernel32.dll")]
        static extern bool WriteFile(SafeFileHandle hFile, [In] byte[] lpBuffer,
            int nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten,
            [In] IntPtr lpOverlapped);
    }
}
