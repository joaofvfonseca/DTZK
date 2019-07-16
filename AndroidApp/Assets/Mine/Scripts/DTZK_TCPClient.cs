using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TMPro;

public class DTZK_TCPClient : MonoBehaviour
{
    private string serverIP;
    private TcpClient socketConnection;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI inputText;

    void Start()
    {
        statusText.text = "Disconnected";
    }

    public void SetServerIP()
    {
        serverIP = inputText.text;
        Debug.Log(serverIP);
        if (serverIP.Length < 8 || serverIP == null)
        {
            statusText.text = "Please type a valid IPv4 address";
            return;
        }
        try
        {
            socketConnection = new TcpClient(serverIP, 12813);
        }
        catch (SocketException socketException)
        {
            statusText.text = socketException.ToString();
            return;
        }
        if (socketConnection.GetStream().CanWrite)
        {
            statusText.text = "Connected @ " + serverIP;
        }
        else
        {
            statusText.text = "Could not connect, try again";
        }
    }

    public void SendData(string param)
    {
        if (serverIP == null)
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
