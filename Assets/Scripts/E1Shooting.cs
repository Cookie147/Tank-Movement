using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1Shooting : MonoBehaviour
{
    public int missileSpeed;
    public int maxShots, shots;
    public float time, shotReloadTime;
    //0 is the primary mouse button, 1 the secondary, 2 the middle one
    public int shootButton = 0;
    public float reloadTime, timeSinceShot;
    public Rigidbody turret;
    public Rigidbody shellPrefab;
    public GameObject shotLocation;


    // Start is called before the first frame update
    void Start()
    {
        //to be  accustomed for each enemy tank
        reloadTime = 0.3f;
        timeSinceShot = 0.2f;
        maxShots = 5;
        shotReloadTime = 1.2f;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateReload();
        if (timeSinceShot >= reloadTime && shots < maxShots)
        {

        }
    }

    private void UpdateReload()
    {
        time += Time.deltaTime;
        if (time > shotReloadTime)
        {
            if (shots > 0)
            {
                --shots;
                time %= shotReloadTime;
            }
        }
        timeSinceShot += Time.deltaTime;
    }

    private void Shoot()
    {
        //animation and sounds
        //Rigidbody shot = 
        Rigidbody shot = Instantiate(shellPrefab, shotLocation.transform.position, turret.transform.rotation);
        shot.GetComponent<ShellMovement>().SetMyTank(GetComponentInParent<BoxCollider>().gameObject);
    }
}
