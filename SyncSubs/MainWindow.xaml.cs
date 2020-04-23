﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
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
using Microsoft.Win32;

namespace SyncSubs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StreamReader

        public MainWindow()
        {
            InitializeComponent();
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
            }
        }
    }
}
