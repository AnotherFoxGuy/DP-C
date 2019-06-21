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
        Client client = new Client();
        Client laasteAr = new Client();
        public MainPage()
        {
            InitializeComponent();
            
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (client.CheckValidIpAddress(Iptext.Text))
            {
                client.SetConnection(Iptext.Text, 8000);
                if (e.Value)
                {
                    Test.Text = "True";
                    Connecting("T");
                }
                else
                {
                    Test.Text = "flase";
                    Connecting("F");
                }
            }
            

        }
     
        public void Connecting(string data)
        {
            
            client.OpenConnection();
            Test.Text = "test";
            client.SendData(data);
            client.CloseConnection();
        }
        


    }
}
