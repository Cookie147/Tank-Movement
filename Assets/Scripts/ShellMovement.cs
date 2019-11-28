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
}
