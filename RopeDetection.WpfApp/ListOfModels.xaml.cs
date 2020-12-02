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
    /// Логика взаимодействия для ListOfModels.xaml
    /// </summary>
    public partial class ListOfModels : Page
    {
        public ListOfModels()
        {
            InitializeComponent();
            Btn_Done.Click += BtnDoneClick;
           // AddDoubleClickEventStyle(ListOfModels, new MouseButtonEventHandler(listView1_MouseDoubleClick));

        }

        private void BtnDoneClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ListModels.SelectedItem.ToString()))
                MessageBox.Show("Необходимо выбрать модель !");
            else
            {
                string NameModel = ((ListBoxItem)ListModels.SelectedItem).Content.ToString();
                ImagesDownload operatingModePage = new ImagesDownload(NameModel);
                this.NavigationService.Navigate(operatingModePage);
            }           
        }
        //private void AddDoubleClickEventStyle(ListBox listBox, MouseButtonEventHandler mouseButtonEventHandler)
        //{
        //    if (listBox.ItemContainerStyle == null)
        //        listBox.ItemContainerStyle = new Style(typeof(ListBoxItem));
        //    listBox.ItemContainerStyle.Setters.Add(new EventSetter()
        //    {
        //        Event = MouseDoubleClickEvent,
        //        Handler = mouseButtonEventHandler
        //    });
        //}

    }
}
