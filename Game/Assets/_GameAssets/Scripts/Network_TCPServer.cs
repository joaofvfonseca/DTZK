using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Network_TCPServer : MonoBehaviour
{
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectTcpClient;
    [SerializeField] private Game_PlayerWeapon ammoReloadComp;

    // Start is called before the first frame update
    void Start()
    {
        tcpListenerThread = new Thread(new ThreadStart(ListenForMessages));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    private void ListenForMessages()
    {
        try
        {
            tcpListener = new TcpListener(IPAddress.Parse(LocalIP()), 12813);
            tcpListener.Start();
            Debug.Log("server listening on " + LocalIP());
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectTcpClient = tcpListener.AcceptTcpClient())
                {
                    using (NetworkStream strean = connectTcpClient.GetStream())
                    {
                        int length;
                        while ((length = strean.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            byte[] incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            string message = Encoding.ASCII.GetString(incomingData);
                            Debug.Log(message);
                            ammoReloadComp.ola = true;
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }

    private string LocalIP()
    {
        string localIP = "null";
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
        {
            socket.Connect("8.8.8.8", 65530);
            IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
            localIP = endPoint.Address.ToString();
        }
        return localIP;
    }
}
