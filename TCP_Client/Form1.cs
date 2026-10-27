using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            try
            {
                await client.ConnectAsync(address, 11000);
                using(NetworkStream ns = client.GetStream())
                {
                    byte[] data = Encoding.Default.GetBytes(textBox1.Text);
                    await ns.WriteAsync(data, 0, data.Length);
                }
                textBox1.Clear();
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Помилка сокета: " + sockEx.Message);
            }

            //catch (Exception Ex)
            //{
            //    MessageBox.Show("Помилка : " + Ex.Message);
            //}
            finally
            {
                client.Close();
            }
        }
    }
}
