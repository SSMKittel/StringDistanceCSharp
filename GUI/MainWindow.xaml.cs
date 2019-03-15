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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using SSMKittel.StringDistance;
using System.Diagnostics;

namespace GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Model();
        }

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ((Model) DataContext).File = fileDialog.FileName;
            }
        }

        private async void ComputeMatches(object sender, RoutedEventArgs e)
        {
            computeButton.IsEnabled = false;
            Model m = (Model)DataContext;

            try
            {
                var watch = new Stopwatch();
                watch.Start();
                var matches = await GetMatchesAsync(m.File, m.Threshold);
                watch.Stop();

                double seconds = watch.ElapsedMilliseconds / 1000.0;

                this.Dispatcher.Invoke(() => computeTime.Content = $"Ran in {seconds} seconds");
                this.Dispatcher.Invoke(() => matchesGrid.ItemsSource = matches.Select(x => Tuple.Create(x.First, x.Second, x.Distance)));
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace(Environment.NewLine, " ");
                this.Dispatcher.Invoke(() => computeTime.Content = $"Error: {msg}");
            }
            this.Dispatcher.Invoke(() => computeButton.IsEnabled = true);
        }

        private async Task<IList<Match>> GetMatchesAsync(string file, uint threshold)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous | FileOptions.SequentialScan))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }
            var matcher = MatcherBuilder.Matcher(lines);
            return await Task.Run(() => matcher.Matches(lines, threshold));
        }
    }

    internal class Model : INotifyPropertyChanged
    {
        private uint _threshold = 2;
        private string _file;

        public event PropertyChangedEventHandler PropertyChanged;

        public uint Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Threshold"));
            }
        }
        public string File
        {
            get { return _file; }
            set
            {
                _file = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("File"));
            }
        }
    }
}
