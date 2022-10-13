using System.Collections.ObjectModel;

namespace FileManagerWPF.PathProcessInfoAndOther
{
    public class PathViewRepository
    {
        private ObservableCollection<PathViewContent> _content;
        public ObservableCollection<PathViewContent> Content { get { return _content; } set { _content = value; } }

        public PathViewRepository()
        {
            _content = new ObservableCollection<PathViewContent>();
        }
    }
}
