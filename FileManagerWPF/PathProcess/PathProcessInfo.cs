using FileManagerWPF.Database;
using FileManagerWPF.PathProcessInfoAndOther;
using FileManagerWPF.Searchbar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FileManagerWPF
{
    class PathProcessInfo
    {
        private String _path;
        private DirectoryContentRepository _directoryContentRepository;
        private PathViewRepository _pathViewRepository;
        private ListView _listView;
        private ListBox _pathView;
        private ListView _searchbarView;
        private MainWindow _mainWindow;

        public List<string> PathList { get; set; }

        public PathProcessInfo(ListView listView, ListBox pathView, ListView searchbarView, MainWindow mainWindow)
        {
            _listView = listView;
            _mainWindow = mainWindow;
            _pathView = pathView;
            _searchbarView = searchbarView;
            PathList = new List<string> ();
            var result = Migrations.RunMigrations();
            if (!result)
                MessageBox.Show("Migration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Load();
        }

        private async Task LogFileOpeningHistory(string fileName)
        {
            var repository = new DBRepository();
            var fileOpeningHistoryItem = new FileOpeningHistory(fileName);

            await repository.InsertAsync(fileOpeningHistoryItem);
        }

        public DirectoryContentRepository ContentRepository
        {
            get { return _directoryContentRepository; }
            set { _directoryContentRepository = value; RefreshData(); }
        }

        public PathViewRepository PathViewRepository
        {
            get { return _pathViewRepository; }
            set { _pathViewRepository = value; SetColumnsPathView(); }
        }

        public string Path1
        {
            get { return _path; }
            set { _path = value; }
        }

        public async Task GoTo(String path)
        {
            try
            {
                if (path != "")
                {
                    if (Path.HasExtension(path))
                    {
                        await LogFileOpeningHistory(path);
                        System.Diagnostics.Process.Start(path);
                    }
                    else
                    {
                        if (path != null)
                            PathList.Add(path);
                        Path1 = path;
                        ContentRepository = PathProcess.GetDirectoryContentRepository(Path1, true);
                        PathViewRepository = PathViewProcess.GetPathViewRepository(PathList);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task GoToRelative(String relativePath)
        {
            try
            {
                if (relativePath != "")
                {
                    if (Path.HasExtension(relativePath))
                    {
                        await LogFileOpeningHistory(relativePath);
                        System.Diagnostics.Process.Start(Path1 + "\\" + relativePath);
                    }
                    else
                    {
                        PathList.Add(relativePath);
                        if (Path1[Path1.Length - 1] != '\\')
                            Path1 += "\\";
                        Path1 += relativePath;
                        ContentRepository = PathProcess.GetDirectoryContentRepository(Path1, true);
                        PathViewRepository = PathViewProcess.GetPathViewRepository(PathList);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToPathList(string pathItem)
        {
            var tmp = string.Empty;
            var flag = false;
            for (var i = 0; i < pathItem.Length; i++)
            {
                if (pathItem[i] == '\\') {
                    if (!flag)
                    {
                        tmp += pathItem[i];
                        flag = true;
                    }
                }
                else
                    tmp += pathItem[i];
            }
            PathList.Add(tmp);
        }

        public void GoToClicked()
        {
            PathList.Clear();
            foreach (var pathItem in _pathView.Items)
            {
                if (pathItem == _pathView.SelectedItem)
                {
                    AddToPathList(pathItem.ToString());
                    break;
                }
                AddToPathList(pathItem.ToString());
            }
            var newPath = PathList.Count == 0 ? null : CreatePath();
            Path1 = newPath;
            ContentRepository = PathProcess.GetDirectoryContentRepository(Path1, true);
            PathViewRepository = PathViewProcess.GetPathViewRepository(PathList);
        }

        public void GoToClickedSearchbar(string path)
        {
            if (Path.HasExtension(path))
            {
                _ = LogFileOpeningHistory(path);
                System.Diagnostics.Process.Start(path);
            }
            else
            {
                Path1 = path;
                PathList = path.Split('\\').ToList();
                ContentRepository = PathProcess.GetDirectoryContentRepository(Path1, true);
                PathViewRepository = PathViewProcess.GetPathViewRepository(PathList);
            }
        }

        private String CreatePath()
        {
            var path = PathList[0];
            for (var i = 1; i < PathList.Count; i++)
            {
                if (path[path.Length - 1] != '\\')
                    path += "\\";
                path += PathList[i];
            }
            return path;
        }

        public void GoBack()
        {
            PathList.RemoveRange(PathList.Count - 1, 1);
            var newPath = PathList.Count == 0 ? null : CreatePath();
            Path1 = newPath;
            ContentRepository = PathProcess.GetDirectoryContentRepository(Path1, true);
            PathViewRepository = PathViewProcess.GetPathViewRepository(PathList);
        }

        private void RefreshData()
        {
            _listView.ItemsSource = ContentRepository.DirectoryContent;
            var view = _listView.View;
            if (view == _mainWindow.FindResource("GridView"))
            {
                SetColumnsGridView();
            }
        }

        private void SetColumnsGridView()
        {
            var gridView = (GridView)_mainWindow.FindResource("GridView");
            gridView.Columns.Clear();
            foreach (var directoryContent in _directoryContentRepository.DirectoryContent)
            {
                if (directoryContent.Icon != null)
                {
                    if (SetColumn("Icon", gridView.Columns))
                    {
                        gridView.Columns.Add((GridViewColumn)_mainWindow.FindResource("DataTemplateForIcon"));
                    }
                }
                if (directoryContent.Name != null)
                {
                    if (SetColumn("Name", gridView.Columns))
                    {
                        gridView.Columns.Add((GridViewColumn)_mainWindow.FindResource("DataTemplateForName"));
                    }
                }
            }
        }

        private void SetColumnsPathView()
        {
            if (PathViewRepository != null)
            {
                _pathView.Items.Clear();
                foreach (var pathViewContent in PathViewRepository.Content)
                {
                    if (pathViewContent.FolderName != null)
                    {
                        var tmp = pathViewContent.FolderName;
                        if (tmp[tmp.Length - 1] != '\\')
                            tmp += "\\";
                        _pathView.Items.Add(tmp);
                    }
                }
            }
        }

        public void SearchFilesByName(string name)
        {
            var files = SearchFiles.Find(_path, name);
            SetColumnsSearchbarView(files);
        }

        private void SetColumnsSearchbarView(List<string> files)
        {
            if (files != null)
            {
                _searchbarView.Items.Clear();
                foreach (var file in files)
                    _searchbarView.Items.Add(new SearchFileItem { Name = file });
            }
        }

        private bool SetColumn(String columnHeader, GridViewColumnCollection columns)
        {
            foreach (var column in columns)
            {
                if ((string) column.Header == columnHeader)
                {
                    return false;
                }
            }
            return true;
        }

        private void Load()
        {
            _ = GoTo(Path1);
        }
    }
}
