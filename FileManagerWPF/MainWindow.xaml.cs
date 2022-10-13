using FileManagerWPF.Searchbar;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManagerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PathProcessInfo _pathProcessInfo;
        private string _path = String.Empty;
        public MainWindow()
        {
            InitializeComponent();
            _pathProcessInfo = new PathProcessInfo(ListView, PathView, SearchView, this);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListView.SelectedItem != null)
            {
                if (_path == String.Empty)
                {
                    _ = _pathProcessInfo.GoTo(((DirectoryContent)ListView.SelectedItem).Name);
                    _path = _pathProcessInfo.Path1;
                }
                else
                {
                    _ = _pathProcessInfo.GoToRelative(((DirectoryContent)ListView.SelectedItem).Name);
                    _path = _pathProcessInfo.Path1;
                }
            }
        }

        private void ListView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ListView.SelectedItem != null && e.ClickCount == 1)
            {
                if (_path == String.Empty)
                {
                    var success = FileInfoRepository.FileInfoRepository.GetInfo(((DirectoryContent)ListView.SelectedItem).Name, out var size, out var filesCount, out var createdDate, out var updatedDate, out var isFile);
                    if (!success)
                        ListView.SelectedItem = null;
                    else
                    {
                        if (isFile)
                        {
                            Info1.Text = "File size: " + size;
                            Info2.Text = "Created date: " + createdDate;
                            Info3.Text = "Updated date: " + updatedDate;
                        }
                        else
                        {
                            Info1.Text = "Folder size: " + size;
                            Info2.Text = "Files amount: " + filesCount;
                            Info3.Text = "";
                        }
                    }
                }
                else
                {
                    var success = FileInfoRepository.FileInfoRepository.GetInfoRelative(((DirectoryContent)ListView.SelectedItem).Name, _path, out var size, out var filesCount, out var createdDate, out var updatedDate, out var isFile);
                    if (!success)
                        ListView.SelectedItem = null;
                    else
                    {
                        if (isFile)
                        {
                            Info1.Text = "File size: " + size;
                            Info2.Text = "Created date: " + createdDate;
                            Info3.Text = "Updated date: " + updatedDate;
                        }
                        else
                        {
                            Info1.Text = "Folder size: " + size;
                            Info2.Text = "Files amount: " + filesCount;
                            Info3.Text = "";
                        }
                    }
                }
            }
        }

        private void BackButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_path != String.Empty && _path != null)
            {
                _pathProcessInfo.GoBack();
                _path = _pathProcessInfo.Path1;
            }
        }

        private void PathView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PathView.SelectedItem != null)
            {
                _pathProcessInfo.GoToClicked();
                _path = _pathProcessInfo.Path1;
            }
        }

        private void SearchQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchQuery.Text.Length != 0)
                _pathProcessInfo.SearchFilesByName(SearchQuery.Text);
        }

        private void SearchQuery_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (SearchQuery.Text.Length != 0)
                _pathProcessInfo.SearchFilesByName(SearchQuery.Text);
        }

        private void SearchView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SearchView.SelectedItem != null)
            {
                _pathProcessInfo.GoToClickedSearchbar(((SearchFileItem)SearchView.SelectedItem).Name);
                _path = _pathProcessInfo.Path1;
            }
        }
    }
}
