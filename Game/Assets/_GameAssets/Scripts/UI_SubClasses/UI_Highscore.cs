using UnityEngine;
using TMPro;

public class UI_Highscore : Class_UpdateTextComp
{
    public override void GoAndUpdate()
    {
        param = PlayerPrefs.GetInt("Highscore");
        GetComponent<TextMeshProUGUI>().text = string.Format(text, param);
    }
}
