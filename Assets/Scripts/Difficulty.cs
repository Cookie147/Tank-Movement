using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public int numShots;
    public GameObject storage;

    public void Start()
    {
        storage = GameObject.Find("Storage");
    }

    public void setEasy()
    {
        storage.GetComponent<GameManager>().setNumShots(100);
    }

    public void setMedium()
    {
        storage.GetComponent<GameManager>().setNumShots(10);
    }

    public void setHard()
    {
        storage.GetComponent<GameManager>().setNumShots(5);
    }
}
