using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class DTZK_TCPClient : MonoBehaviour
{
    private string serverIP;
    private TcpClient socketConnection;

    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TMP_InputField inputText;

    void Start()
    {
        statusText.text = "Disconnected";
    }

    public void SetServerIP()
    {
        serverIP = inputText.text;
        if (serverIP.Length < 7 || serverIP == null)
        {
            statusText.text = "Please type a valid IPv4 address";
            return;
        }
        try
        {
            socketConnection = new TcpClient(serverIP, 12813);
        }
        catch (System.Exception socketException)
        {
            statusText.text = socketException.ToString();
            return;
        }
        try
        {
            if (socketConnection.GetStream().CanWrite)
            {
                statusText.text = "Connected @ " + serverIP;
            }
            else
            {
                statusText.text = "Could not connect, try again";
            }
        }
        catch (System.Exception socketException)
        {
            statusText.text = socketException.ToString();
            return;
        }
    }

    public void SendData(string param)
    {
        if (serverIP == null || socketConnection == null)
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

    private void OnApplicationQuit()
    {
        socketConnection.Close();
    }
}
