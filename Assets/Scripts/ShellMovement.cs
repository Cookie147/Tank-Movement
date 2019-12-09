using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public Rigidbody shell;
    public GameObject shot;
    public string type = "Normal";//to be automatized later

    private GameObject myTank;

    // Start is called before the first frame update
    void Start()
    {
        shell = GetComponent<Rigidbody>();
        shot = gameObject;
        speed = 8;
        float x = Mathf.Sin(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        float z = Mathf.Cos(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        shell.velocity = new Vector3(x, 1, z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || (other.CompareTag("Hay") && type == "Normal"))
        {
            ++bounceCount;
            if (bounceCount > maxBounce)
            {
                Destroy(shot);
                return;
            }
            Bounce(other);

        }
        else if (other.CompareTag("Enemy Tank"))
        {
            if (other.gameObject == myTank && bounceCount == 0)
            {
                return;
            }
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
        }
        else if (other.CompareTag("Shot"))
        {
            //some animation and sound

            Destroy(other);
            Destroy(shot);
        }
        else if (other.CompareTag("Player Tank"))
        {
            if (other.gameObject == myTank && bounceCount == 0)
            {
                print("do not destroy");
                return;
            }
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
        }
        else if (other.CompareTag("Hay"))
        {

        }
    }

    /**
     * bounces off a wall in a mathematically ideal way
     */
    private void Bounce(Collider other)
    {
        float angle;
        Vector3 dir = other.transform.position - shell.transform.position;
        float currentAngle = shell.transform.eulerAngles.y;
        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.z))
        {
            angle = 180 - currentAngle;
        }
        else if (Mathf.Abs(dir.z) < Mathf.Abs(dir.x))
        {
            angle = 360 - currentAngle;
        }
        else
        {
            angle = currentAngle + 180;
        }
        shot.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        shell.velocity = new Vector3(Mathf.Sin(angle * Mathf.PI / 180) * speed, 0, Mathf.Cos(angle * Mathf.PI / 180) * speed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (bounceCount == 0 && other.gameObject == myTank)
        {
            return;
        }
        if (other.CompareTag("Enemy Tank"))
        {
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
        }
        else if (other.CompareTag("Player Tank"))
        {
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
        }
    }

    public void SetMyTank(GameObject tank)
    {
        myTank = tank;
    }
}
