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
    public const float maxRayDistance = 55f;
    public const float inaccurracy = 10f;
    public bool justShot;
    public Rigidbody turret;
    public Rigidbody shellPrefab;
    public Rigidbody[] shots;
    public GameObject shotLocation;
    public GameObject playerTank;

    private float shotDirection;
    private bool randomDirection;


    // Start is called before the first frame update
    void Start()
    {
        //to be  accustomed for each enemy tank
        reloadTime = 1f;
        playerTank = GameObject.Find("Player Tank");
        shots = new Rigidbody[maxShots];
        GetNewShotDirection(1);
    }

    // Update is called once per frame
    void Update()
    {
        reloadTime -= Time.deltaTime;
        GetNewShotDirection(1);//to be added when it is finished
        if (justShot)
        {
            //GetNewShotDirection(1);//to be deleted
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
        reloadTime = Random.Range(1f, 5f);
        justShot = true;
        randomDirection = false;
    }

    /*
     * calculates a new direction where the tank should shoot
     * This tank will always shoot directly at the player tank, bouncing off a wall if necessary
     * 
     * @var shotDirection will contain a float between 0 and 360 indicating the direction
     */
    private void GetNewShotDirection(int bounce)
    {
        //at first, check if the player can be directly hit
        if(playerTank != null)
        {
            Vector3 direction = playerTank.transform.position - transform.position;
            direction.y = 0;
            if(Physics.Raycast(transform.position, direction, out RaycastHit hit, maxRayDistance))
            {
                print(hit.collider.tag + " was hit in direction " + direction.ToString());
                if(hit.collider.CompareTag("Player Tank"))
                {
                    int side = playerTank.transform.position.x > transform.position.x ? 1 : -1;
                    shotDirection = (Vector3.Angle(Vector3.forward, direction) * side + 360) % 360;
                    print("player can be accessed easily, the angle is " + shotDirection);
                    return;
                }
            }
        }
        //stores the best direction found, will stay -1 if no angle hitting the player is found
        int bestDirection = -1;
        //a score how good the current direction in "bestDirection" is to compare if a new hit is even better or not
        //int heuristicScore = 0;

        //check all around for a possibility to hit the player
        for (int i = 0; i < 360; i += 5)
        {
            Vector3 directionToCheck = Quaternion.Euler(new Vector3(0, i, 0)) * Vector3.forward;
            print("checking: " + i + " from " + transform.position.ToString());
            //the first raycast that will most likely hit a wall, or maybe a player or mine (ignored so far)
            if (Physics.Raycast(transform.position, directionToCheck, out RaycastHit hit, Mathf.Infinity))
            {
                //if the player is hit in this direction, that is the best option we can possibly find, so save the angle and end the method
                if (hit.collider.CompareTag("Player Tank"))
                {
                    bestDirection = i;
                    print("direct player hit detected in direction " + bestDirection);
                    break;
                }
                //if a wall is hit, we'll have to check with a new raycast if a shot that bounces off the wall would hit something useful
                //if bounce is 0, the shot will be destroyed at this point, so it is useless to check anything else
                if (hit.collider.CompareTag("Wall") && bounce != 0)
                {
                    //this could be done recusrively to be able to perform arbitrarily many bounces. Here, however, it will be implemented for 1 bounce only
                    if (Physics.Raycast(hit.point, Vector3.Reflect(directionToCheck, hit.normal), maxRayDistance))
                    {
                        //if the player is hit now, that is the best option we can find now, so save the INITIAL angle (not the one of the second Raycast) and end the method
                        if (hit.collider.CompareTag("Player Tank"))
                        {
                            print("player hit detected after 1 hit");
                            bestDirection = i;
                            break;
                        }
                    }
                    //copy mine check from below
                }
                else if (hit.collider.CompareTag("Mine"))
                {
                    //check whether the player is inside the mine's explosion radius. If so, then this is a good option (or even better than the player directly?)
                }
            }
            else print("no collision found");
        }//for

        //if, after checking the whole circle, no interesting hit has been found, return some random direction that will stay the same until a shot is fired (either in this direction or towards the player)
        if(bestDirection == -1 && !randomDirection)
        {
            shotDirection = Random.Range(0f, 360f);
            randomDirection = true;
            print("random direction: " + shotDirection);

            //check if it will shoot itself in this direction
        }
        else
        {
            shotDirection = bestDirection;
            print("shotDirection is now: " + shotDirection);
        }
    }

    /*
     * turns towards the "shotDirection" and indicates if the turret points to the intended direction
     * 
     * @ true if the y component of the rotation is, within small bounds "inaccurracy", equal to "shotDirection"
     */
    private bool Turn()
    {
        
        float currentAngle = (transform.eulerAngles.y + 360) % 360;
        /*
        if (Mathf.Abs(currentAngle - shotDirection) < inaccurracy) return true;
        Vector3 targetDirection = Quaternion.Euler(new Vector3(0, shotDirection, 0)) * Vector3.forward;
        print("target direction is " + targetDirection.ToString());
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotationSpeed * Time.deltaTime * Mathf.Deg2Rad, 0.0f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newDirection), rotationSpeed * Time.deltaTime);
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, newDirection, rotationSpeed * Time.deltaTime);
        //transform.Rotate
        return false;
        //*/
        //*
        //print("current angle is: " + currentAngle);
        if (Mathf.Abs(currentAngle - shotDirection) < inaccurracy) return true;
        //int dir = Vector3.SignedAngle(transform.eulerAngles, Quaternion.Euler(new Vector3(0, shotDirection, 0)) * Vector3.forward, Vector3.up) > 0 ? 1 : -1;
        int dir = 0;
        transform.LookAt(transform.rotation.eulerAngles + new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        //transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime * dir);
        //print("rotated from " + currentAngle + " to " + transform.eulerAngles.y);
        return false;
        //*/
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
        //print("c: " + c);
        return c < maxShots;
    }
}
