using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{

    public int maxShots;
    public int maxMines;
    public float time, shotReloadTime;
    //0 is the primary mouse button, 1 the secondary, 2 the middle one
    public int shootButton = 0;
    public string mineButton;
    public float reloadTime, timeSinceShot;
    public Rigidbody turret;
    public Rigidbody shellPrefab;
    public Rigidbody minePrefab;
    public GameObject shotLocation;
    public GameObject[] shots;
    public GameObject[] mines;
    

    // Start is called before the first frame update
    void Start()
    {
        mines = new GameObject[maxMines];
        shots = new GameObject[maxShots];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        timeSinceShot += Time.deltaTime;

        if (timeSinceShot >= reloadTime && Count(shots) < maxShots && Input.GetMouseButtonDown(shootButton))
        {
            Shoot();
            timeSinceShot = 0;
        }

        if(Count(mines) < maxMines && Input.GetKeyDown(mineButton))
        {
            LayMine();
        }
    }

    private void Shoot()
    {
        //animation and sounds
        Rigidbody shot = Instantiate(shellPrefab, shotLocation.transform.position, turret.transform.rotation);
        shot.GetComponent<ShellMovement>().SetMyTank(GetComponentInParent<BoxCollider>().gameObject);
        for(int i=0; i<maxShots; ++i)
        {
            if(shots[i] == null)
            {
                shots[i] = shot.gameObject;
                break;
            }
        }
    }

    /*
     * lays a new mine at the position the tank is atm and adds it to the array "mines" so that we know when a mine explodes and don't count it towards the limit anymore
     */
    private void LayMine()
    {
        Vector3 minePos = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject mine = Instantiate(minePrefab, minePos, transform.rotation).gameObject;
        for(int i=0; i<maxMines; ++i)
        {
            if (!mines[i])
            {
                mines[i] = mine;
                break;
            }
        }
    }

    /*
     * counts how many non-null objects are in the given array
     * 
     * @param arr array of GameObjects
     */
    private int Count(GameObject[] arr)
    {
        int c = 0;
        foreach (GameObject g in arr)
        {
            if (g) ++c;
        }
        return c;
    }
}
