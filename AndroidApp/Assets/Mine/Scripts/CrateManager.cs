using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour
{
    [SerializeField] private DTZK_TCPClient dtzk_tcpClient;

    void Start()
    {
        
    }

    void Update()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetKeyDown(KeyCode.K)))
        {
            dtzk_tcpClient.SendData("ammo");
        }
    }
}
