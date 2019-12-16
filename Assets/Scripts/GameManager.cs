using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int maxShots;
    private bool playerAlive;
    private float timer = 0.0f;
    private float waitingTime = 2.5f;
    public GameObject tank;
    public GameObject chassis;
    public GameObject leftTracks;
    public GameObject rightTracks;
    public GameObject turret;
    public Material newMaterial;
    private bool colorSet = false;


    void Awake()
    {
        //dont destroy script in next sceen
        DontDestroyOnLoad(gameObject);

        maxShots = 5;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            //in loadscreen
            if(SceneManager.GetActiveScene().buildIndex%2 == 1)
            {
                //Wait for 2.5 seconds in loadscreen of next level
                timer += Time.deltaTime;
                if (timer > waitingTime)
                {
                    timer = 0f;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    colorSet = false;
                }
            }

            //in level
            if(SceneManager.GetActiveScene().buildIndex%2 == 0)
            {
                if (colorSet == false)
                {
                    ColorTank();
                }

                WinOrLose();
            }
        }
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

    public void SetColor(Material choosenMaterial)
    {
        newMaterial = choosenMaterial;
        Debug.Log(newMaterial);
    }

    public void WinOrLose()
    {
        playerAlive = true;

        //player dead?
        if (GameObject.FindGameObjectWithTag("Player Tank") == null)
        {
            playerAlive = false;

            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                timer = 0f;
                SceneManager.LoadScene("DeadScreen");
            }

            timer += Time.deltaTime;
            if (timer > waitingTime)
            {
                timer = 0f;
                colorSet = false;
                OpenMenu();
            }
        }

        //all enemies dead?
        if (GameObject.FindGameObjectsWithTag("Enemy Tank").Length == 0 && playerAlive)
        {

            timer += Time.deltaTime;
            if (timer > 1f)
            {
                timer = 0f;
                //load loadsceen of next level
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    public void ColorTank()
    {
        tank = GameObject.FindGameObjectWithTag("Player Tank");
        chassis = tank.transform.Find("TankChassis").gameObject;
        leftTracks = tank.transform.Find("TankTracksLeft").gameObject;
        rightTracks = tank.transform.Find("TankTracksRight").gameObject;
        turret = tank.transform.Find("TankTurret").gameObject;

        chassis.GetComponent<Renderer>().material = newMaterial;
        leftTracks.GetComponent<Renderer>().material = newMaterial;
        rightTracks.GetComponent<Renderer>().material = newMaterial;
        turret.GetComponent<Renderer>().material = newMaterial;

        colorSet = true;
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
