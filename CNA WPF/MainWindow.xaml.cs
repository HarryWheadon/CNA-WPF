using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
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
using Packets;

namespace CNA_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private Client m_client;
        public MainWindow(Client client)
        {
            InitializeComponent();
            m_client = client;
        }

        // Adds a new message
        public void UpdateChatBox(string message)
        {
            Console.WriteLine("updated chat box");
            chatBox.Dispatcher.Invoke(() =>
            {
                chatBox.Text += message + Environment.NewLine;
                chatBox.ScrollToEnd();
            });
        }

        public void UpdateReturnBox(string message)
        {
            Console.WriteLine("updated return box");
            returnBox.Dispatcher.Invoke(() =>
            {
                returnBox.Text += "Server: " + message + Environment.NewLine;
                returnBox.ScrollToEnd();
            });
        }

        // When the submit button is clicked
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // The message box has to contain something
            if (messageText.Text == "")
            {
                MessageBox.Show("No message in text box!", "warning");
            }
            else
            {
                // A name has to be filled in for the button to be pressed
                if (localName.Text == "")
                {
                    MessageBox.Show("Please enter a name in Local Name Textbox!", "warning");
                }
                else
                {
                    // Sends message as a packet
                    Packet message = new Packets.ChatMessagePacket(messageText.Text);
                    message.packetType = PacketType.CHAT_MESSAGE;
                    m_client.Send(message);
                    UpdateChatBox(messageText.Text);
                    UpdateReturnBox(messageText.Text);
                    messageText.Text = (" ");
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void localName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
