using System;
using System.IO;

namespace CodeShell.Helpers
{
    public static class AttachmentHelper
    {/// <summary>
     /// Move file from folder to an other
     /// </summary>
     /// <param name="sorsFl">source file</param>
     /// <param name="distFl">distination file</param>
     /// <returns>bool true if success else false</returns>
        public static bool MoveFile(string sorsFl, string distFl)
        {
            try
            {
                string fldr = GetFolder(distFl);
                if (!Directory.Exists(fldr))
                    Directory.CreateDirectory(fldr);
                if (File.Exists(distFl))
                {
                    File.Delete(distFl);
                }
                File.Move(sorsFl, distFl);
                return true;
            }
            catch
            {
                //string err = ex.Message;
                return false;
            }
        }

        // <summary>
        /// Get file containing folder
        /// </summary>
        /// <param name="fileFullPath">File path</param>
        /// <returns>string folder path</returns>
        private static string GetFolder(string fileFullPath)
        {
            int indx = fileFullPath.LastIndexOf('\\');
            return fileFullPath.Substring(0, indx);
        }
    }
}
