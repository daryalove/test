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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        public RegistrationPage()
        {
            InitializeComponent();
            Btn_Registration.Click += BtnRegistrationClick;
        }

        private void BtnRegistrationClick(object sender, RoutedEventArgs e)
        {
            ListOfModels listOfModels = new ListOfModels();
            this.NavigationService.Navigate(listOfModels);
        }
    }
}
