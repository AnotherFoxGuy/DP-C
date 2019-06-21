using Project_Green;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Opdracht_C
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {     
        public MainPage()
        {
            InitializeComponent();        
        }
     
        private void IpScanner_Clicked(object sender, EventArgs e)
        {
            IPList.ItemsSource = new IPScanner().Getstrings();
        }

        private void IPList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var client = new Client();
            client.SetConnection((string)e.Item, 8000);
            client.OpenConnection();
            client.SendData("F");
            client.CloseConnection();
        }
        
    }
}
