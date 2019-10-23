using DriveManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalFileLister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<DriveDto> DriveList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            SetDrives();
        }

        private void SetDrives()
        {
            try
            {
                DriveList = new ObservableCollection<DriveDto>();
                var driveList = DriveOperations.GetLocalDrives();
                foreach (var drive in driveList)
                {
                    DriveList.Add(drive);
                }
                this.DataContext = this;
            }
            catch
            {

            }
        }

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var fullFileList = new List<LocalFileDto>();
                foreach(var drive in DriveList.ToArray())
                {
                    if (drive.IsSelected)
                    {
                        fullFileList.AddRange( DriveOperations.GetLocalFiles(drive.DriveLetter));
                    }
                }

                lvFiles.ItemsSource = fullFileList;
            }
            catch {}

        }


    }
}
