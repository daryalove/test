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
    /// Логика взаимодействия для ModelCreation.xaml
    /// </summary>
    public partial class ModelCreation : Page
    {
        public ModelCreation()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Модель была успешно создана !", "Создание модели");
            HomePage _homePage = new HomePage();
            this.NavigationService.Navigate(_homePage);
        }
    }
}
