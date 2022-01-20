using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.IO;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace Silvercrest.Web.Helpers.Files
{
    public static class FileHelper
    {
        public static async Task<string> GetExtensionFromStream(Stream stream)
        {
            var mime = await GetMimeFromStreamAsync(stream);
            RegistryKey key = Registry.ClassesRoot.OpenSubKey($@"MIME\Database\Content Type\{mime}", false);
            
            if (key == null)
            {
                return string.Empty;
            }

            var objectResult = key.GetValue("Extension", null);

            if (objectResult == null)
            {
                return string.Empty;
            }

            return objectResult.ToString();
        }

        private static async Task<string> GetMimeFromStreamAsync(Stream stream)
        {
            stream.Position = 0;
            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);

            try
            {
                IntPtr mimeTypePtr;
                FindMimeFromData(IntPtr.Zero, null, buffer, buffer.Length, null, 0, out mimeTypePtr, 0);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch
            {
                return "unknown/unknown";
            }
        }

        [DllImport(@"Urlmon.dll", CharSet = CharSet.Auto)]
        private extern static UInt32 FindMimeFromData(IntPtr pBC, [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
                                                      [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1, SizeParamIndex = 3)] byte[] pBuffer, int cbSize,
                                                      [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed,
                                                      int dwMimeFlags, out IntPtr ppwzMimeOut, int dwReserverd);
    }
}