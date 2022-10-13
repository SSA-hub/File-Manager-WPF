using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagerWPF.Searchbar
{
    public class SearchFileItem
    {
        public string Name { get; set; }
    }

    public static class SearchFiles
    {
        public static List<string> Find(string path, string name)
        {
            return Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories).Where(f => f.Contains(name) && f.LastIndexOf(name) > f.LastIndexOf('\\')).ToList();
        }
    }
}
