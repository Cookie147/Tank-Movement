using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{

    public int maxShots;
    public int maxMines;
    public float shotReloadTime;
    //0 is the primary mouse button, 1 the secondary, 2 the middle one
    public int shootButton = 0;
    public string mineButton;
    public float timeSinceShot;
    public Rigidbody turret;
    public Rigidbody shellPrefab;
    public Rigidbody minePrefab;
    public GameObject shotLocation;
    public GameObject[] shots;
    public GameObject[] mines;


    // Start is called before the first frame update
    void Start()
    {
        maxShots = GameObject.Find("Storage").GetComponent<GameManager>().getNumShots();
        if (maxShots == 0) maxShots = 5;
        print(maxShots);
        mines = new GameObject[maxMines];
        shots = new GameObject[maxShots];
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceShot -= Time.deltaTime;

        if (timeSinceShot < 0 && Count(shots) < maxShots && Input.GetMouseButtonDown(shootButton))
        {
            Shoot();
            timeSinceShot = shotReloadTime;
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

    /*
     * for different difficulties the player tank has differently many shots at its disposal
     * via this function the game menu will be able to change this
     */
    public void SetMaxShots(int newMax)
    {
        maxShots = newMax;
    }
}
