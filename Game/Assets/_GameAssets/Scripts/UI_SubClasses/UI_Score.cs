using UnityEngine;
using TMPro;

public class UI_Score : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = GameObject.Find("!MANAGER").GetComponent<Game_Manager>().GetAvaiableScore();
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
