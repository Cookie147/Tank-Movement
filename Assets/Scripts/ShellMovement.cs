using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public const float D = 0.75f;
    public const float armTime = 0.15f;
    public float age;
    public Rigidbody shell;
    public GameObject shot;
    public string type = "normal";//to be automatized later

    private bool stillInsideMyTank, insideMyTank;
    private GameObject myTank;

    // Start is called before the first frame update
    void Start()
    {
        shell = GetComponent<Rigidbody>();
        shot = shell.gameObject;
        speed = 8;
        stillInsideMyTank = true;
        float x = Mathf.Sin(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        float z = Mathf.Cos(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        shell.velocity = new Vector3(x, 1, z);
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;
        insideMyTank = false;
        //if (!armed) CheckArmed();
        print("1: " + insideMyTank);
    }
    /*
    private void LateUpdate()
    {
        if (!insideMyTank) stillInsideMyTank = false;
        print("3: " + stillInsideMyTank);
    }
    */
    /**
     * Checks unit collisions and reacts correspondingly:
     * wall: bounce if bounce limit has not yet been reached (destroy this shell otherwise)
     * hay: same as wall for normal type shells, set hay aflame if shell type is 'fast'
     * tank: destroy tank (and shell)
     * mine: do nothing as the mine itself checks for unit collisions and triggers if hit by a shell
     * (shield: destroy shell and weaken shield)
     */
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
            if (age < armTime) return;
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
            print("destroy tank");
        }
        else if (other.CompareTag("Shot"))
        {
            //some animation and sound

            Destroy(other);
            Destroy(shot);
        }
        else if (other.CompareTag("Player Tank"))
        {
            if (age < armTime) return;
            //explosion and sound
            Destroy(other.gameObject);
            Destroy(shot);
            print("destroy tank");
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

    private void CheckArmed()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == myTank) insideMyTank = true;
        print("2: " + insideMyTank + ", 2.1: " + stillInsideMyTank);
        if (bounceCount == 0)
        {
            //ReallyLateUpdate();
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

    private void ReallyLateUpdate()
    {
        if (!insideMyTank) stillInsideMyTank = false;
        print("3: " + stillInsideMyTank);
    }

    public void SetMyTank(GameObject tank)
    {
        myTank = tank;
    }
}
