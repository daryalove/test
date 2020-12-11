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
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Btn_Login.Click += BtnLoginClick;
            Btn_Registration.Click += BtnRegistrationClick;
        }

        private void BtnRegistrationClick(object sender, RoutedEventArgs e)
        {
            ListItems registrationPage = new ListItems();
            this.NavigationService.Navigate(registrationPage);
        }

        private void BtnLoginClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
