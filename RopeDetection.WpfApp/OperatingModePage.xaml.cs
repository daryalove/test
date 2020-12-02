using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RopeDetection.WpfApp
{
    /// <summary>
    /// Логика взаимодействия для OperatingModePage.xaml
    /// </summary>
    public partial class OperatingModePage : Page
    {       
        public OperatingModePage(string NameModel)
        {
            InitializeComponent();
            Btn_Analysis.Click += BtnAnalysisClick;
            Btn_Learning.Click += BtnLearningClick;
        }

        private void BtnLearningClick(object sender, RoutedEventArgs e)
        {
            ListOfModels imagesDownload = new ListOfModels();
            this.NavigationService.Navigate(imagesDownload);
        }

        private void BtnAnalysisClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
