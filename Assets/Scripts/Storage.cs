using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int maxShots;
    //Sounds

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        maxShots = 5;
    }

    //Difficulty Settings
    public void setNumShots(int numShots)
    {
        maxShots = numShots;
    }

    public int getNumShots()
    {
        return maxShots;
    }
}
