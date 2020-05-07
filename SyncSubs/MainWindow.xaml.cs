using Microsoft.Win32;
using Subtitle;
using SubtitlesParser.Classes.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SyncSubs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SubParser parser = new SubParser();

        private FileStream recipientStream;
        private string recipientFilePath;
        private FileStream donorStream;
        private string donorFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Synchronize the first subtitle timing with second subtitle timing
        /// </summary>
        /// <param name="recipientNum">Represents the number of line in first subtitle file starting FROM which
        ///                         it will sync timing with second file</param>
        /// <param name="donorNum">Represents the number of line WITH which first subtitle file would sync</param>
        private void Synchronize(int recipientNum, int donorNum)
        {
            using (recipientStream)
            using (donorStream)
            {
                var recipientList = parser.ParseStream(recipientStream);
                var donorList = parser.ParseStream(donorStream);

                int[] recipientStartTimings = recipientList.Select(x => x.StartTime).ToArray();
                int[] recipientEndTimings = recipientList.Select(x => x.EndTime).ToArray();

                int[] donorStartTimings = donorList.Select(x => x.StartTime).ToArray();
                int[] donorEndTimings = donorList.Select(x => x.EndTime).ToArray();

                List<SubtitleFile> resultSubtitleList = new List<SubtitleFile>();

                try
                {
                    for (; recipientNum < recipientStartTimings.Length; recipientNum++, donorNum++)
                    {
                        recipientStartTimings[recipientNum] = donorStartTimings[donorNum];
                        recipientEndTimings[recipientNum] = donorEndTimings[donorNum];
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

                for (int k = 0; k < recipientList.Count; k++)
                {
                    resultSubtitleList.Add(new SubtitleFile(recipientStartTimings[k], recipientEndTimings[k], recipientList[k].Lines));
                }

                string resultSubtitleString = string.Empty;
                foreach (var subtitleFile in resultSubtitleList)
                {
                    resultSubtitleString += subtitleFile;
                }

                recipientText.Text = resultSubtitleString;
            }
        }

        private void FindRecipientFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Subtitle files (*.srt)|*.srt",
                Title = "Open subtitle file",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                recipientText.Text = File.ReadAllText(openFileDialog.FileName);
                recipientFilePath = openFileDialog.FileName;
                recipientStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        private void FindDonorFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Subtitle files (*.srt)|*.srt",
                Title = "Open subtitle file",
            };
            if (openFileDialog.ShowDialog() == true)
            {
                donorText.Text = File.ReadAllText(openFileDialog.FileName);
                donorFilePath = openFileDialog.FileName;
                donorStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            }
        }

        private void SynchronizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(firstNumberInput.Text, out int result) &&
                !int.TryParse(secondNumberInput.Text, out int secondResult))
            {
                MessageBox.Show("Please fill number fields with numeric values");
                return;
            }

            Synchronize(Convert.ToInt32(firstNumberInput.Text) - 1, Convert.ToInt32(secondNumberInput.Text) - 1);
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            if (recipientText == null)
            {
                MessageBox.Show("Please open subtitle file to save");
                return;
            }

        }
    }
}
