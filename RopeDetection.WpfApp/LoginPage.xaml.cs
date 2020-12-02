using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            Btn_Login.Click += BtnLoginClick;
        }

        private void BtnRegistrationClick(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage();
            this.NavigationService.Navigate(registrationPage);
        }
        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {

            ListOfModels listOfModels = new ListOfModels();
            this.NavigationService.Navigate(listOfModels);
        }
    }
}
