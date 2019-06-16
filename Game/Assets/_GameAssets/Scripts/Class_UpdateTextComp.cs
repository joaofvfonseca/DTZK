using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Class_UpdateTextComp : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private int param;

    public void GoAndUpdate()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
        Debug.Log("hey, it works");
    }
}
