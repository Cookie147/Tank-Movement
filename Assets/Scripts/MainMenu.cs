﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playerTank;
    public GameObject d;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //playerTank = GameObject.Find("Player Tank");
        //change color of tank
        //int difficulty = d.getDifficulty();
        //playerTank.sendMessage("setMaxShots", difficulty);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    

}
