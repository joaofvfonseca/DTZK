using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Main : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //generate ID
    }

    // Update is called once per frame
    private void Update()
    {
        
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

    public void OpenOptions()
    {

    }

    public void OpenCredits()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
