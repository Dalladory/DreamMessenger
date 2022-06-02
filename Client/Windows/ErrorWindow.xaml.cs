using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public ErrorWindow()
        {
            InitializeComponent();
        }
        public ErrorWindow( string text, string header="Error")
        {
            InitializeComponent();
            tBlockHeader.Text = header;
            tBlockText.Text = text;

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
