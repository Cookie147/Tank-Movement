using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{

    public int colour;
    public float timer;
    public MeshRenderer mr;
    public Material yellowMat, redMat;

    private const int YELLOW = 0;
    private const int RED = 1;


    // Start is called before the first frame update
    void Start()
    {
        timer = 15f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Explode();
        else if (timer <= 5f)
            Flash();
    }

    /*
     * makes the mine explode and takes all units in the explosion radius with it
     */
    private void Explode()
    {

    }

    /*
     * when the timer is low the mine will begin to swap between red and yellow indicating that the explosion is near
     * the lower the timer the faster the materials will be switched
     */
    private void Flash()
    {
        if (timer % 1 <= 0.5f)
        {
            if(colour == YELLOW)
            {
                mr.material = redMat;
            }
            else if(colour == RED)
            {
                mr.material = yellowMat;
            }
        }
    }
}
