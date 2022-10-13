using System;
using System.IO;

namespace FileManagerWPF.FileInfoRepository
{
    public static class FileInfoRepository
    {
        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public static bool GetInfo(string path, out long size, out int? filesCount, out DateTime? createdDate, out DateTime? updatedDate, out bool isFile)
        {
            isFile = false;
            size = 0;
            filesCount = 0;
            createdDate = null;
            updatedDate = null;
            try
            {
                if (path != "")
                {
                    if (Path.HasExtension(path))
                    {
                        isFile = true;
                        var fileInfo = new FileInfo(path);
                        size = fileInfo.Length;
                        createdDate = fileInfo.CreationTime;
                        updatedDate = fileInfo.LastWriteTime;
                        filesCount = null;
                    }
                    else
                    {
                        isFile = false;
                        var dirInfo = new DirectoryInfo(path);
                        size = DirSize(dirInfo);
                        filesCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
                        createdDate = null;
                        updatedDate = null;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool GetInfoRelative(string relativePath, string path, out long size, out int? filesCount, out DateTime? createdDate, out DateTime? updatedDate, out bool isFile)
        {
            isFile = false;
            size = 0;
            filesCount = 0;
            createdDate = null;
            updatedDate = null;
            try
            {
                if (relativePath != "")
                {
                    if (Path.HasExtension(relativePath))
                    {
                        isFile = true;
                        var fileInfo = new FileInfo(path + "\\" + relativePath);
                        size = fileInfo.Length;
                        createdDate = fileInfo.CreationTime;
                        updatedDate = fileInfo.LastWriteTime;
                        filesCount = null;
                    }
                    else
                    {
                        isFile = false;
                        var dirInfo = new DirectoryInfo(path + "\\" + relativePath);
                        size = DirSize(dirInfo);
                        filesCount = Directory.GetFiles(path + "\\" + relativePath, "*", SearchOption.AllDirectories).Length;
                        createdDate = null;
                        updatedDate = null;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
