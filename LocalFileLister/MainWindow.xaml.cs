using DriveManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

            GetDrives();
            loadingControl.Visibility = Visibility.Hidden;
        }

        private void GetDrives()
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
                tbxErrorMessage.Text = "Can't access drive information";
            }
        }

        public void GetFiles()
        {
            try
            {               
                var fullFileList = new List<LocalFileDto>();
                foreach (var drive in DriveList.ToArray())
                {
                    if (drive.IsSelected)
                    {
                        fullFileList.AddRange(DriveOperations.GetPathFiles(drive.DriveLetter));
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    lvFiles.ItemsSource = fullFileList;
                });
            }
            catch 
            {
                Dispatcher.Invoke(() =>
                {
                    tbxErrorMessage.Text = "Can't retriving file information";
                });
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    loadingControl.Visibility = Visibility.Hidden;
                });
            }
        }

        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {

               loadingControl.Visibility = Visibility.Visible;

                Thread fileSearchThread = new Thread(()=> { GetFiles(); });
                fileSearchThread.Start();       

        }


    }
}
