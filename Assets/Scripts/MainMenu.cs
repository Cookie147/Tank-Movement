using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playerTank;
    public Difficulty d;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //playerTank = GameObject.Find("Player Tank");
        //Debug.Log("Yes");
        //float numShots = d.getDifficulty();
        //playerTank.SendMessage("setMaxShots", numShots);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    

}
