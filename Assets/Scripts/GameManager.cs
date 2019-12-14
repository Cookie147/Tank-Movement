using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int maxShots;
    private bool playerAlive;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        maxShots = 5;
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {

            
                Debug.Log("level");
                playerAlive = true;

                if (GameObject.FindGameObjectsWithTag("Player Tank").Length == 0)
                {
                    playerAlive = false;
                    Debug.Log("Loser");
                    SceneManager.LoadScene("Menu");
                }

                if (GameObject.FindGameObjectsWithTag("Enemy Tank").Length == 0 && playerAlive)
                {
                    Debug.Log("Win");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            

            //Debug.Log("Scene" + SceneManager.GetActiveScene().buildIndex);
            //if (SceneManager.GetActiveScene().buildIndex % 2 == 1)
            //{
               // StartCoroutine(DelayLoadLevel(5f));
           // }
        }
    


        /*
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        playerAlive = false;
        numEnemies = 0;

        for(int i=0; i<gameObjects.Length; i++)
        {
            if(gameObjects[i].CompareTag("Player Tank"))
            {
                playerAlive = true;
            }

            if(gameObjects[i].CompareTag("Enemy Tank"))
            {
                ++numEnemies;
            }
        }

        Debug.Log(numEnemies);
        Debug.Log(playerAlive);
        if (!playerAlive)
        {
            Debug.Log("Loser");
        }

        if (numEnemies == 0 && playerAlive)
        {
            Debug.Log("Win");
        } */
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

    private IEnumerator DelayLoadLevel(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log("Wait");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
