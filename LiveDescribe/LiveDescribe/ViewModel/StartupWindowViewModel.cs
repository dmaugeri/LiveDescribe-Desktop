using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using LiveDescribe.Extensions;
using LiveDescribe.Factories;
using LiveDescribe.Model;
using LiveDescribe.Properties;
using LiveDescribe.Resources.UiStrings;
using Microsoft.Win32;

namespace LiveDescribe.ViewModel
{
    public class StartupWindowViewModel
    {

        #region Instance Variables
        private readonly MainWindowViewModel _mainWindowViewModel;
        #endregion

        #region EventHandlers
        public event EventHandler WindowClosedRequested;
        #endregion

        public StartupWindowViewModel(MainWindowViewModel mainWindowViewModel)
        {

            _mainWindowViewModel = mainWindowViewModel;
            RecentFilePaths = new ObservableCollection<NamedFilePath>();
            SetRecentDocumentsList();

            OpenProjectOnStartup = new RelayCommand(
                canExecute: () => true,
                execute: () =>
                {
                    var projectChooser = new OpenFileDialog
                    {
                        Filter = string.Format(UiStrings.OpenFileDialog_Format_OpenProject, Project.Names.ProjectExtension)
                    };

                    bool? dialogSuccess = projectChooser.ShowDialog();
                    if (dialogSuccess != true)
                        return;

                    _mainWindowViewModel.OpenProjectPath.Execute(projectChooser.FileName);
                    OnWindowClosed();
                });

            NewProjectOnStartup = new RelayCommand(
                canExecute: () => true,
                execute: () =>
                {

                    var viewModel = DialogShower.SpawnNewProjectView();
                    if (viewModel.DialogResult != true)
                        return;

                    if (viewModel.CopyVideo)
                        _mainWindowViewModel.CopyVideoAndSetProject(viewModel.VideoPath, viewModel.Project);
                    else
                        _mainWindowViewModel.SetProject(viewModel.Project);
                    OnWindowClosed();
                });
        }
        #region Properties
        public ObservableCollection<NamedFilePath> RecentFilePaths { get; private set; }
        #endregion

        private void SetRecentDocumentsList()
        {
            foreach (var namedFilePath in Settings.Default.RecentProjects)
                RecentFilePaths.Add(namedFilePath);
        }

        public ICommand OpenProjectOnStartup { get; private set; }
        public ICommand NewProjectOnStartup { get; private set; }

        #region Event Invocations
        public void OnWindowClosed()
        {
            EventHandler handler = WindowClosedRequested;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion
    }
}
