using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardcastSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.Boardcast Server 2.Boardcast Client");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    BoardcastServer server = new BoardcastServer();
                    break;
                case "2":
                    BoardcastClient client = new BoardcastClient();
                    break;
            }
            
            Console.Read();
        }
    }

    public class BoardcastServer
    {
        #region Declarations
        private Socket sock;
        private IPEndPoint iep1;
        private byte[] data;
        #endregion

        #region Memberfunction
        /// <summary>
        /// Constructor
        /// </summary>
        public BoardcastServer()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            iep1 = new IPEndPoint(IPAddress.Broadcast, 9050);

            data = Encoding.ASCII.GetBytes("hello");
            sock.SetSocketOption(SocketOptionLevel.Socket,
            SocketOptionName.Broadcast, 1);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    sock.SendTo(data, iep1);
                    Thread.Sleep(2000);
                }
            });
        }
        #endregion
    }

    public  class BoardcastClient
    {
        #region Memberfunction
        public BoardcastClient()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 9050);
            sock.Bind(iep);
            EndPoint ep = (EndPoint)iep;
            Console.WriteLine("Ready to receive…");

            byte[] data = null;
            int recv = 0;
            string stringData = string.Empty;

            while (true)
            {
                data = new byte[1024];
                recv = sock.ReceiveFrom(data, ref ep);
                stringData = Encoding.ASCII.GetString(data, 0, recv);
                Console.WriteLine("received: {0} from: {1}", stringData, ep.ToString());
            }
        }
        #endregion
    }
}
