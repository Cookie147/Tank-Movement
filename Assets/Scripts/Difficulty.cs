using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public int numShots;
    public GameObject gameManager;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    public void setEasy()
    {
        gameManager.GetComponent<GameManager>().setNumShots(100);
    }

    public void setMedium()
    {
        gameManager.GetComponent<GameManager>().setNumShots(10);
    }

    public void setHard()
    {
        gameManager.GetComponent<GameManager>().setNumShots(5);
    }
}
