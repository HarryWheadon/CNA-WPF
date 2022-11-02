using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace CNA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (messageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "warning");
            }
            else
            {
                string message = messageText.Text;

                // The work to perform on another thread
                ThreadStart ButtonThread = delegate ()
                {
                // sets the text on a Textblock Control
                // This will work as its using the dispatcher
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action<string>(SendMessage), message);
                };
                // Create new thread and kick it started!
                new Thread(ButtonThread).Start();
            }
        }
        private void SendMessage(string status)
        {
            chatBox.Text = status;
        }

        private void localName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

/*if (messageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "warning");
            }
            else
            {
                string message = messageText.Text;
                messageText.Text = "";
                if(localName.Text == "")
                {
                    MessageBox.Show("Please enter a name in Local Name Textbox!", "warning");
                    messageText.Text = message;
                }
                else
                {
                    string name = localName.Text;
                    chatBox.Text += name + " : " + message;
                }
            }*/