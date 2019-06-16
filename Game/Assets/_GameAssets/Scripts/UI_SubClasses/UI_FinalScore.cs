using UnityEngine;
using TMPro;

public class UI_FinalScore : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = GameObject.Find("!MANAGER").GetComponent<Game_Manager>().GetFinalScore();
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
