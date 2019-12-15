using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSet : MonoBehaviour
{
    public GameObject gameManager;
    public Material myMaterial;

    public void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    public void setColor1()
    {
        myMaterial = (Material)Resources.Load("BlueTankColor", typeof(Material));
        gameManager.GetComponent<GameManager>().SetColor(myMaterial);
    }

    public void setColor2()
    {
        myMaterial = (Material)Resources.Load("RedTankColor", typeof(Material));
        gameManager.GetComponent<GameManager>().SetColor(myMaterial);
    }

    public void setColor3()
    {
        myMaterial = (Material)Resources.Load("PinkTankColor", typeof(Material));
        gameManager.GetComponent<GameManager>().SetColor(myMaterial);
    }

    public void setColor4()
    {
        myMaterial = (Material)Resources.Load("GreenTankColor", typeof(Material));
        gameManager.GetComponent<GameManager>().SetColor(myMaterial);
    }

}
