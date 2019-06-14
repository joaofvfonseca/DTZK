using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Menu_Main : MonoBehaviour
{

    [SerializeField] private GameObject ui_SetUsername;
    [SerializeField] private GameObject ui_PlayQuit;
    [SerializeField] private TextMeshProUGUI mandatoryUsernameText;
    [SerializeField] private TextMeshProUGUI menuUsernamePlaceholder;
    [SerializeField] private TextMeshProUGUI menuUsernameText;
    [SerializeField] private Button menuSetUsernameButton;
    [SerializeField] private Button mandatorySetUsernameButton;



    // Start is called before the first frame update
    private void Start()
    {
        if (!UserHasUsername())
        {
            ui_SetUsername.SetActive(true);
            ui_PlayQuit.SetActive(false);
        }
        else
        {
            menuUsernamePlaceholder.text = PlayerPrefs.GetString("Username");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(menuUsernameText.text.Length < 4)
        {
            menuSetUsernameButton.interactable = false;
        }
        else
        {
            menuSetUsernameButton.interactable = true;         
        }

        if (mandatoryUsernameText.text.Length < 4)
        {
            mandatorySetUsernameButton.interactable = false;
        }
        else
        {
            mandatorySetUsernameButton.interactable = true;
        }
    }

    private int GenerateID()
    {
        //wish i had time
        return 0;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetUsername()
    {
        if (ui_PlayQuit.activeSelf)
        {
            PlayerPrefs.SetString("Username", menuUsernameText.text);
        }
        else
        {
            PlayerPrefs.SetString("Username", mandatoryUsernameText.text);
            ui_SetUsername.SetActive(false);
            ui_PlayQuit.SetActive(true);
        }        
        menuUsernamePlaceholder.text = PlayerPrefs.GetString("Username");
    }

    private bool UserHasUsername()
    {
        if (PlayerPrefs.HasKey("Username")) return true;
        return false;
    }
}
