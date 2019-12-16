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

    public void SetEasy()
    {
        gameManager.GetComponent<GameManager>().SetNumShots(100);
    }

    public void SetMedium()
    {
        gameManager.GetComponent<GameManager>().SetNumShots(10);
    }

    public void SetHard()
    {
        gameManager.GetComponent<GameManager>().SetNumShots(5);
    }
}
