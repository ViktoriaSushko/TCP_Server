using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Agent_Server
{
   
    public partial class Form1 : Form
    {
        Task task = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (task != null) return;
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            //passive socket
          
            task = Task.Run(() =>
            {
                try
                {
                    TcpListener listener = new TcpListener(IPAddress.Any, 11000);
                    listener.Start();
                    do
                    {
                        if (listener.Pending())
                        {
                            //active socket
                            TcpClient client = listener.AcceptTcpClient();
                            using(NetworkStream ns = client.GetStream())
                            {
                                byte[] sendBuff = RecieveScreen();
                                ns.Write(sendBuff);
                            }

                        }
                    } while (true);
                }
                catch (SocketException sockEx)
                {
                    MessageBox.Show("Помилка сокета: " + sockEx.Message);
                }
            });
            
            Text = "Server is starting!";
        }

        private byte[] RecieveScreen()
        {
            ScreenCapture screen = new ScreenCapture();
            Image image = screen.CaptureScreen();
            byte[] buff = null;
            using(MemoryStream ms=new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                buff = ms.ToArray();
            }
            return buff;
        }
    }
}
