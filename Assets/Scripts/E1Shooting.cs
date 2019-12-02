using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Shooting : MonoBehaviour
{
    public int maxShots = 5;
    //public int maxBounce;
    //0 is the primary mouse button, 1 the secondary, 2 the middle one
    public int shootButton = 0;
    public int rotationSpeed = 100;
    public float reloadTime;
    public const float inaccurracy = 10f;
    public bool justShot;
    public Rigidbody turret;
    public Rigidbody shellPrefab;
    public GameObject shotLocation;
    public Rigidbody[] shots;

    public float shotDirection;


    // Start is called before the first frame update
    void Start()
    {
        //to be  accustomed for each enemy tank
        reloadTime = 1f;
        shots = new Rigidbody[maxShots];
        GetNewShotDirection(1);
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime -= Time.deltaTime;
        if (justShot)
        {
            GetNewShotDirection(1);
            justShot = false;
        }
        if (Turn() && AbleToShoot()) //don't change this order!
        {
            Shoot();
        }
    }

    /*
     * shoots and adds the new shot to the Array "shots"
     */
    private void Shoot()
    {
        //animation and sounds
        Rigidbody shot = Instantiate(shellPrefab, shotLocation.transform.position, turret.transform.rotation);
        shot.GetComponent<ShellMovement>().SetMyTank(GetComponentInParent<BoxCollider>().gameObject);
        for(int i = 0; i < shots.Length; ++i) {
            if(shots[i] == null)
            {
                shots[i] = shot;
                break;
            }
        }
        justShot = true;
    }

    /*
     * calculates a new direction where the tank should shoot
     * This tank will always shoot directly at the player tank, bouncing off a wall
     * 
     * @return a float between 0 and 360 indicating the direction
     */
    private void GetNewShotDirection(int bounce)
    {
        shotDirection = Random.Range(0f, 360f);
    }

    /*
     * turns towards the "shotDirection" and indicates if the turret points to the intended direction
     * 
     * @ true if the y component of the rotation is within small bounds equal to "shotDirection"
     */
    private bool Turn()
    {
        if (Mathf.Abs(transform.eulerAngles.y - shotDirection) < inaccurracy) return true;
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime);
        return false;
    }

    /*
     * @return whether or not the tank is allowed to shoot, that means whether there are existing
     * less than "maxShots" shots in the world
     * can only return true every 1 to 5 seconds
     */
    private bool AbleToShoot()
    {
        if (reloadTime > 0) return false;
        int c = 0;
        foreach (Rigidbody r in shots)
        {
            if (r != null) ++c;
        }
        reloadTime = Random.Range(1f, 5f);
        print("c: " + c);
        return c < maxShots;
    }
}
