using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace TCP_Server
{
    public partial class Form1 : Form
    {
        Thread thread = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive) return;
            thread = new Thread(ServerFunc);
            thread.IsBackground = true;
            thread.Start();
            Text = "Server is starting!";
        }

        private void ServerFunc(object? obj)
        {
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())[1];
            //passive socket
            TcpListener listener = new TcpListener(IPAddress.Any, 11000);
            try
            {
                listener.Start(5);
                do
                {
                    if (listener.Pending())
                    {
                        //active socket
                        TcpClient client = listener.AcceptTcpClient();
                        byte[] buff = new byte[4096];
                        NetworkStream ns = client.GetStream();
                        int len = ns.Read(buff, 0, buff.Length);
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"{len} receive from {client.Client.RemoteEndPoint}");
                        string result = Encoding.Default.GetString(buff, 0, len);
                        sb.AppendLine(result);
                        if (result.Trim() == "EXIT")
                        {
                            listener.Stop();
                            textBox1.BeginInvoke(new Action(() =>
                            {
                                textBox1.AppendText("Chat was stopped!" + Environment.NewLine);
                            }));
                            return;
                        }
                        else
                        {
                            textBox1.BeginInvoke(new Action<string>(AddText), sb.ToString());
                        }


                        client.Client.Shutdown(SocketShutdown.Both);
                        client.Close();
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                } while (true);
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Помилка сокета: " + sockEx.Message);
            }

            catch (Exception Ex)
            {
                MessageBox.Show("Помилка : " + Ex.Message);
            }
            finally
            {
                listener.Stop();
            }
        }

        private void AddText(string text)
        {
            StringBuilder sb = new StringBuilder(textBox1.Text);
            sb.AppendLine(text);
            textBox1.Text = sb.ToString();
        }
    }
}
