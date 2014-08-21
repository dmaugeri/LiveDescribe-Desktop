using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LiveDescribe.Model;
using LiveDescribe.Properties;
using LiveDescribe.ViewModel;

namespace LiveDescribe.View
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        private readonly StartupWindowViewModel _dataContext;
        private bool _shutdownapp = true;

        public StartupWindow(StartupWindowViewModel dataContext, MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContext = dataContext;
            _dataContext = dataContext;
            _mainWindowViewModel = mainWindowViewModel;
            dataContext.WindowClosedRequested += (sender, args) => CloseWindowWithoutClosingApp();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindowAndApp();
        }

        private void StartupWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (_shutdownapp)
            {
                Application.Current.Shutdown();
            }
        }

        private void RecentProjectItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((ListViewItem)sender).Content;
            var itemPath = item as NamedFilePath;
            if (itemPath == null) return;

            _mainWindowViewModel.OpenProjectPath.Execute(itemPath.Path);
            _dataContext.OnWindowClosed();
        }

        private void CloseWindowWithoutClosingApp()
        {
            _shutdownapp = false;
            Close();
        }

        private void CloseWindowAndApp()
        {
            _shutdownapp = true;
            Close();
        }
    }
}
