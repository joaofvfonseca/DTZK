using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class DTZK_TCPClient : MonoBehaviour
{
    private string serverIP;
    private TcpClient socketConnection;

    void Start()
    {
        SetServerIP("192.168.1.121");
    }

    public void SetServerIP(string param)
    {
        serverIP = param;
        socketConnection = new TcpClient(serverIP, 12813);
    }

    public void SendData(string param)
    {
        if (serverIP == "")
        {
            return;
        }
        try
        {
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                byte[] paramAsByteArray = Encoding.ASCII.GetBytes(param);
                stream.Write(paramAsByteArray, 0, paramAsByteArray.Length);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
