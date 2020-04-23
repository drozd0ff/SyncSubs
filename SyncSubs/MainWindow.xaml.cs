using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Subtitle;
using SubtitlesParser.Classes.Parsers;

namespace SyncSubs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SubParser parser = new SubParser();

        private FileStream stream1;
        private string filePath1;
        private FileStream stream2;
        private string filePath2;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Synchronize(int firstNum, int secondNum)
        {
            using (stream1)
            using (stream2)
            {
                var items = parser.ParseStream(stream1);
                var itemss = parser.ParseStream(stream2);

                var startTiming = items.Select(x => x.StartTime).ToArray();
                var endTiming = items.Select(x => x.EndTime).ToArray();

                var startTimingSample = itemss.Select(x => x.StartTime).ToArray();
                var endTimingSample = itemss.Select(x => x.EndTime).ToArray();

                List<SubtitleFile> resultSubtitleList = new List<SubtitleFile>();

                try
                {
                    for (; firstNum < startTiming.Length; firstNum++, secondNum++)
                    {
                        startTiming[firstNum] = startTimingSample[secondNum];
                        endTiming[firstNum] = endTimingSample[secondNum];
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                for (int k = 0; k < items.Count; k++)
                {
                    resultSubtitleList.Add(new SubtitleFile(startTiming[k], endTiming[k], items[k].Lines));
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Subtitle files (*.srt)|*.srt",
                Title = "Open subtitle file",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                subtitleText1.Text = File.ReadAllText(openFileDialog.FileName);
                filePath1 = openFileDialog.FileName;
                stream1 = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Subtitle files (*.srt)|*.srt",
                Title = "Open subtitle file",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                subtitleText2.Text = File.ReadAllText(openFileDialog.FileName);
                filePath2 = openFileDialog.FileName;
                stream2 = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Synchronize(Convert.ToInt32(firstNumberInput.Text), Convert.ToInt32(secondNumberInput.Text));
        }
    }
}
