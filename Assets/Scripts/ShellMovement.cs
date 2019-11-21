using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public const float D = 0.75f;
    public Rigidbody shell;
    public GameObject shot;
    public string type = "normal";//to be automatized later

    // Start is called before the first frame update
    void Start()
    {
        shell = GetComponent<Rigidbody>();
        shot = shell.gameObject;
        speed = 8;
        float x = Mathf.Sin(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        float z = Mathf.Cos(shell.rotation.eulerAngles.y * Mathf.PI / 180) * speed;
        shell.velocity = new Vector3(x, 1, z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        else if(other.CompareTag("Hay"))
        {

        }
        else if(other.CompareTag("Enemy Tank"))
        {
            
        }
        else if(other.CompareTag("Shot"))
        {
            //some animation

            Destroy(other);
            Destroy(shot);
        }
    }

    /**
     * bounces off a wall in a mathematically ideal way
     */
    private void Bounce(Collider other)
    {
        /*
        Vector3 shellv = shell.transform.eulerAngles;
        Vector3 wallv = other.transform.eulerAngles;
        float a = Vector3.Angle(shellv, wallv);

        print("shell: " + shellv.ToString() + ", wall: " + wallv.ToString() + ", angle: " + a);
        */
        //Vector3 v = shell.velocity;
        //shell.velocity = new Vector3(v.x, v.y, -v.z);

        float angle = 0;
        Vector3 dir = other.transform.position - shell.transform.position;
        if (dir.x < dir.z)
        {
            dir.z = 0;
        }
        else if (dir.z < dir.x)
        {
            dir.x = 0;
        }
        else
        {
            angle = 180;
            shot.transform.Rotate(0, angle, 0);
            return;
        }
        shell.transform.rotation = Quaternion.Euler(Vector3.Reflect(shell.transform.eulerAngles, dir));
        //shell.velocity = Vector3.Reflect(shell.transform.eulerAngles, dir);



        /*
        float angle = shell.transform.eulerAngles.y;
        if(angle % 90 == 0)
        {
            //only one collision is possible
            //⇒ rotate by 180° 
            //not tested yet
            shell.transform.rotation = Quaternion.Euler(new Vector3(0, (shell.transform.eulerAngles.y + 180) % 360, 0));
        }
        if(angle < 180)
        {

        }
        else if(angle > 180)
        {

        }


        switch((angle + 45) % 90)
        {
            case 1: 
                break;
            default: 
                break;
        }
        */
    }
}
