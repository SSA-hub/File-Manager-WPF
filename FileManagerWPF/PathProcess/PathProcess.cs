using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace FileManagerWPF
{
    public static class PathProcess
    {
        private static List<DriveInfo> GetDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfo> newAllDrives = new List<DriveInfo>();
            foreach (var driveInfo in allDrives)
            {
                if (driveInfo.IsReady)
                {
                    newAllDrives.Add(driveInfo);
                }
            }
            return newAllDrives;
        }

        private static List<DirectoryInfo> GetDirectories(String path)
        {
            if (path == "" || path == null)
            {
                return null;
            }
            if (Path.HasExtension(path))
            {
                path = Path.GetDirectoryName(path);
            }
            return new DirectoryInfo(path).GetDirectories().ToList();
        }

        private static List<FileInfo> GetFiles(String path)
        {
            if (path == "" || path == null)
            {
                return null;
            }
            if (Path.HasExtension(path))
            {
                path = Path.GetDirectoryName(path);
            }
            return new DirectoryInfo(path).GetFiles().ToList();
        }

        public static DirectoryContentRepository GetDirectoryContentRepository(String path, bool full)
        {
            var newDirectoryContentRepository = new DirectoryContentRepository();
            DirectoryContent directoryContent;
            if (path == "" || path == null)
            {
                var driveList = GetDrives();
                newDirectoryContentRepository = new DirectoryContentRepository();
                foreach (var driveInfo in driveList)
                {
                    directoryContent = new DirectoryContent();
                    directoryContent.Name = driveInfo.Name;
                    directoryContent.Icon = new BitmapImage(new Uri(@"..\..\icons\drive.png", UriKind.Relative));
                    newDirectoryContentRepository.DirectoryContent.Add(directoryContent);
                }
                return newDirectoryContentRepository;
            }
            var directoriesList = GetDirectories(path);
            foreach (var directoryInfo in directoriesList)
            {
                directoryContent = new DirectoryContent();
                directoryContent.Name = directoryInfo.Name;
                directoryContent.Icon = new BitmapImage(new Uri(@"..\..\icons\folder.png", UriKind.Relative));
                newDirectoryContentRepository.DirectoryContent.Add(directoryContent);
            }
            if (full)
            {
                var filesList = GetFiles(path);
                foreach (var file in filesList)
                {
                    directoryContent = new DirectoryContent();
                    directoryContent.Name = file.Name;
                    directoryContent.Icon = new BitmapImage(new Uri(@"..\..\icons\file.png", UriKind.Relative));
                    newDirectoryContentRepository.DirectoryContent.Add(directoryContent);
                }
            }            
            return newDirectoryContentRepository;
        }
    }
}
