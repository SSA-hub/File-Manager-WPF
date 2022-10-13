using System;

namespace FileManagerWPF.PathProcessInfoAndOther
{
    public class PathViewContent
    {
        private String _folreName;

        public PathViewContent()
        {
            FolderName = null;
        }

        public string FolderName
        {
            get { return _folreName; }
            set { _folreName = value; }
        }
    }
}
