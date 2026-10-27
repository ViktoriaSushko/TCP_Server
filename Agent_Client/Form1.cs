using System.Net;
using System.Net.Sockets;

namespace Agent_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetPicture_Click(object sender, EventArgs e)
        {
            //active socket
            TcpClient client = new TcpClient();
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            IPEndPoint ep = new IPEndPoint(address, 11000);
            try
            {
                client.Connect(ep);
                using (NetworkStream ns = client.GetStream())
                {
                    Image image = Image.FromStream(ns);
                    Bitmap bmp = new Bitmap(image, pictureBox1.ClientSize);
                    pictureBox1.Image = bmp;
                }
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Помилка сокета: " + sockEx.Message);
            }

        }
    }
}
