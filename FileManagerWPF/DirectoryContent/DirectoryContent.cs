using System;
using System.Windows.Media.Imaging;

namespace FileManagerWPF
{
    public class DirectoryContent
    {
        private String _name;
        private BitmapImage _icon;

        public DirectoryContent()
        {
            Icon = null;
            Name = null;
        }
        public BitmapImage Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
