using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public int difficulty;
    public GameObject playerTank;

    public void setEasy()
    {
        playerTank = GameObject.Find("Player Tank");
        difficulty = 1;
    }

    public int getDifficulty()
    {
        return difficulty;
    }
}
