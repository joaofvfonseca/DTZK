using UnityEngine;
using TMPro;

public class UI_FinalRounds : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = GameObject.Find("!MANAGER").GetComponent<Game_Manager>().GetRoundNumber();
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
